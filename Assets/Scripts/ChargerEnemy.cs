using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : MonoBehaviour
{
    float chargeDistance = 4f;
    int maxStepsPerTurn = 1;

    Transform playerTransform;
    Movement movement;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        MoveTowardsPlayer();
        Attack();
    }

    void Attack()
    {
        // TODO: Draw something that lets me player see attack intentions
    }

    void MoveTowardsPlayer()
    {
        if (!Utilities.IsInLinearRange(transform.position, playerTransform.position, chargeDistance))
        {
            movement.MoveTo(playerTransform.position, maxStepsPerTurn);
        }
    }
}
