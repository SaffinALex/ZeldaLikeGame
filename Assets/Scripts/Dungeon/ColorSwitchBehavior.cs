using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitchBehavior : MonoBehaviour
{
    public Sprite[] sprites;
    public AudioClip sound;
    public float recoveryTime;
    private float timer;
    private bool isRed;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && timer >= recoveryTime)
        {
            timer = 0;
            GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
            if (isRed) GameVariables.Instance.switchIsRed = false;
            else GameVariables.Instance.switchIsRed = true;
        }
    }

    private void Start()
    {
        if (GameVariables.Instance.switchIsRed)
        {
            isRed = true;
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else
        {
            isRed = false;
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }

    private void FixedUpdate()
    {
        if (timer <= recoveryTime) timer += Time.fixedDeltaTime;
        if (GameVariables.Instance.switchIsRed && !isRed)
        {
            isRed = true;
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else if(!GameVariables.Instance.switchIsRed && isRed)
        {
            isRed = false;
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }
}
