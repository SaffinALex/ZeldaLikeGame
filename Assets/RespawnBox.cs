using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet"))
        {
            GameVariables.Instance.player.positionToRespawn = transform.position;
        }
    }
}
