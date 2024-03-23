using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombBehavior : WeaponBehavior
{
    #region Variables

    public CarryItemBehavior bombCarryBehavior;

    #region projectile

    public float timeBeforeExplosion;
    public float explosionDuration;
    #endregion

    public AudioClip explosion;

    private bool isExploded = false;
    
    #endregion

    #region Trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isExploded)
        {
            if(collision.CompareTag("Ennemy")) collision.GetComponent<EnemiesBehavior>().GetDamage(damage);
            else if (collision.CompareTag("Player"))
            {
                player.GetDamage(damage, transform);
            }
        }
        
    }
 
    #endregion

    #region BombLife


    private void CheckIfExplode()
    {
        timeBeforeExplosion -= Time.deltaTime;
        if (timeBeforeExplosion <= 0)
        {
            isExploded = true;
            Destroy(bombCarryBehavior);
            GetComponent<Animator>().SetBool("isExplode", true);
            StartCoroutine("Explode");
        }
    }
    private IEnumerator Explode()
    {
        GameVariables.Instance.gameAudioSource.PlayOneShot(explosion);
        player.SetBoolAnimator("isCarrying", false);
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }



    #endregion



    #region lifetime


    void Update()
    {
        CheckIfExplode();
        if (Input.GetKey(associateKey) && bombCarryBehavior.getState() == CarryItemBehavior.CarryObjectState.carryByPlayer)
        {
            bombCarryBehavior.ThrowItem();
        }
    }
    #endregion

    public override void Activate()
    {
        if (GameVariables.Instance.inventory.GetBomb() > -1)
        {
            GameVariables.Instance.inventory.RemoveBombs(1);
            bombCarryBehavior.Initialize(player);
        }
        else
        {
            Destroy(gameObject);
        }

    }



}
