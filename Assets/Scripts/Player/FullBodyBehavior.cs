using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBodyBehavior : MonoBehaviour
{
    [Range(0f, 1f)]
    public double percentage;
    public int totalColliderCount;
    private int waterColliderCount = 0;

    public void IncreaseWaterCount()
    {
        waterColliderCount++;
        CheckIfInWater();
    }

    public void DecreaseWaterCount()
    {
        waterColliderCount--;
        CheckIfInWater();
    }

    public bool CheckIfInWater()
    {
        return waterColliderCount >= totalColliderCount * percentage;
    }
}
