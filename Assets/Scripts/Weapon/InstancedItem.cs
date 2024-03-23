using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/PlayerItem")]
public class InstancedItem : BaseItem
{
    public GameObject prefab;

    public override void Use(Transform userTransform, BrainBehavior brain, KeyCode associateKey, WeaponModule weaponModule)
    {
        GameObject instantiateObject = Instantiate(prefab, userTransform.position, Quaternion.identity);
        instantiateObject.GetComponent<WeaponBehavior>().initialize(associateKey, brain, damage, weaponModule);
    }
}
