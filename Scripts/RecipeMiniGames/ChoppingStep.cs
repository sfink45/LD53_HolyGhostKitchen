using Assets.Scripts.RecipeMiniGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ChoppingStep : RecipeStep
{
    public enum ChopStyle { Rough, Medium, Fine }


    public ChopStyle _chopStyle;
    [SerializeField] GameObject _handObject;
    [SerializeField] GameObject _cuttableObject;
    [SerializeField] GameObject _knifeObject;
    [SerializeField] float _handSpeed;
    [SerializeField] float _knifeSpeed;
    [SerializeField] float _knifeCutLength;
    [SerializeField] float _cutWidth;
    [SerializeField] float _cutStart;
    [SerializeField] Vector2 PivotCut;
    [SerializeField] float cutPosition;
    [SerializeField] float maxcutPosition;
    [SerializeField] float maxHandPosition;
    [SerializeField] float handPosition;
    [SerializeField] float offset;
    [SerializeField] float roughChopWidth;
    [SerializeField] float mediumChopWidth;
    [SerializeField] float fineChopWidth;

    [SerializeField] ChopPanel _tipPanel;
    [SerializeField] SpriteRenderer _cuttableRenderer;

    float lastCut = 256;
    Texture2D _texture;
    GameObject _panelGameObject;

    bool _wasCut = false;

    private void Start()
    {
        switch (_chopStyle)
        {
            case ChopStyle.Rough:
                _targetValue = roughChopWidth;
                break;
            case ChopStyle.Medium:
                _targetValue = mediumChopWidth;
                break;
            case ChopStyle.Fine:
                _targetValue = fineChopWidth;
                break;
        }
        IsInteractiveStep = true;
        _texture = _cuttableRenderer.sprite.texture;
        maxcutPosition = _texture.width;
        _panelGameObject = GameManager.Instance.ShowTipPanel(_tipPanel, this).gameObject;
        _panelGameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        _panelGameObject.SetActive(true);
    }

    public override void HandleActiveUpdate(float deltaTime)
    {
        if (RecipeInput.IsMoveRight())
        {
            //// Move the hand right
            //Debug.Log("hand move right");


            //_handObject.transform.position += new Vector3(_handSpeed * deltaTime, 0);

        }
        else if (RecipeInput.IsMoveLeft())
        {
            // Move the hand left
            //Debug.Log("hand move right");
            var movementFactor = _handSpeed * deltaTime;
            if (cutPosition < maxcutPosition)
            {
                handPosition += movementFactor;
                cutPosition = maxcutPosition * (handPosition / maxHandPosition);
                _handObject.transform.position -= new Vector3(movementFactor, 0);
            }
        }

        if (RecipeInput.IsChopAction())
        {
            // Trigger the chop
            //Debug.Log("chop");
            AudioManager.Instance.PlaySound("chop");
            GameManager.Instance.SuperSublteScreenShake();
            CalculateChopScore(_targetValue, _targetValue == fineChopWidth);
            _knifeObject.transform.DOComplete(true);
            //_knifeObject.transform.localPosition = new Vector3(_knifeObject.transform.position.x, _knifeObject.transform.position.y + _knifeCutLength);
            _knifeObject.transform.DOLocalMoveY(-_knifeCutLength, _knifeSpeed).OnComplete(() => _knifeObject.transform.DOLocalMoveY(1, _knifeSpeed * .1f));

            SplitCuttableObject();
        }

        if(cutPosition >= maxcutPosition )
        {
            Complete();
        }
    }

    private void Complete()
    {
        StepCompleted();
    }

    private void CalculateChopScore(float targetChopWidth, bool allowSmallerChops = false)
    {
        var sliceWidth = 256 - cutPosition;
        var scoringSlice = lastCut - sliceWidth;
        //Debug.Log(scoringSlice);
        var result = CalculateScore(scoringSlice, allowSmallerChops);
        GameManager.Instance.SendFeedback(result.rank, result.feedback);
    }

    private void SplitCuttableObject()
    {
        var sliceWidth = 256 - cutPosition;
        var newSprite = Sprite.Create(_texture, new Rect(0, 0, sliceWidth, 256), PivotCut);
        var pieceSprite = Sprite.Create(_texture, new Rect(sliceWidth, 0, lastCut - sliceWidth, 256), PivotCut);
        lastCut = sliceWidth;
        var go = new GameObject();
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = newSprite;
        sr.sortingLayerName = ("MiniGame_Actor");
        go.transform.position = new Vector3(_handObject.transform.position.x + offset, _cuttableObject.transform.position.y);

        var scalex = 1f;
        var scaley = 1f;
        if (!_wasCut)
        {
            scalex = gameObject.transform.localScale.x;
            scaley = gameObject.transform.localScale.y;
            _wasCut = true;
        }
        go.transform.localScale = new Vector3(_cuttableObject.transform.localScale.x *scalex,  _cuttableObject.transform.localScale.y * scaley );
        var go2 = new GameObject();
        var sr2 = go2.AddComponent<SpriteRenderer>();
        sr2.sprite = pieceSprite;
        sr2.sortingLayerName = ("MiniGame_Actor");
        go2.transform.position = new Vector3(_handObject.transform.position.x + offset*2, _cuttableObject.transform.position.y);
        go2.transform.DOMoveX(10, 1).OnComplete(() => Destroy(go2));
        go2.transform.localScale = new Vector3(_cuttableObject.transform.localScale.x * scalex, _cuttableObject.transform.localScale.y * scaley);
        Destroy(_cuttableObject);
        _cuttableObject = go;
    }

    public override void Destroy()
    {
        Destroy(_panelGameObject);
        Destroy(_cuttableObject);
        Destroy(gameObject);
    }
}
