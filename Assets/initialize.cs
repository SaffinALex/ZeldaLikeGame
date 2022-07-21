using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initialize : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        GameVariables.Instance.gameAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
