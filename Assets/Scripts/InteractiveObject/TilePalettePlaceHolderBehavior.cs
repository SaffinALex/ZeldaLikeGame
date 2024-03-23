using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePalettePlaceHolderBehavior: MonoBehaviour
{ 
    public GameObject objectToInstantiate;
    private Tilemap tilemap;

    public void Start()
    {
        tilemap = GameObject.Find("InteractiveObject").GetComponent<Tilemap>();
        Vector3Int tilePosition = tilemap.WorldToCell(Instantiate(objectToInstantiate, this.GetComponent<Transform>()).transform.position);
        tilemap.SetTile(tilePosition, null);
        Destroy(gameObject);
    }
}
