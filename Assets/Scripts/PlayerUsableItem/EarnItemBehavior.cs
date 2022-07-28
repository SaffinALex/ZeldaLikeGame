using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnItemBehavior : MonoBehaviour
{
    public int quantity;
    public bool isRupee;
    public bool isBomb;
    public bool isKey;
    public bool isArrow;
    public AudioClip playSoundWhenGet;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("PlayerFeet"))
        {
            if (isRupee) GameVariables.Instance.inventory.AddRupees(quantity);
            if (isKey) GameVariables.Instance.inventory.AddKeys(quantity);
            if (isBomb) GameVariables.Instance.inventory.AddBombs(quantity);
            if (isArrow) GameVariables.Instance.inventory.AddArrows(quantity);
            GameVariables.Instance.gameAudioSource.PlayOneShot(playSoundWhenGet);
            Destroy(gameObject);
        }
    }

    internal void GiveItem()
    {
        if (isRupee) GameVariables.Instance.inventory.AddRupees(quantity);
        if (isKey) GameVariables.Instance.inventory.AddKeys(quantity);
        if (isBomb) GameVariables.Instance.inventory.AddBombs(quantity);
        if (isArrow) GameVariables.Instance.inventory.AddArrows(quantity);
    }

}
