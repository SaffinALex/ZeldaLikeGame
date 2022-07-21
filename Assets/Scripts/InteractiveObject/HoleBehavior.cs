using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehavior : MonoBehaviour
{
    private Vector2 oldPosition;
    private bool isAttracted;
    private List<GameObject> attractedObjects;
    public int damage;
    public AudioClip fallPlayerSound;
    public AudioClip fallObjectSound;
    public AudioClip fallEnemySound;
    public float fallDuration;
    private float timer;
    public float fallSpeed;
    public float coefMalusAttracted1;
    public float coefMalusAttracted2;
    public float holeDistanceBeforeDead;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet") && !GameVariables.Instance.player.isAttracted)
        {
            isAttracted = true;
            attractedObjects.Add(collision.gameObject);
            isAttracted = true;
        }
        if (collision.CompareTag("EnnemyFeet") && !collision.transform.parent.GetComponent<EnemiesBehavior>().isAttracted)
        {
            attractedObjects.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet"))
        {
            attractedObjects.Remove(collision.gameObject);
            GameVariables.Instance.player.isAttracted = false;
            isAttracted = false;
        }
        if (collision.CompareTag("EnnemyFeet"))
        {
            attractedObjects.Remove(collision.gameObject);
            collision.transform.parent.GetComponent<EnemiesBehavior>().isAttracted = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        attractedObjects = new List<GameObject>();
        oldPosition = GameVariables.Instance.player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject g in attractedObjects)
        {
            float d = Vector2.Distance(g.transform.position, transform.position);
            if (g.tag == "PlayerFeet" && !GameVariables.Instance.player.GetBoolAnimator("isFalling") && !GameVariables.Instance.player.isInvincible)
            {
                GameVariables.Instance.player.SetAttractedVector(Vector2.MoveTowards(g.transform.position, transform.position, fallSpeed * Time.deltaTime), Mathf.Lerp(coefMalusAttracted2, coefMalusAttracted1, d));
                GameVariables.Instance.player.isAttracted = true;
            }
            else if (g.tag == "EnnemyFeet" && !g.transform.parent.GetComponent<EnemiesBehavior>().GetComponent<Animator>().GetBool("isFalling"))
            {
                g.transform.parent.GetComponent<EnemiesBehavior>().SetAttractedVector(Vector2.MoveTowards(g.transform.position, transform.position, fallSpeed * 2 * Time.deltaTime), Mathf.Lerp(coefMalusAttracted2, coefMalusAttracted1, d));
                g.transform.parent.GetComponent<EnemiesBehavior>().isAttracted = true;

            }
        }
        
    }

    private void LateUpdate()
    {
        if (!isAttracted) oldPosition = GameVariables.Instance.player.transform.position;
    }

    public void WhenPlayerFall()
    {
        if(!GameVariables.Instance.player.isInvincible && !GameVariables.Instance.player.GetBoolAnimator("isFalling")) StartCoroutine(FallingPlayer());
    }
    public void WhenEnnemyFall(EnemiesBehavior e)
    {
        StartCoroutine(FallingEnnemy(e));
    }
    public IEnumerator FallingPlayer()
    {
        GameVariables.Instance.player.SetBoolAnimator("isFalling", true);
        yield return new WaitForSeconds(fallDuration);
        GameVariables.Instance.gameAudioSource.PlayOneShot(fallPlayerSound);
        GameVariables.Instance.player.GiveDamageAndRespawn(damage, oldPosition);
        GameVariables.Instance.player.SetBoolAnimator("isFalling", false);
    }
    public IEnumerator FallingEnnemy(EnemiesBehavior ennemy)
    {
        ennemy.GetComponent<Animator>().SetBool("isFalling", true);
        yield return new WaitForSeconds(fallDuration);
        GameVariables.Instance.gameAudioSource.PlayOneShot(fallEnemySound);
        ennemy.gameObject.SetActive(false);
        ennemy.GetComponent<Animator>().SetBool("isFalling", false);
    }

}
