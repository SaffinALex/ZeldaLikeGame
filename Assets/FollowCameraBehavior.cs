using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 maxPosition;
    public Vector2 minPosition;
    public float smoothCoeff;
    public Transform target;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != target.position) {
            float position_x = Mathf.Clamp(target.transform.position.x, minPosition.x, maxPosition.x);
            float position_y = Mathf.Clamp(target.transform.position.y, minPosition.y, maxPosition.y);
            transform.position = Vector3.Lerp(transform.position, new Vector3(position_x, position_y, transform.position.z), smoothCoeff);
        }
    }

}
