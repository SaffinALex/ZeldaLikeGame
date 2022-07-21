using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public int damage;
    //public List<Effect> effects;
    public Sprite icon;
    public string name;
    public bool isEnable;
    public bool needToBeInstantiate;
    public abstract void Activate(PlayerBehavior player);
    
}
