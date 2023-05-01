using Assets.Scripts.RecipeMiniGames;
using TMPro;
using UnityEngine;

public class TipPanel : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _titleLabel;
    [SerializeField] protected TextMeshProUGUI _headingLabel;

    public virtual void Setup(RecipeStep step) { }
    public virtual void Setup(string title, string text)
    {
        if(_headingLabel != null) _headingLabel.text = title;
        if(_titleLabel != null) _titleLabel.text = text;
    }
}