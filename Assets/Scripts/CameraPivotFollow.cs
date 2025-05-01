using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Events;

public class CameraPivotFollow : MonoBehaviour
{
    public Transform playerTransform;


    void OnEnable()
    {
        EventBus<UpdateActivePlayer>.OnEvent += NewFollowTarget;
    }

    void OnDisable()
    {
        EventBus<UpdateActivePlayer>.OnEvent -= NewFollowTarget;
    }

    void Update()
    {
        if (playerTransform)
        {
            transform.position = playerTransform.position;
        }
    }

    void NewFollowTarget(UpdateActivePlayer evt)
    {
        playerTransform = evt.gameObject.transform;
    }
}
