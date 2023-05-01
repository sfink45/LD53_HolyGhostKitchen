using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudDisplay : MonoBehaviour
{
    [SerializeField] protected Image _bg;
    [SerializeField] protected TextMeshProUGUI _text;

    internal void UpdateDisplay(float current, float max, bool floatingPointDisplay = true)
    {
        var format = (current < 10 && floatingPointDisplay) ? "0.0" : "0";

        _bg.fillAmount = current / max;
        _text.text = current.ToString(format);
    }
}
