using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.RecipeMiniGames;
using TMPro;
using UnityEngine;

public class ChopPanel : TipPanel
{

    public void SetText(ChoppingStep.ChopStyle chopStyle)
    {
        var displayString = "X";
        switch (chopStyle)
        {
            case ChoppingStep.ChopStyle.Rough:
                displayString = "ROUGH";
                break;
            case ChoppingStep.ChopStyle.Medium:
                displayString = "MEDIUM";
                break;
            case ChoppingStep.ChopStyle.Fine:
                displayString = "FINE";
                break;
        }
        _titleLabel.text = _titleLabel.text.Replace("X", displayString);
    }

    public override void Setup(RecipeStep step)
    {
        var chopStep = step as ChoppingStep;
        SetText(chopStep._chopStyle);
    }
}
