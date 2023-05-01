using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryOutBag : MonoBehaviour
{
    SpriteRenderer _sprite;
    float timetilCarry = 1f;
    bool isCarried = false;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCarried) return;

        timetilCarry -= Time.deltaTime;
        if(timetilCarry <= 0)
        {
            AudioManager.Instance.PlaySound("bag");
            isCarried = true;
            _sprite.sortingOrder -= 1;
            gameObject.transform.DOShakePosition(2);
            gameObject.transform.DOLocalMoveX(25, 5).OnComplete(() => Destroy(gameObject));
        }
    }
}
