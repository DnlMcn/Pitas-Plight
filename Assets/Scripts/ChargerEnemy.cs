using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : MonoBehaviour
{
    float chargeDistance = 4f;
    int maxStepsPerTurn = 1;
    bool isPreparingAttack;
    bool isAttacking;

    Transform playerTransform;
    Movement movement;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void EnemyTurn()
    {
        if (!isAttacking && !isPreparingAttack && IsPlayerInAttackRange())
        {
            PrepareAttack();
        }
        else if (!isAttacking && isPreparingAttack && IsPlayerInAttackRange())
        {
            Attack();
        }
        else if (!isAttacking && !isPreparingAttack && !IsPlayerInAttackRange())
        {
            MoveTowardsPlayer();
        }
    }

    void PrepareAttack()
    {
        // TODO: Draw something that lets the player see attack range and intention
    }

    void Attack()
    {
        // TODO: Move enemy according to attack, call event to deal damage
    }

    void MoveTowardsPlayer()
    {
        StartCoroutine(movement.MoveTo(playerTransform.position, maxStepsPerTurn));
    }

    bool IsPlayerInAttackRange()
    {
        return Utilities.IsInLinearRange(transform.position, playerTransform.position, chargeDistance);
    }
}
