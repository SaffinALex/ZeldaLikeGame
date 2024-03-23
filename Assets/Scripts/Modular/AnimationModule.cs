using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationModule : ModuleBehavior
{
    [SerializeField]
    private Animator playerAnimator;

    public override void InitializeModule()
    {
        
    }

    public override void UpdateModule()
    {
        
    }

    public void OnPlayerStartMoving()
    {
        playerAnimator.SetBool("IsMoving", true);
    }

    public void OnPlayerStopMoving()
    {
        playerAnimator.SetBool("IsMoving", false);
    }

    public void OnPlayerGoRight()
    {
        playerAnimator.SetFloat("MoveY", 0.0f);
        playerAnimator.SetFloat("MoveX", 1.0f);
    }

    public void OnPlayerGoLeft()
    {
        playerAnimator.SetFloat("MoveY", 0.0f);
        playerAnimator.SetFloat("MoveX", -1.0f);
    }

    public void OnPlayerGoUp()
    {
        playerAnimator.SetFloat("MoveX", 0.0f);
        playerAnimator.SetFloat("MoveY", 1.0f);
    }

    public void OnPlayerGoDown()
    {
        playerAnimator.SetFloat("MoveX", 0.0f);
        playerAnimator.SetFloat("MoveY", -1.0f);
    }

    public void OnPushStop()
    {
        playerAnimator.SetBool("IsPushing", false);
    }

    public void OnPushStart()
    {
        playerAnimator.SetBool("IsPushing", true);
    }

    internal void OnPlayerIsHitByFluid()
    {
        playerAnimator.SetBool("isInLava", true);
    }

    internal void OnPlayerIsNotHitByFluid()
    {
        playerAnimator.SetBool("isInLava", false);
    }

    internal void WhenPlayerCloseChest()
    {
        playerAnimator.SetBool("openChest", false);
    }

    internal void WhenPlayerOpenChest()
    {
        playerAnimator.SetBool("openChest", true);
    }

    internal void SetBoolAnimator(string v1, bool v2)
    {
        playerAnimator.SetBool(v1, v2);
    }

    public bool GetBoolAnimator(string v)
    {
        return playerAnimator.GetBool(v);
    }

    public void SwitchJumpAnimation()
    {
        SetBoolAnimator("isJumping", !GetBoolAnimator("isJumping"));
    }
}
