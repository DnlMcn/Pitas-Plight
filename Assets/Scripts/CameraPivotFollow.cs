using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotFollow : MonoBehaviour
{
    public Transform playerTransform;

    void Update()
    {
        if (playerTransform)
        {
            transform.position = playerTransform.position;
        }
    }
}
