using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _optionsPanel;

    public void StartGame() => GameManager.Instance.LoadScene(1);

    public void QuitGame() => GameManager.Instance.QuitGame();

    public void ShowOptions()
    {
        _mainPanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        _mainPanel.SetActive(true);
        _optionsPanel.SetActive(false);
    }
}
