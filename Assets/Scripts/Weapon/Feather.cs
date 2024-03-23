using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : WeaponBehavior
{
    private float timer;
    public float jumpDuration;
    public AudioClip playerJump;
    public AudioClip playerHitGround;
    private bool jumpFinish;

   public void Update()
    {
        if (player.IsJumping)
        {
            timer += Time.deltaTime;
            if (timer >= jumpDuration && !jumpFinish)
            {
                jumpFinish = true;
                GameVariables.Instance.gameAudioSource.PlayOneShot(playerHitGround);
            }
            else if (jumpFinish)
            {
                player.IsJumping = false;
                associatedModule.DecreasedCounter();
                Destroy(gameObject);
            }
        }
    }

    public override void Activate()
    {
        associatedModule.IncreaseCounter();
        jumpFinish = false;
        timer = 0;
        player.IsJumping = true;
        GameVariables.Instance.gameAudioSource.PlayOneShot(playerJump);
    }
}
