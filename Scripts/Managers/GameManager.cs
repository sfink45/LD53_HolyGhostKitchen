using Assets.Scripts.RecipeMiniGames;
using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameUIManager GameUiManager { get; internal set; }

    [SerializeField] List<BaseRecipe> _possibleRecipes;
    [SerializeField] List<BaseRecipe> _queuedRecipes;
    [SerializeField] GameObject _offScreenStaging;
    [SerializeField] float _newOrderCooldown;
    [SerializeField] private float flubRange;
    [SerializeField] CameraShake _cameraShake;
    [SerializeField] int minPerfectScore;
    [SerializeField] int minGoodScore;
    [SerializeField] int minOkayScore;
    [SerializeField] GameObject _bag;


    [SerializeField] int _ordersComplete;
    [SerializeField] float maxTime;
    bool loadingScene = false;


    public int OrdersCompleted { get => _ordersComplete; }
    public float TotalTime { get; private set; }

    float _timeLeft;

    bool inMainGameLoop = false;

    float _currentOrderCooldown;
    BaseRecipe _activeRecipe { get => _activeOrderIndex >= _queuedRecipes.Count ? null : _queuedRecipes[_activeOrderIndex]; }
    int _activeOrderIndex = 20;
    int _maxOrders;

    Background _bg;

    bool done = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += HandleSceneLoad;
        }
        else
        {
            SceneManager.sceneLoaded -= HandleSceneLoad;
            Destroy(gameObject);
        }



    }

    private void HandleSceneLoad(Scene scene, LoadSceneMode arg1)
    {
        if (scene.buildIndex == 1)
        {
            done = false;
            _ordersComplete = 0;
            _timeLeft = maxTime;
            TotalTime = maxTime;
            _currentOrderCooldown = _newOrderCooldown;
            inMainGameLoop = true;
            GameUiManager = FindAnyObjectByType<GameUIManager>();
            _bg = FindAnyObjectByType<Background>();
            _maxOrders = GameUiManager._orderHolders.Count;
            //if (_cameraShake == null) _cameraShake = FindFirstObjectByType<CameraShake>();
            loadingScene = false;
        }
        else
        {
            //Destroy(this.gameObject);
        }
    }

    private void Start()
    {

    }

    void Update()
    {
        if (!inMainGameLoop || loadingScene || done) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameUiManager.IsPaused())
            {
                Unpause();
            }
            else
            {
                Time.timeScale = 0.01f;
                GameUiManager.DisplayPause();
            }
        }

        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f && !done)
        {
            done = true;
            GameUiManager.ShowGameOverScreen();
        }

        GameUiManager.UpdateTimeDisplay(_timeLeft, maxTime);

        if (Input.GetKeyDown(KeyCode.Space))
            SubtleScreenShake();

        if (ShouldSpawnNewRecipe(Time.deltaTime))
        {
            if (_queuedRecipes.Count < _maxOrders)
            {
                _queuedRecipes.Add(OrderRandomRecipe());
                SetIcon(_queuedRecipes[_queuedRecipes.Count - 1], _queuedRecipes.Count - 1);
            }
            else
            {
                var index = _queuedRecipes.FindIndex(0, r => r == null);
                if (index >= 0)
                {
                    _queuedRecipes[index] = OrderRandomRecipe();
                    SetIcon(_queuedRecipes[index], index);
                }
            }
        }

        if (_activeRecipe == null)
        {
            for (int i = 0; i < _maxOrders; ++i)
            {
                var keycodeValue = (int)KeyCode.Alpha1 + i;
                var keyCode = (KeyCode)keycodeValue;
                if (Input.GetKeyDown(keyCode))
                {
                    _activeOrderIndex = i;
                    if (_activeRecipe == null)
                    {
                        _activeOrderIndex = 20;
                        continue;
                    }
                    if (!_activeRecipe.RecipeStepObject.IsInteractiveStep) _activeOrderIndex = 20;
                    _queuedRecipes[i].Activate();
                }
            }
        }
        else if (_activeRecipe.IsComplete)
        {
            AlertScore(_activeRecipe);
            GameUiManager.RemoveIcon(_activeOrderIndex);
            Destroy(_activeRecipe.gameObject);
            _queuedRecipes[_activeOrderIndex] = null;
            _activeOrderIndex = 20;
            _ordersComplete++;
        }

        for (int i = 0; i < _queuedRecipes.Count; ++i)
        {
            var r = _queuedRecipes[i];
            if (r == null) continue;
            if (r.IsComplete)
            {
                AlertScore(r);
                _ordersComplete++;
                GameUiManager.RemoveIcon(i);
                Destroy(r.gameObject);
                _queuedRecipes[i] = null;
            }
            else r.HandleUpdate();
        }
    }

    internal void SendFeedback(StepRank.Ranks rank, string feedback)
    {
        var mainText = rank.ToString();
        
        SendFeedback(mainText, feedback);
    }

    private void AlertScore(BaseRecipe r)
    {
        Instantiate(_bag, new Vector3(_bag.transform.position.x + UnityEngine.Random.Range(-10, 10), _bag.transform.position.y), Quaternion.identity, null);
        AudioManager.Instance.PlaySound("ding");
        GameUiManager.UpdateScoreDisplay(++_ordersComplete, _ordersComplete);
        var score = r.GetScoreAverage();
        Debug.Log(score);
        var timeAdded = 1;
        if (score >= minPerfectScore)
        {
            timeAdded = 10;
        }
        else if (score >= minGoodScore)
        {
            timeAdded = 5;
        }
        else if (score > minOkayScore)
        {
            timeAdded = 3;
        }
        _timeLeft += timeAdded;
        TotalTime += timeAdded;

        GameUiManager.ShowTimeAdded(timeAdded);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        GameUiManager.DisplayPause();
    }

    internal void SendFeedback(string mainText, string hintText)
    {
        GameUiManager.ShowFeedback(mainText, hintText);
    }

    public void ScreenShake(float intensity, float time)
    {
        _bg.transform.DOShakeScale(time, intensity * .5f);
        //_cameraShake.Shake(intensity, time);
    }

    public void SubtleScreenShake()
    {
        ScreenShake(.25f, .25f);
    }

    public void SuperSublteScreenShake()
    {
        ScreenShake(.1f, .25f);
    }

    private bool ShouldSpawnNewRecipe(float delta)
    {
        _currentOrderCooldown += delta + UnityEngine.Random.Range(-flubRange, flubRange);
        return _currentOrderCooldown >= _newOrderCooldown;
    }

    public void SetIcon(BaseRecipe recipe, int index)
    {
        AudioManager.Instance.PlaySound("printer");
        //GameUiManager.AddIcon(recipe.Icon, index);
        // I really f'n hate this but it works and that's all i have time for...
        recipe.SendIconToUI(GameUiManager._orderHolders[index].GetComponentInChildren<OrderHolder>().gameObject, GameUiManager._startX, GameUiManager._startY, GameUiManager._moveToY);
    }

    private BaseRecipe OrderRandomRecipe()
    {
        _currentOrderCooldown = 0f;
        var recipe = _possibleRecipes[UnityEngine.Random.Range(0, _possibleRecipes.Count)];
        var recipego = Instantiate(recipe);
        recipego.OrderIn(_offScreenStaging.transform.position);
        return recipego;
    }

    public void ClearRecipe()
    {
        _activeOrderIndex = 20;
    }

    public void LoadScene(int index)
    {
        done = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public TipPanel ShowTipPanel(TipPanel panel, RecipeStep step)
    {
        var tipPanel = Instantiate(panel, GameUiManager.transform) as TipPanel;
        tipPanel.Setup(step);
        return tipPanel;
    }

}
