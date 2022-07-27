using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : BaseWeapon
{
    #region Variables

    #region trajectory
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
    public float numeratorAfterBouncing;
    private bool isStable = false;
    #endregion

    #region projectile
    public int numberOfBounce;
    private int cptBounce;
    public float timeBeforeExplosion;
    public float explosionDuration;
    private bool hasExplode = false;
    private bool isLaunch = false;
    #endregion

    private PlayerBehavior player;

    public AudioClip hitGroundSound;
    public AudioClip explosion;
    private bool isUnGrap = false;
    private bool isNotAvailable = false;
    public ShadowBehavior shadowPos;
    private Vector2 playerPosWhenLaunch;
    public AudioClip throwObject;
    public AudioClip carryBombAudio;
    public AudioClip impossibleAudio;
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExplode && collision.CompareTag("Ennemy"))
        {
            collision.GetComponent<EnemiesBehavior>().GetDamage(damage);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hasExplode && collision.CompareTag("Ennemy"))
        {
            collision.GetComponent<EnemiesBehavior>().GetDamage(damage);
        }
        if (hasExplode && collision.CompareTag("Player"))
        {
            player.GetDamage(damage, transform);
            if(player.CarryObject != null)
            {
                if (player.CarryObject.GetComponent<CarryItemBehavior>() != null) player.CarryObject.GetComponent<CarryItemBehavior>().UnGrap();
                else if (player.CarryObject.GetComponent<Bomb>() != null) player.CarryObject.GetComponent<Bomb>().UnGrap();
            }
        }
    }
    #endregion

    #region BombLife
    internal void UnGrap()
    {
        isUnGrap = true;
    }
    private void CheckIfExplode()
    {
        if (!hasExplode)
        {
            transform.position = nextPos;
            if (playerDirection.x != 0 && playerDirection.y == 0)
            {
                shadowPos.transform.position = new Vector2(nextPos.x, playerPosWhenLaunch.y);
            }
            else if(playerDirection.x != 0 && playerDirection.y != 0)
            {
                shadowPos.transform.position = new Vector2(nextPos.x, nextPos.y - 3);
                //shadowPos.transform.position = Vector2.MoveTowards(shadowPos.transform.position, targetPos, 0.2f * Time.deltaTime);
                shadowPos.transform.position = new Vector2(nextPos.x, nextPos.y);
            }
            else if (playerDirection.x == 0 )
            {
                shadowPos.transform.position = new Vector2(nextPos.x, nextPos.y);
            }
            timeBeforeExplosion -= Time.deltaTime;
            if (timeBeforeExplosion <= 0)
            {
                hasExplode = true;
                GetComponent<Animator>().SetBool("isExplode", true);
                StartCoroutine("Explode");
            }

        }
    }
    private IEnumerator Explode()
    {
        GameVariables.Instance.gameAudioSource.PlayOneShot(explosion);
        shadowPos.gameObject.SetActive(false);
        player.SetBoolAnimator("isCarrying", false);
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }
    public void Bouncing()
    {
        cptBounce++;
        speed /= numeratorAfterBouncing;
        arcHeight /= numeratorAfterBouncing;
        distance /= numeratorAfterBouncing;
        GameVariables.Instance.gameAudioSource.PlayOneShot(hitGroundSound);
        if (!isStable)
            targetPos += (distance * playerDirection);
        startPos = transform.position;
        

    }

    #endregion

    #region Trajectory

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
            cptBounce = numberOfBounce;
        }
        else if (dist == 0)
        {
            baseY = Mathf.MoveTowards(transform.position.y, targetPos.y, speed * Time.deltaTime);
            nextPos = new Vector2(nextX, baseY);
        }
        else if (dist != 0 )
        {
            baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
            arc = (arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist));
            nextPos = new Vector2(nextX, baseY + arc);
        }

        
        // Do something when we reach the target
        if (nextPos == targetPos) Bouncing();
    }
    #endregion
    
    #region lifetime
    void FixedUpdate()
    {
        if (isLaunch && !hasExplode && shadowPos.IsTrigger())
        {
            if (!isStable)
            {
                isStable = true;
                targetPos = shadowPos.transform.position;
                CheckIfExplode();
            }
        }
        //CheckIfExplode();
        if (isLaunch && cptBounce <= numberOfBounce)
        {
            CalculateTrajectoryWhenLaunch();
        }
        if (cptBounce > numberOfBounce) shadowPos.gameObject.SetActive(false);
        else if (!isLaunch)
        {
            if (Input.GetKey(KeyCode.B) || isUnGrap)
            {
                player.CarryObject = null;
                isLaunch = true;
                startPos = transform.position;
                playerDirection = player.GetVectorMove();
                targetPos = (Vector2)this.player.transform.position + (distance * playerDirection);
                gameObject.transform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
                player.SetBoolAnimator("isCarrying", false);
                playerPosWhenLaunch = player.transform.position;
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
            else if (!hasExplode)
            {
                nextPos = new Vector2(player.transform.position.x, player.transform.position.y + 16);
                if (player.CarryObject != gameObject) player.CarryObject = gameObject;
                player.SetBoolAnimator("isCarrying", true);
            }

        }
    }

    void Update()
    {
        if (isNotAvailable)
        {
            timeBeforeExplosion -= Time.deltaTime;
            if (timeBeforeExplosion <= 0) Destroy(gameObject);
        }
        else
        {
            CheckIfExplode();
        }
    }
    #endregion

    public override void Activate(PlayerBehavior player)
    {
        if (GameVariables.Instance.inventory.GetBomb() > 0)
        {
            GameVariables.Instance.inventory.RemoveBombs(1);
            if (player.CarryObject != null)
            {
                if (player.CarryObject.GetComponent<CarryItemBehavior>() != null) player.CarryObject.GetComponent<CarryItemBehavior>().UnGrap();
                else if (player.CarryObject.GetComponent<Bomb>() != null) player.CarryObject.GetComponent<Bomb>().UnGrap();
                player.CarryObject = gameObject;
            }
            else player.CarryObject = gameObject;
            this.player = player;
            cptBounce = 0;
            hasExplode = false;
            player.SetBoolAnimator("isCarrying", true);
            GameVariables.Instance.gameAudioSource.PlayOneShot(carryBombAudio);
            nextPos = new Vector2(player.transform.position.x, player.transform.position.y + 16);
            transform.position = nextPos;
            shadowPos.transform.position = transform.position;
        }
        else if(!isNotAvailable)
        {
            isNotAvailable = true;
            timeBeforeExplosion = impossibleAudio.length;
            GetComponent<Animator>().SetBool("isNotAvailable", true);
            GameVariables.Instance.gameAudioSource.PlayOneShot(impossibleAudio);
        }
    }



#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(targetPos, new Vector3(5, 5, 5));
    }
#endif
}
