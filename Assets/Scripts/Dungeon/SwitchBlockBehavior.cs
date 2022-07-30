using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlockBehavior : MonoBehaviour
{
    public bool isRed;
    public Sprite[] sprites;

    private void FixedUpdate()
    {
        if( ((isRed && GameVariables.Instance.switchIsRed) || (!isRed && !GameVariables.Instance.switchIsRed)) /*&& GetComponent<BoxCollider2D>().enabled*/)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else if( ((!isRed && GameVariables.Instance.switchIsRed) || (isRed && !GameVariables.Instance.switchIsRed)) /*&& !GetComponent<BoxCollider2D>().enabled*/)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }

    }
}
