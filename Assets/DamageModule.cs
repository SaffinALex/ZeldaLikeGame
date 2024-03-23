using UnityEngine;
using ModularLib;
using System.Collections.Generic;
using System.Collections;
using System;

public class DamageModule : ModuleBehavior
{
    public int initialLifePoint;
    public float recoveryTime;
    private float recoveryTimer;
    public AudioClip hitSound;
    public float knockbackSpeedFactor = 1f;
    private int currentLifePoint;
    private bool isInvincible;

    public float knockbackStrength = 1f;
    public float knockbackTime = 0.2f;
    private float knockbackCountdown = 0;
    private Vector3 knockbackDirection;

    public BinaryData IsHitByFluid { get; set; }

    public BinaryData IsHitByEnemy { get; set; }
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }

    public Asset brain;

    private int indexListSavePos;

    private struct RespawnPos
    {
        public float time;
        public Vector2 position;
    }

    private List<RespawnPos> listSavePos;

    public override void InitializeModule()
    {
        IsInvincible = false;
        IsHitByEnemy = new BinaryData(brain);
        IsHitByFluid = new BinaryData(brain);
        IsHitByEnemy.Value = false;
        IsHitByFluid.Value = false;

        currentLifePoint = initialLifePoint;

        indexListSavePos = 0;
        listSavePos = new List<RespawnPos>();
        for (int i = 0; i < 5; i++)
        {
            listSavePos.Add(new RespawnPos());
        }
    }

    public void FixedUpdateModule()
    {
        if (!IsHitByFluid.Value && !isInvincible)
        {
            RespawnPos rp = listSavePos[indexListSavePos];
            rp.time = Time.realtimeSinceStartup;
            rp.position = transform.position;
            listSavePos[indexListSavePos] = rp;
            indexListSavePos++;
            if (indexListSavePos >= 5) indexListSavePos = 0;
            if (recoveryTimer > 0) recoveryTimer -= Time.fixedDeltaTime;
        }

    }
    public override void UpdateModule()
    {
        if (recoveryTimer <= 0) IsHitByEnemy.Value = false;

        if (knockbackCountdown > 0)
        {
            knockbackCountdown -= Time.deltaTime;
            transform.position += knockbackDirection * knockbackStrength * knockbackSpeedFactor * Time.deltaTime;
        }
    }

    public void GetDamage(int dmg, Transform enemy)
    {
        if (recoveryTimer <= 0 && !IsHitByFluid.Value && !IsInvincible)
        {
            IsHitByEnemy.Value = true;
            currentLifePoint -= dmg;
            recoveryTimer = recoveryTime;
            GameVariables.Instance.gameAudioSource.PlayOneShot(hitSound);
            GameVariables.Instance.inventory.SetHeart(currentLifePoint);

            // Définir la direction du recul (vecteur normal entre l'ennemi et le joueur)
            knockbackDirection = (transform.position - enemy.position).normalized;

            // Commencer le décompte du recul
            knockbackCountdown = knockbackTime;
        }
    }

    public void SinkPlayer(int dmg, float time)
    {
        if (!IsHitByFluid.Value && !IsInvincible)
        {
            if (recoveryTimer <= 0)
            {
                currentLifePoint -= dmg;
                GameVariables.Instance.inventory.SetHeart(currentLifePoint);
            }
            recoveryTimer = recoveryTime;
            knockbackCountdown = 0;
            StartCoroutine(WhenPlayerSink(time));
        }
    }

    public IEnumerator WhenPlayerSink(float time)
    {
        IsHitByFluid.Value = true;
        RespawnPos respawnPoint = listSavePos[0];
        foreach (RespawnPos rp in listSavePos)
        {
            if (rp.time < respawnPoint.time) respawnPoint = rp;
        }
        yield return new WaitForSeconds(time);
        transform.position = respawnPoint.position;
        IsHitByFluid.Value = false;
    }

    public void SwitchInvinciblePropertie()
    {
        IsInvincible = !IsInvincible;
    }
}
