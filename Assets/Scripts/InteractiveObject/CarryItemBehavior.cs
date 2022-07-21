using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryItemBehavior : MonoBehaviour
{
    [Tooltip("Position we want to hit")]
    private Vector2 targetPos;
    public float distance;
    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 10;

    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 1;

    Vector2 startPos;
    private Vector2 playerDirection;
    Vector2 nextPos;
    private PlayerBehavior player;
    private bool isLaunch = false;
    public float explosionDuration;
    public AudioClip hitGroundSound;
    public AudioClip explosion;
    private bool isCarrying = false;
    private bool isStable = false;
    private Vector2 playerPosition;
    public List<string> collisionTag;
    public int damage;
    private bool canCarry;
    private bool isUnGrap = false;
    public List<BoxCollider2D> boxColliders;
    public float timeToGrap;
    private Vector2 playerOrientation;
    private bool isGrabbing;
    public ShadowBehavior shadowPos;
    private Vector2 playerPosWhenLaunch;
    public float grabSpeed;
    public string weaponTagName;
    public AudioClip throwObject;
    public AudioClip carryItemBegin;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isLaunch && !GetBoolAnimator("isExploded"))
        {
            if (collision.CompareTag("Ennemy") && !GetBoolAnimator("isExploded"))
            {
                SetBoolAnimator("isExploded", true);
                collision.GetComponent<EnemiesBehavior>().GetDamage(damage);
                HitGround();
            }

        }


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isLaunch && !GetBoolAnimator("isExploded"))
        {
            if (collision.CompareTag("Ennemy") && !GetBoolAnimator("isExploded"))
            {
                SetBoolAnimator("isExploded", true);
                collision.GetComponent<EnemiesBehavior>().GetDamage(damage);
                HitGround();
            }
        }
        if (!isCarrying && collision.CompareTag("Player"))
        {
            CheckIfCanCarry(collision.GetComponent<PlayerBehavior>());
        }
    }

    private void CheckIfCanCarry(PlayerBehavior o)
    {
        Vector2 dir = ((Vector2)transform.position - (Vector2)o.transform.position).normalized;
        if (dir.y >= 0.8f && o.LookUp())
        {
            canCarry = true;
            playerOrientation = new Vector2(0, 1);
        }
        else if (dir.y <= -0.8f && o.LookDown())
        {
            canCarry = true;
            playerOrientation = new Vector2(0, -1);
        }
        else if (dir.x <= -0.8f && o.LookLeft())
        {
            canCarry = true;
            playerOrientation = new Vector2(-1, 0);
        }
        else if (dir.x >= 0.8f && o.LookRight())
        {
            canCarry = true;
            playerOrientation = new Vector2(1, 0);
        }
        else
        {
            canCarry = false;
        }
        if (player == null) player = o;
    }
    private void CalculateTrajectoryWhenLaunch()
    {
        // Compute the next position, with arc added in
        float baseY;
        float arc;
        float x0 = startPos.x;
        float x1 = targetPos.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        if (isStable)
        {
            nextX = targetPos.x;
            baseY = Mathf.MoveTowards(transform.position.y, targetPos.y, speed * Time.deltaTime);
            nextPos = new Vector2(nextX, baseY);
        }
        else if (dist == 0)
        {
            baseY = Mathf.MoveTowards(transform.position.y, targetPos.y, speed * Time.deltaTime);
            nextPos = new Vector2(nextX, baseY);
        }
        else if (dist != 0)
        {
            baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
            arc = (arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist));
            nextPos = new Vector2(nextX, baseY + arc);
        }


        // Do something when we reach the target
        if (nextPos == targetPos && !GetBoolAnimator("isExploded")) HitGround();
        else
        {
            SetPosition();
        }
    }
    private void SetPosition()
    {
        transform.position = nextPos;
        if (playerDirection.x != 0 && playerDirection.y == 0)
        {
            shadowPos.transform.position = new Vector2(nextPos.x, playerPosWhenLaunch.y);
        }
        else if (playerDirection.x != 0 && playerDirection.y != 0)
        {
            shadowPos.transform.position = new Vector2(nextPos.x, nextPos.y - 3);
            //shadowPos.transform.position = Vector2.MoveTowards(shadowPos.transform.position, targetPos, 0.2f * Time.deltaTime);
            shadowPos.transform.position = new Vector2(nextPos.x, nextPos.y);
        }
        else if (playerDirection.x == 0)
        {
            shadowPos.transform.position = new Vector2(nextPos.x, nextPos.y);
        }
        if (shadowPos.IsTrigger())
        {
            HitGround();
        }
    }
    private void Awake()
    {
        shadowPos.gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        if (isGrabbing)
        {
            transform.position = Vector2.MoveTowards((Vector2)transform.position, nextPos, grabSpeed * Time.deltaTime);
        }
        //CheckIfExplode();
        if (isLaunch && !GetBoolAnimator("isExploded"))
        {
            CalculateTrajectoryWhenLaunch();

        }
        else if (!isLaunch && player!=null)
        {
            if ( ((Input.GetKey(KeyCode.A) && player.GetWeaponATag() == weaponTagName) || (Input.GetKey(KeyCode.B) && player.GetWeaponBTag() == weaponTagName))  && !isCarrying && canCarry && !isGrabbing && !player.GetBoolAnimator("isGrabbing"))
            {
                Activate();
                GameVariables.Instance.gameAudioSource.PlayOneShot(carryItemBegin);
            }
            else if ((((Input.GetKey(KeyCode.A) && player.GetWeaponATag() == weaponTagName) || (Input.GetKey(KeyCode.B) && player.GetWeaponBTag() == weaponTagName)) || isUnGrap ||
                (player.GetWeaponBTag() != weaponTagName && player.GetWeaponATag() != weaponTagName) ) && isCarrying)
            {
                player.CarryObject = null;
                playerPosWhenLaunch = player.transform.position;
                shadowPos.gameObject.SetActive(true);
                isLaunch = true;
                startPos = transform.position;
                playerDirection = player.GetVectorMove();
                targetPos = (Vector2)this.player.transform.position + (distance * playerDirection);
                gameObject.transform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
                player.SetBoolAnimator("isCarrying", false);
                isCarrying = false;
                if (!player.GetBoolAnimator("IsMoving") || isUnGrap)
                {
                    isStable = true;
                    targetPos = player.transform.position;
                }
                else
                {
                    GameVariables.Instance.gameAudioSource.PlayOneShot(throwObject);
                }
            }
            if (isCarrying)
            {
                nextPos = new Vector2(player.transform.position.x, player.transform.position.y + 16);
                player.SetBoolAnimator("isCarrying", true);
                transform.position = nextPos;
                if (player.CarryObject != gameObject) player.CarryObject = gameObject;
            }

        }
    }

    public void HitGround()
    {
        GameVariables.Instance.gameAudioSource.PlayOneShot(hitGroundSound);
        shadowPos.gameObject.SetActive(false);
        SetBoolAnimator("isExploded", true);
        StartCoroutine("Explode");

    }
    // Update is called once per frame

    public void Activate()
    {
        canCarry = false;
        playerPosition = player.transform.position;
        isGrabbing = true;
        StartCoroutine("PlayGrabAnimation");
        nextPos = new Vector2(player.transform.position.x, player.transform.position.y + 16);
        //transform.position = nextPos;
        foreach (BoxCollider2D b in boxColliders) b.enabled = false;
        shadowPos.transform.position = transform.position;
    }

    private IEnumerator PlayGrabAnimation()
    {
        player.SetBoolAnimator("isGrabbing", true);
        player.ChangePlayerOrientation(playerOrientation);
        if (player.CarryObject != null)
        {
            if (player.CarryObject.GetComponent<CarryItemBehavior>() != null) player.CarryObject.GetComponent<CarryItemBehavior>().UnGrap();
            else if (player.CarryObject.GetComponent<Bomb>() != null) player.CarryObject.GetComponent<Bomb>().UnGrap();
            player.CarryObject = gameObject;
        }
        yield return new WaitForSeconds(timeToGrap);
        player.CarryObject = gameObject;
        player.SetBoolAnimator("isCarrying", true);
        player.SetBoolAnimator("isGrabbing", false);
        isGrabbing = false;
        isCarrying = true;
    }
    public void UnGrap()
    {
        isUnGrap = true;
    }

    private IEnumerator Explode()
    {
        GameVariables.Instance.gameAudioSource.PlayOneShot(explosion);
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }

    private void SetBoolAnimator(string name, bool value)
    {
        GetComponent<Animator>().SetBool(name, value);
    }
    private bool GetBoolAnimator(string name)
    {
        return GetComponent<Animator>().GetBool(name);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(targetPos, new Vector3(5, 5, 5));
    }
#endif
}
