using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyBehavior : EnemiesBehavior
{

    public override void SetAttractedVector(Vector2 normalized, float coefWhenAttracted)
    {
        attractedVector = normalized;
        this.coefWhenAttracted = coefWhenAttracted;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        target = Vector2.zero;
    }

    public override void MoveRandomly()
    {
        if ((target == Vector2.zero || (Vector2)transform.position == target) )
        {
            CurrentTimeToWait = 0;
            canMove = false;
            int nbr = Random.Range(0, 3);
            if (nbr == 0)
            {
                target = new Vector2(transform.position.x + caseSize * numberOfMovingCase, transform.position.y);
                currentDirection = Position.Right;
            }
            else if (nbr == 1)
            {
                target = new Vector2(transform.position.x - caseSize * numberOfMovingCase, transform.position.y);
                currentDirection = Position.Left;
            }
            else if (nbr == 2)
            {
                target = new Vector2(transform.position.x , transform.position.y + caseSize * numberOfMovingCase );
                currentDirection = Position.Up;
            }
            else if (nbr == 3)
            {
                target = new Vector2(transform.position.x, transform.position.y - caseSize * numberOfMovingCase);
                currentDirection = Position.Down;
            }
        }
        if (canMove)
        {
            if (!isAttracted) transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            else transform.position = attractedVector;
        }
    }

    public override void MoveToPlayer()
    {
        if (canMove)
        {
            ChangeNpcDirection();
            if(!isAttracted) transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            else transform.position = attractedVector;
        }
    }

    public override void GetDamage(int dmg)
    {
        if (timeRecoveryTimer <= 0)
        {
            currentLife -= dmg;
            isPushBack = true;
            timeRecoveryTimer = timeRecovery;
            GetComponent<Animator>().SetBool("isRecoveryTime", true);
            if (currentLife <= 0)
            {
                GameVariables.Instance.gameAudioSource.PlayOneShot(deadSound);
                gameObject.SetActive(false);
            }
            else
                GameVariables.Instance.gameAudioSource.PlayOneShot(hitSound);

        }
    }
    public override void MoveBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -pushbackSpeed * Time.deltaTime);
    }
    #region debug

    #endregion
}
