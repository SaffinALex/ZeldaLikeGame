using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSlotBehavior : Slot
{
    public bool isItemA;
    public override void Refresh()
    {
        base.Refresh();

        if (isItemA) GameVariables.Instance.player.A = weapon.gameObject;
        else GameVariables.Instance.player.B = weapon.gameObject;
    }
    public override void Refresh(int number)
    {
        base.Refresh(number);

        if (isItemA) GameVariables.Instance.player.A = weapon.gameObject;
        else GameVariables.Instance.player.B = weapon.gameObject;
    }
}
