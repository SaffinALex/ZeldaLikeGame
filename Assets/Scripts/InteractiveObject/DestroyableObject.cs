using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField]
    private float destroySeconds;
    [SerializeField]
    private List<GameObject> listItemCanBeSpawned;
    public AudioClip sound;
    private void OnBecameVisible()
    {
        new GameVariables.TriggerEvent("ResetObject" + gameObject.GetInstanceID().ToString(), ResetObject);
    }

    public void ResetObject()
    {
        GetComponent<Animator>().SetBool("IsDestroyed", false);
        GetComponent<BoxCollider2D>().enabled = true;

    }
    public void DestroyObject()
    {
        StartCoroutine(playDestroyedAnimation());
    }

    public IEnumerator playDestroyedAnimation()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("IsDestroyed", true);
        GetComponent<AudioSource>().PlayOneShot(sound);
        yield return new WaitForSeconds(0.01f);
        Instantiate(listItemCanBeSpawned[Random.Range(0, listItemCanBeSpawned.Count)], transform);
        //Destroy(this.gameObject);
    }
}
