using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damage;
    public float sinkDuration;
    public AudioClip sinkAudio;
    public AudioClip plouf;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet"))
        {
            collision.GetComponent<PartOfBodyBehavior>().IncreaseWaterCount();
        }
        if (collision.CompareTag("PlayerFeet") && collision.GetComponent<PartOfBodyBehavior>().CheckIfInWater() )
        {
            GameVariables.Instance.player.damageModule.SinkPlayer(damage, sinkDuration);
        }
        if (collision.CompareTag("EnnemyFeet") && !collision.transform.parent.GetComponent<Animator>().GetBool("isInLava"))
        {
            collision.transform.parent.GetComponent<Animator>().SetBool("isInLava", true);
            collision.transform.parent.GetComponent<BasicEnemyBehavior>().Sink(sinkDuration);
            GameVariables.Instance.gameAudioSource.PlayOneShot(plouf);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet") && collision.GetComponent<PartOfBodyBehavior>().CheckIfInWater() && !GameVariables.Instance.player.damageModule.IsHitByFluid.Value)
        {
            GameVariables.Instance.player.damageModule.SinkPlayer(damage, sinkDuration);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet"))
        {
            collision.GetComponent<PartOfBodyBehavior>().DecreaseWaterCount();
        }
    }
}
