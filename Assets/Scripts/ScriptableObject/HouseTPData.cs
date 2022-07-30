using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HouseTPData", menuName = "TP/HouseData") ]
public class HouseTPData : ScriptableObject
{
    public Vector2 maxPosition;
    public Vector2 minPosition;
    public AudioClip ambientMusic;
    public Vector2 playerSpawnPosition;
    public string sceneName;
    public bool boxColliderIsActive;
}
