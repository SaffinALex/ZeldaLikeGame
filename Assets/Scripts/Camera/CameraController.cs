using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player; // Le Transform du joueur
    public float screenEdgeBuffer = 5f; // Une marge pour commencer le mouvement avant que le joueur n'atteigne réellement le bord de l'écran
    public float screenEdgeBufferLeft;
    public float cameraSpeed;
    public float playerSpeed;
    public int cameraPixelLeft;
    public int cameraPixelUp;
    public int pushUpPixelPlayer;
    public int pushLeftPixelPlayer;
    private Vector3 cameraTargetPosition;
    private Vector3 initialPosition;
    private Vector3 playerTargetPosition;

    void Start()
    {
        // Définit la position cible initiale de la caméra sur la position actuelle
        cameraTargetPosition = transform.position;
        initialPosition = transform.position;
    }

    void Update() 
    {
        if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.SwipeCamera && transform.position == cameraTargetPosition)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
            initialPosition = transform.position;
        }
        if (!GameVariables.instance.player.IsJumping)
        {
            // Vérifie si le joueur est près du bord de l'écran en x
            if (Mathf.Abs(player.position.x - transform.position.x) > Camera.main.orthographicSize - screenEdgeBufferLeft)
            {
                // Calcule la nouvelle position cible de la caméra en x
                float direction = Mathf.Sign(player.position.x - transform.position.x);
                cameraTargetPosition = initialPosition + new Vector3(direction * cameraPixelLeft, 0, 0);
                playerTargetPosition = player.position + new Vector3(direction * pushLeftPixelPlayer, 0, 0);
                GameStateManager.Instance.SetState(GameStateManager.GameState.SwipeCamera);
            }

            // Vérifie si le joueur est près du bord de l'écran en y
            if (Mathf.Abs(player.position.y - transform.position.y) > Camera.main.orthographicSize - screenEdgeBuffer)
            {
                // Calcule la nouvelle position cible de la caméra en y
                float direction = Mathf.Sign(player.position.y - transform.position.y);
                cameraTargetPosition = initialPosition + new Vector3(0, direction * cameraPixelUp, 0);
                playerTargetPosition = player.position + new Vector3(0, direction * pushUpPixelPlayer, 0);
                GameStateManager.Instance.SetState(GameStateManager.GameState.SwipeCamera);
            }

            // Déplace la caméra vers la position cible
            transform.position = Vector3.MoveTowards(transform.position, cameraTargetPosition, cameraSpeed * Time.deltaTime);
            if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.SwipeCamera)
            {
                player.position = Vector3.MoveTowards(player.position, playerTargetPosition, playerSpeed * Time.deltaTime);
            }

        }

    }
}
