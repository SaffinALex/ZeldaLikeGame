using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : BaseWeapon
{
    public float jumpDuration;
    private float timer;
    private PlayerBehavior player;
    public float couldown;
    public AudioClip playerJump;
    public AudioClip playerHitGround;
    private bool jumpFinish;
    public override void Activate(PlayerBehavior player)
    {
        this.player = player;
        player.isInvincible = true;
        player.SetBoolAnimator("isJumping", true);
        GameVariables.Instance.gameAudioSource.PlayOneShot(playerJump);
    }

   public void Update()
    {
        if (player.isInvincible || jumpFinish)
        {
            timer += Time.deltaTime;
            if (timer >= jumpDuration && !jumpFinish)
            {
                player.isInvincible = false;
                player.SetBoolAnimator("isJumping", false);
                jumpFinish = true;
                GameVariables.Instance.gameAudioSource.PlayOneShot(playerHitGround);
            }
            else if (jumpFinish)
            {
                if((couldown -= Time.deltaTime) <= 0) Destroy(gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpFinish = false;
    }

}
