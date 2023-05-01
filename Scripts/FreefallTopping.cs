using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreefallTopping : MonoBehaviour
{
    public bool _isLanded;
    public string _on = "";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Topping") return;
        var other = collision.gameObject;

        if (other.tag != "Topping") return;

        var toppingObj = other.GetComponent<FreefallTopping>();
        toppingObj._isLanded = true;
        toppingObj._on = tag;
        toppingObj.tag = tag;
        AudioManager.Instance.PlaySound("drop");
        GameManager.Instance.ScreenShake(.1f, .5f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (tag == "Topping") return;
        var other = collision.gameObject;
        if (other.tag != "Topping") return;

        var toppingObj = other.GetComponent<FreefallTopping>();
        toppingObj._isLanded = true;
        toppingObj._on = tag;
        toppingObj.tag = tag;
        AudioManager.Instance.PlaySound("drop");
        GameManager.Instance.ScreenShake(.1f, .5f);
    }
}
