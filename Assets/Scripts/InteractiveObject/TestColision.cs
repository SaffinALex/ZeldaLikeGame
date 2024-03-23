using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestColision : MonoBehaviour
{
    public Tilemap tilemap; // Obtenez votre Tilemap ici.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Obtenez la position de la tuile dans laquelle se trouve le joueur.
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

        // Calculez les coordonnées x de la gauche et de la droite de la tuile.
        float tileWidth = 1; // Si vos tuiles sont de 16x16 pixels.
        float tileLeftX = cellPosition.x * tileWidth; // Supposons que tileWidth est la largeur de votre tuile en unités Unity.
        float tileRightX = tileLeftX + tileWidth;

        // Calculez les coordonnées x de la gauche et de la droite du sprite du joueur.
        float spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x; // Assurez-vous d'avoir un SpriteRenderer sur votre GameObject du joueur.
        float spriteLeftX = transform.position.x - spriteWidth / 2;
        float spriteRightX = spriteLeftX + spriteWidth;

        // Calculez combien de l'espace occupé par le sprite du joueur se chevauche avec la tuile d'eau.
        float overlapLeftX = Math.Max(spriteLeftX, tileLeftX);
        float overlapRightX = Math.Min(spriteRightX, tileRightX);
        float overlapWidth = overlapRightX - overlapLeftX;

        // Convertissez cette quantité d'espace en un pourcentage de la largeur totale du sprite.
        float overlapPercentage = (overlapWidth / spriteWidth) * 100; // Ce sera le pourcentage du sprite du joueur qui est dans la tuile d'eau.
        if (overlapPercentage >= 10) // Si plus de 50% du sprite du joueur est dans l'eau
        {
            Debug.Log("Je coule !");
            // Le sprite du joueur est considéré comme étant dans l'eau.
        }
        else
        {
            Debug.Log("Je Nage");
        }
    }
}
