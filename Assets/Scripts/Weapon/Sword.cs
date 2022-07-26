﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BaseWeapon
{

    [SerializeField]
    private float attackTime;

    private bool canAttack = true;
    private float moveX, moveY;
    private PlayerBehavior player;
    public GameObject sprite;
    public AudioClip sound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DestroyableObject>() != null)
        {
            collision.gameObject.GetComponent<DestroyableObject>().DestroyObject();
        }
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            collision.gameObject.GetComponent<EnemiesBehavior>().GetDamage(damage);
        }
    }

    #region attack

    public IEnumerator attackTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackTime);
        player.gameObject.GetComponent<Animator>().SetBool("isAttack", false);
        player.CanMove = true;
        GetComponent<Animator>().SetBool("isAttack", false);
        yield return new WaitForSeconds(0.1f);
        canAttack = true;
        Destroy(gameObject);
    }
    private void Attack()
    {
        if (canAttack)
        {
            GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
            sprite.SetActive(true);
            player.ReplaceDirection();
            GetComponent<Animator>().SetBool("isAttack", true);
            player.gameObject.GetComponent<Animator>().SetBool("isAttack", true);
            player.CanMove = false;
            StartCoroutine(attackTimer());
        }
    }

    #endregion
    public override void Activate(PlayerBehavior player)
    {
        this.player = player;
        player.ReplaceDirection();
        GetComponent<Animator>().SetFloat("moveX", player.GetComponent<Animator>().GetFloat("MoveX"));
        GetComponent<Animator>().SetFloat("moveY", player.GetComponent<Animator>().GetFloat("MoveY"));
        Attack();
    }
}
