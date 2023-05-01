using Assets.Scripts.RecipeMiniGames;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreefallToppingStep : RecipeStep
{
    [SerializeField] string _stepTitle;
    [SerializeField] float _handForce;
    [SerializeField] Rigidbody2D _hand;
    [SerializeField] List<FreefallTopping> toppings;
    [SerializeField] GameObject _parent;
    [SerializeField] TipPanel _tipPanel;
    private GameObject _panelGameObject;

    private void Start()
    {
        gameObject.SetActive(false);
        var temp = GameManager.Instance.ShowTipPanel(_tipPanel, this);
        temp.Setup(_stepTitle, "");
        _panelGameObject = temp.gameObject;
        _panelGameObject.SetActive(false);
        IsInteractiveStep = true;
    }
    public override void Activate()
    {
        IsActivated = true;
        _ranks = new List<StepRank.Ranks>();

        _panelGameObject.SetActive(true);
        if (IsInteractiveStep) transform.DOMove(Vector3.zero, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            gameObject.SetActive(true);
            //_panelGameObject.SetActive(false);
            });
    }
    public override void HandleActiveUpdate(float deltaTime)
    {
        if(toppings.TrueForAll(t => t._isLanded))
        {
            SetScore();
            StepCompleted();
        }

        if(RecipeInput.IsMoveRight())
        {
            _hand.AddForce(new Vector2(_handForce * deltaTime, 0));
        }
        else if(RecipeInput.IsMoveLeft())
        {
            _hand.AddForce(new Vector2(-_handForce * deltaTime, 0));
        }
    }

    private void SetScore()
    {
        foreach(var t in toppings)
        {
            StepRank.Ranks rankToAdd = StepRank.Ranks.Okay;

            switch(t._on)
            {
                case "Target":
                    rankToAdd = StepRank.Ranks.Perfect;
                    break;
                case "Table":
                    rankToAdd = StepRank.Ranks.Good;
                    break;
                case "Ground":
                    rankToAdd = StepRank.Ranks.Poor;
                    break;
            }

            _ranks.Add(rankToAdd);
        }
    }

    public override void Destroy()
    {

        Destroy(_panelGameObject.gameObject);
        Destroy(gameObject);
    }
}
