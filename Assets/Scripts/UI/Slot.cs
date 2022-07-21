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

    // Start is called before the first frame update
    void Awake()
    {
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
}
