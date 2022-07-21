using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugControlPoint : MonoBehaviour
{
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(16, 16, 0));
    }
#endif
}
