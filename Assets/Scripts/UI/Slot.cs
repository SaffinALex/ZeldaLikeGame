using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public BaseWeapon weapon;
    public Text text;
    public Image icons;
    public bool hasNumber;
    public Text number;

    // Start is called before the first frame update
    private void Update()
    {
        if (!hasNumber)
        {
            number.gameObject.SetActive(false);
        }
        else
        {
            number.gameObject.SetActive(true);
        }
    }


    public virtual void Refresh()
    {

        if (weapon.isEnable)
        {
            text.text = weapon.name;
            icons.sprite = weapon.icon;
        }
        else
        {
            text.text = "";
            icons.sprite = null;
        }
    }
    public virtual void Refresh(int number)
    {

        if (weapon.isEnable)
        {
            text.text = weapon.name;
            icons.sprite = weapon.icon;
        }
        else
        {
            text.text = "";
            icons.sprite = null;
        }
        if (hasNumber)
        {
            this.number.text = "x" + number;
        }
    }
}
