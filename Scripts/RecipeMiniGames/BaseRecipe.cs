using Assets.Scripts.RecipeMiniGames;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BaseRecipe : MonoBehaviour
{
    public bool IsActiveRecipe { get; set; } = false;
    public bool IsComplete { get; internal set; }

    public List<RecipeStep> RecipeSteps;
    public RecipeStep RecipeStepObject;

    public List<float> Scores = new List<float>();

    protected int _currentStep = 0;

    public Image Icon;

    private void Start()
    {
        IsActiveRecipe = false;
    }

    public void HandleUpdate()
    {
        if (!RecipeStepObject.IsActivated) return;
        if (IsActiveRecipe) RecipeStepObject.HandleActiveUpdate(Time.deltaTime);
        else RecipeStepObject.HandlePassiveUpdate(Time.deltaTime, Icon);
    }

    public void OrderIn(Vector3 position)
    {
        RecipeStepObject = Instantiate(RecipeSteps[_currentStep], position, Quaternion.identity, null);
        RecipeStepObject.OnStepCompleted += OnStepCompleted;
        //if(Icon == null)
        //Icon = Instantiate(RecipeStepObject._icon);
        //Icon.name = "OrderIn";
        IsActiveRecipe = false;
    }

    public void Activate()
    {

        if (IsActiveRecipe) return;

        if (_currentStep >= RecipeSteps.Count)
        {
            Debug.Log("Order Completed");
            return;
        }
        IsActiveRecipe = RecipeStepObject.IsInteractiveStep;
        if (!RecipeStepObject.IsActivated)
            RecipeStepObject.Activate();
        else if (!RecipeStepObject.IsInteractiveStep)
            RecipeStepObject.Activate();
    }

    private void OnStepCompleted(float score)
    {
        Scores.Add(score);
        if(RecipeStepObject.IsInteractiveStep) GameManager.Instance.ClearRecipe();
        RecipeStepObject.Destroy();
        _currentStep++;

        if (_currentStep >= RecipeSteps.Count) IsComplete = true;
        else
        {
            OrderIn(new Vector3(700, -100));
            Icon.sprite = RecipeStepObject._icon.sprite;
            Icon.name = "on step complete";
        }
    }

    internal float GetScoreAverage() => Scores.Average() * 100;

    internal void SendIconToUI(GameObject gameObject, float x, float y, float dest)
    {
        if(Icon == null)
        {
            Icon = Instantiate(RecipeStepObject._icon, gameObject.transform);
            Icon.gameObject.transform.SetParent(gameObject.transform);
            //Icon.transform.SetParent(_orderHolders[index].GetComponentInChildren<OrderHolder>().transform);
            Icon.transform.localPosition = new Vector3(x, y);
            Icon.transform.DOLocalMoveY(dest, 2f);
        }
    }
}
