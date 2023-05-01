using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI description;
    [SerializeField] protected Button button;
    public void Init()
    {
        button.onClick.AddListener(HandleOnClick);

        var templateText = description.text;
        templateText = templateText.Replace("X", GameManager.Instance.OrdersCompleted.ToString());
        templateText = templateText.Replace("Q", GameManager.Instance.TotalTime.ToString("0"));
        description.text = templateText;
    }

    private void HandleOnClick()
    {
        GameManager.Instance.LoadScene(0);
    }
}
