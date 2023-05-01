using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameOverScreen _gameOver;
    [SerializeField] public List<GameObject> _orderHolders;
    [SerializeField] public float _startX, _startY, _moveToY;
    [SerializeField] HudDisplay _timeDisplay;
    [SerializeField] HudDisplay _scoreDisplay;
    [SerializeField] GameObject _gameScreen;

    [SerializeField] protected TextMeshProUGUI addedTimeText;
    [SerializeField] protected TextMeshProUGUI feedbackText;

    public void AddIcon(Image icon, int index)
    {
        icon.transform.SetParent(_orderHolders[index].GetComponentInChildren<OrderHolder>().transform);
        icon.transform.localPosition = new Vector3(_startX, _startY);
        icon.transform.DOLocalMoveY(_moveToY, 2f);
    }

    public void RemoveIcon(int index)
    {
        var delete = _orderHolders[index].GetComponentInChildren<HudIcon>();
        Destroy(delete.gameObject);
    }

    internal bool IsPaused() => _pauseMenu.active;

    internal void DisplayPause()
    {
        _pauseMenu.SetActive(!IsPaused());
    }

    internal void UpdateTimeDisplay(float currentTime, float maxTime)
    {
        _timeDisplay.UpdateDisplay(currentTime, maxTime);
    }
    internal void UpdateScoreDisplay(float currentTime, float maxTime)
    {
        _scoreDisplay.UpdateDisplay(currentTime, maxTime, false);
    }

    public void ShowTimeAdded(float timeAdded)
    {
        addedTimeText.gameObject.SetActive(true);
        addedTimeText.text = $"+{timeAdded}";
        addedTimeText.gameObject.transform.DOShakeRotation(1f, 90).OnComplete(() => addedTimeText.gameObject.SetActive(false));
    }

    public void ShowFeedback(string mainFeedback, string additionalFeedback)
    {
        feedbackText.DOComplete();
        feedbackText.gameObject.SetActive(true);
        feedbackText.text = string.IsNullOrEmpty(additionalFeedback) ? mainFeedback : $"{mainFeedback} ({additionalFeedback})";
        feedbackText.gameObject.transform.DOShakeRotation(1f, 2).OnComplete(() => feedbackText.gameObject.SetActive(false));
    }

    internal void ShowGameOverScreen()
    {
        var panels = gameObject.GetComponentsInChildren<TipPanel>();

        foreach (var p in panels) p.gameObject.SetActive(false);

        _gameScreen.SetActive(false);
        _gameOver.Init();
        _gameOver.gameObject.SetActive(true);
    }

    public void Unpause()
    {
        GameManager.Instance.Unpause();
    }
}
