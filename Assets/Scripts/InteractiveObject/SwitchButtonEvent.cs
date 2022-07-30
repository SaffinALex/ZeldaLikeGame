using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButtonEvent : MonoBehaviour
{
    public AudioClip sound;
    public string[] eventsName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeet") && !GameVariables.Instance.player.GetBoolAnimator("isJumping") && !GetComponent<Animator>().GetBool("isActivate"))
        {
            GetComponent<Animator>().SetBool("isActivate", true);
            GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
            foreach(string s in eventsName)
            {
                GameVariables.TriggerEventByName(s);
            }

        }
    }
}
