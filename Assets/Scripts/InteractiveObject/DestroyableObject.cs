using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField]
    private float destroySeconds;
    [SerializeField]
    private List<GameObject> listItemCanBeSpawned;
    private Animator animator;
    public AudioClip sound;
    public int itemRate;
    private bool isDestroyed;



    public void Awake()
    {
        animator = GetComponent<Animator>();
        isDestroyed = false;
    }

    public void OnBecameVisible()
    {
        new GameVariables.TriggerEvent("ResetObject" + gameObject.GetInstanceID().ToString(), ResetObject);
    }

    public void ResetObject()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        isDestroyed = false;
        animator.SetBool("IsDestroyed", isDestroyed);
    }
    public void DestroyObject()
    {
        StartCoroutine(playDestroyedAnimation());
        isDestroyed = true;
        animator.SetBool("IsDestroyed", isDestroyed);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public IEnumerator playDestroyedAnimation()
    {
        GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
        if (Random.Range(0, 100) < itemRate) Instantiate(listItemCanBeSpawned[Random.Range(0, listItemCanBeSpawned.Count)], transform);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(destroySeconds);
    }
}
