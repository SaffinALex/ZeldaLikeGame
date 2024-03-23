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
        if (player.isFlying || jumpFinish)
        {
            timer += Time.deltaTime;
            if (timer >= jumpDuration && !jumpFinish)
            {
                player.isFlying = false;
                player.SetBoolAnimator("isJumping", false);
                jumpFinish = true;
                GameVariables.Instance.gameAudioSource.PlayOneShot(playerHitGround);
            }
            else if (jumpFinish)
            {
                Destroy(gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpFinish = false;
    }

    public override void Activate()
    {
        timer = 0;
        player.isFlying = true;
        player.SetBoolAnimator("isJumping", true);
        GameVariables.Instance.gameAudioSource.PlayOneShot(playerJump);
    }
}
