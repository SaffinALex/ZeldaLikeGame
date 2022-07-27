using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager: MonoBehaviour
{
    public GameObject UI;
    public GameObject inventory;
    public GameObject dialogueBox;


    public void LoadUI()
    {
        GameObject ui = Instantiate(UI);
        InventoryBehavior inventory = Instantiate(this.inventory, ui.transform).GetComponent<InventoryBehavior>();
        GameObject dialogueBox = Instantiate(this.dialogueBox, ui.transform);
        GameVariables.Instance.dialogueBox = dialogueBox;
        GameVariables.Instance.inventory = inventory;

    }

    public void LoadInventory()
    {

    }
}
