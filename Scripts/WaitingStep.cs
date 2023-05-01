using Assets.Scripts.RecipeMiniGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingStep : RecipeStep
{
    float _currentWaitingTime;
    private void Start()
    {
        IsInteractiveStep = false;
    }

    public override void Activate()
    {
        if(!IsActivated)
        {
            base.Activate();
            AudioManager.Instance.PlaySound("fry");
        }
        else
        {
            GetScore();
            StepCompleted();
        }
    }

    private void GetScore()
    {
        var result = CalculateScore(_currentWaitingTime, false, "Too Long", "Not Long Enough");
        GameManager.Instance.SendFeedback(result.rank, result.feedback);
    }

    public override void HandlePassiveUpdate(float deltaTime, Image Icon)
    {
        _currentWaitingTime += deltaTime;
        Icon.fillAmount = (_currentWaitingTime / _targetValue);

    }

    public override void Destroy()
    {
        Destroy(gameObject);
    }
}
