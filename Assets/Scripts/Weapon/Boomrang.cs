using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomrang : BaseWeapon
{
    public float range;
    public float speed;
    public float effectTime;
    private Transform baseTransform;
    private bool reversePath = false;
    private PlayerBehavior player;
    public AudioClip sound;
    public float soundLenghtOffset;
    private float timer = 0;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!reversePath && !collision.CompareTag("ColliderObject") || collision.CompareTag("Ennemy"))
        {
            if (collision.CompareTag("Ennemy"))
            {
                collision.GetComponent<EnemiesBehavior>().GetDamage(damage);
                collision.GetComponent<EnemiesBehavior>().SetIsActive(effectTime);
            }
            reversePath = true;
        }
        if (collision.gameObject.GetComponent<DestroyableObject>() != null)
        {
            collision.gameObject.GetComponent<DestroyableObject>().DestroyObject();
        }
    }

    public void FixedUpdate()
    {
        if (GetComponent<Animator>().GetBool("isActive"))
        {
            timer += Time.deltaTime;
            if(timer >= sound.length + soundLenghtOffset)
            {
                timer = 0;
                GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
            }

            if (!reversePath)
            {
                Vector2 t = (Vector2)baseTransform.position + player.GetVectorMove() * range;
                if ((Vector2)transform.position != t)
                {
                    transform.position = Vector2.MoveTowards(transform.position, t, speed * Time.deltaTime);
                }
                else
                {
                    reversePath = true;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, baseTransform.position, speed * Time.deltaTime);
            }
            if(reversePath && transform.position == baseTransform.position)
            {
                reversePath = false;
                player.CanMove = true;
                player.GetComponent<Animator>().SetBool("UseWeapon", false);
                GetComponent<Animator>().SetBool("isActive", false);
                Destroy(this.gameObject);
            }
        }
    }
    private void Attack()
    {
        player.CanMove = false;
      //  GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
        player.GetComponent<Animator>().SetBool("UseWeapon", true);
        baseTransform = player.transform;
        GetComponent<Animator>().SetBool("isActive", true);
    }
    public override void Activate(PlayerBehavior player)
    {
        this.player = player;
        Attack();
    }
}
