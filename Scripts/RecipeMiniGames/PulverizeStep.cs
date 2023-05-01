using Assets.Scripts.RecipeMiniGames;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulverizeStep : RecipeStep
{
    [SerializeField] string _stepTitle;
    [SerializeField] GameObject _pestal;
    [SerializeField] List<SpriteRenderer> _mortarStages;
    [SerializeField] List<RecipeInput.KeyboardActions> _actions;
    [SerializeField] float _strikeDistance;
    [SerializeField] float _strikeSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _startYPosRotPoint;
    [SerializeField] float _fadeDuration;
    [SerializeField] TipPanel _tipPanel;
    

    RecipeInput.KeyboardActions _lastInput = RecipeInput.KeyboardActions.Empty;
    RecipeInput.KeyboardActions _firstCircleInput = RecipeInput.KeyboardActions.Empty;

    int _currentActionIndex = 0;
    int _flubs = 0;
    private GameObject _panelGameObject;
    float yStartPos;

    private void Start()
    {

        var temp = GameManager.Instance.ShowTipPanel(_tipPanel, this);
        temp.Setup(_stepTitle, GetInstructions());
        _panelGameObject = temp.gameObject;
        _panelGameObject.SetActive(false);
        IsInteractiveStep = true;
        for (int i = 1; i < _mortarStages.Count; ++i)
        {
            _mortarStages[i].color = new Color(1, 1, 1, 0);
        }
        yStartPos = _pestal.transform.localPosition.y;
    }

    private string GetInstructions()
    {
        var result = "";
        foreach(var a in _actions)
        {
            result += $"{a.ToString()} ";
        }
        return result;
    }

    public override void HandleActiveUpdate(float deltaTime)
    {
        if (_currentActionIndex >= _actions.Count) return;

        if (Input.anyKeyDown)
        {
            if (_actions[_currentActionIndex] == RecipeInput.KeyboardActions.CircleRight ||
               _actions[_currentActionIndex] == RecipeInput.KeyboardActions.CircleLeft)
            {
                if (_lastInput == RecipeInput.KeyboardActions.Empty)
                {

                    _lastInput = RecipeInput.GetActionFromKeyCode();
                    _firstCircleInput = _lastInput;
                    ExecuteAction(_actions[_currentActionIndex], true);
                }
                else
                {
                    var nextInput = RecipeInput.GetNextInCircle(_lastInput, _actions[_currentActionIndex]);
                    if (nextInput == RecipeInput.GetActionFromKeyCode())
                    {

                        ExecuteAction(_actions[_currentActionIndex]);
                        if (RecipeInput.GetNextInCircle(nextInput, _actions[_currentActionIndex]) == _firstCircleInput)
                        {
                            _lastInput = RecipeInput.KeyboardActions.Empty;
                            _firstCircleInput = RecipeInput.KeyboardActions.Empty;
                            ++_currentActionIndex;
                            TransitionSprite();
                        }
                        else _lastInput = nextInput;


                    }
                }
            }
            else if (RecipeInput.IsKeyboardAction(_actions[_currentActionIndex]))
            {
                ExecuteAction(_actions[_currentActionIndex]);
                ++_currentActionIndex;
                TransitionSprite();
            }
        }
    }

    protected virtual void ExecuteAction(RecipeInput.KeyboardActions keyCode, bool isFirstCircleMove = false)
    {
        _ranks.Add(StepRank.Ranks.Perfect);
        switch (keyCode)
        {
            case RecipeInput.KeyboardActions.Down:
            case RecipeInput.KeyboardActions.Up:
            case RecipeInput.KeyboardActions.Left:
            case RecipeInput.KeyboardActions.Right:
                _pestal.transform.DOLocalMoveY(-_strikeDistance, _strikeSpeed).OnComplete(() => _pestal.transform.DOLocalMoveY(yStartPos, _strikeSpeed * .5f));
                AudioManager.Instance.PlaySound("smash");
                GameManager.Instance.SubtleScreenShake();
                break;
            case RecipeInput.KeyboardActions.CircleLeft:
                if(isFirstCircleMove)
                    _pestal.transform.DOLocalMoveY(_startYPosRotPoint, _strikeSpeed * .5f).OnComplete(() => ExecuteAction(_actions[_currentActionIndex]));
                else
                _pestal.transform.DORotate(new Vector3(0, 0, -90f), _rotationSpeed, RotateMode.LocalAxisAdd);

                AudioManager.Instance.PlaySound("scrape");

                break;
            case RecipeInput.KeyboardActions.CircleRight:
                if(isFirstCircleMove)
                    _pestal.transform.DOLocalMoveY(_startYPosRotPoint, _strikeSpeed * .5f).OnComplete(() => ExecuteAction(_actions[_currentActionIndex]));
                else
                _pestal.transform.DORotate(new Vector3(0, 0, 90f), _rotationSpeed, RotateMode.LocalAxisAdd);

                AudioManager.Instance.PlaySound("scrape");
                break;

        }
    }

    public void TransitionSprite()
    {
        _mortarStages[_currentActionIndex].color = new Color(1, 1, 1, 1);
        if (_mortarStages.Count - 1 > _currentActionIndex)
        {
            _mortarStages[_currentActionIndex - 1].DOColor(new Color(1, 1, 1, 0), _fadeDuration);
        }
        else
        {
            _mortarStages[_currentActionIndex - 1].DOColor(new Color(1, 1, 1, 0), _fadeDuration).OnComplete(() => {
                GameManager.Instance.SendFeedback("Perfect", "");
                StepCompleted();
                });
        }
    }

    public override void Destroy()
    {
        Destroy(_panelGameObject);
        Destroy(gameObject);
    }

    public override void Activate()
    {
        base.Activate();
        _panelGameObject.SetActive(true);
    }

}
