using ModularLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModule : ModuleBehavior
{
    [SerializeField]
    private KeyCode myKey = KeyCode.Space;

    private double delayBetweenItemUse;

    public BaseItem item;

    public BrainBehavior brain;

    private bool canUseWeapon;
    private bool isHit;

    private int instanciedItemCount;
    public override void InitializeModule()
    {
        canUseWeapon = true;
        isHit = false;
    }

    public override void UpdateModule()
    {
        UseItem();
    }

    private void UseItem()
    {
        if (Input.GetKey(myKey) && !isHit && canUseWeapon && instanciedItemCount < item.MaxAuthorizedInstance)
        {
            delayBetweenItemUse = item.GetRecoveryTime();
            item.Use(transform, brain, myKey, this);
            StartCoroutine(ItemRecoveryTime());
        }
    }

    public void WhenPlayerIsHit()
    {
        isHit = true;
    }

    public void WhenPlayerIsNotHit()
    {
        isHit = false;
    }

    private IEnumerator ItemRecoveryTime()
    {
        canUseWeapon = false;
        yield return new WaitForSeconds((float)delayBetweenItemUse);
        canUseWeapon = true;
    }

    internal void IncreaseCounter()
    {
        instanciedItemCount++;
    }

    internal void DecreasedCounter()
    {
        instanciedItemCount--;
    }
}
