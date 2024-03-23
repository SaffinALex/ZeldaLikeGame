using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfBodyBehavior : MonoBehaviour
{
    public FullBodyBehavior parent;
    public bool isInContact = false;

    public void IncreaseWaterCount()
    {
        if (!isInContact)
        {
            isInContact = true;
            parent.IncreaseWaterCount();
        }
    }

    public void DecreaseWaterCount()
    {
        if (isInContact)
        {
            isInContact = false;
            parent.DecreaseWaterCount();
        }
    }

    public bool CheckIfInWater()
    {
        return parent.CheckIfInWater();
    }
}
