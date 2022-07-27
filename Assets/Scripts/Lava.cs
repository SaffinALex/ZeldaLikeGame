using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damage;
    public float sinkDuration;
    public AudioClip sinkAudio;
    public AudioClip plouf;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeet") && !GameVariables.Instance.player.GetBoolAnimator("isInLava") && !GameVariables.Instance.player.GetBoolAnimator("isJumping"))
        {
            GameVariables.Instance.player.SetBoolAnimator("isInLava", true);
            GameVariables.Instance.player.sinkPlayer(damage, sinkDuration);
            GameVariables.Instance.gameAudioSource.PlayOneShot(sinkAudio);
        }
        if (collision.CompareTag("EnnemyFeet") && !collision.transform.parent.GetComponent<Animator>().GetBool("isInLava"))
        {
            collision.transform.parent.GetComponent<Animator>().SetBool("isInLava", true);
            collision.transform.parent.GetComponent<BasicEnemyBehavior>().Sink(sinkDuration);
            GameVariables.Instance.gameAudioSource.PlayOneShot(plouf);
        }
    }
}
