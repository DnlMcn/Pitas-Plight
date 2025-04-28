using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotFollow : MonoBehaviour
{
    public Transform playerTransform;

    EventBinding<UpdateActivePlayer> currentPlayerEvent;

    void OnEnable()
    {
        currentPlayerEvent = new EventBinding<UpdateActivePlayer>(NewFollowTarget);
        EventBus<UpdateActivePlayer>.Register(currentPlayerEvent);
    }

    void OnDisable()
    {
        EventBus<UpdateActivePlayer>.Deregister(currentPlayerEvent);
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
