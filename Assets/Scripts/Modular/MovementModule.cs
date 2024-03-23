using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularLib;
using System;

public class MovementModule : ModuleBehavior
{
    public float characterSpeed;
    private float moveX;
    private float moveY;
    private Vector2 currentDirection;
    private bool canMove;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }


    public BinaryData isMoving;
    public VectorData directionData;

    public GameObject player;
    public Asset brain;
    public override void InitializeModule()
    {
        isMoving = new BinaryData(brain);
        isMoving.Value = false;

        directionData = new VectorData(brain);
        directionData.Value = new Vector2(0, 0);

        canMove = true;
    }

    public void FixedUpdateModule()
    {
        GetInput();
        if (canMove)
        {
            MoveCharacter();
        }
    }
    public override void UpdateModule()
    {

    }

    private void MoveCharacter()
    {
        Vector3 newPosition = transform.position + ((new Vector3(moveX, moveY, 0)).normalized * characterSpeed) * Time.deltaTime;

        newPosition.x = Mathf.Round(newPosition.x);
        newPosition.y = Mathf.Round(newPosition.y);

        GetComponent<Rigidbody2D>().MovePosition(newPosition);
    }

    private void GetInput()
    {
        moveX = 0;
        moveY = 0;
        if (canMove && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveY = -1.0f;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveY = 1.0f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveX = -1.0f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveX = 1.0f;
            }
            isMoving.Value = true;
            SetDirection();
        }
        else
        {
            isMoving.Value = false;
        }


    }

    private void SetDirection()
    {
        // Check for input and update direction accordingly
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            currentDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = Vector2.right;
        }

        directionData.Value = currentDirection;
    }

    public Vector2 getCurrentDirection()
    {
        return currentDirection;
    }

    public void OnPlayerIsNotHitByEnemy()
    {
        canMove = true;
    }

    public void OnPlayerIsHitByEnemy()
    {
        canMove = false;
    }
}
