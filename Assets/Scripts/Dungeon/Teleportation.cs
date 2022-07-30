using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public HouseTPData data;
    public AudioClip sound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFeet") && GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.Playing)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.SwipeCamera);
            GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
            GameVariables.Instance.LoadLevel(data);
        }
    }
}
