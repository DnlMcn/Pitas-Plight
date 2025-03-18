using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : MonoBehaviour
{
    float chargeDistance = 4f;
    int maxStepsPerTurn = 1;
    bool isPreparingAttack;
    bool isAttacking;

    public Transform playerTransform;
    Movement movement;
    public GameEvent endEnemyTurn;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    public void EnemyTurn()
    {
        StartCoroutine(Utilities.Wait(0.5f));

        if (!isAttacking && !isPreparingAttack && IsPlayerInAttackRange())
        {
            PrepareAttack();
            print("Enemy preparing attack");
        }
        else if (!isAttacking && isPreparingAttack && IsPlayerInAttackRange())
        {
            Attack();
            print("Enemy attacking");
        }
        else if (!isAttacking && !isPreparingAttack && !IsPlayerInAttackRange())
        {
            MoveTowardsPlayer();
            print("Enemy moving towards player");
        }

        endEnemyTurn.Raise();
    }

    void PrepareAttack()
    {
        isPreparingAttack = true;
        // TODO: Draw something that lets the player see attack range and intention
    }

    void Attack()
    {
        isPreparingAttack = false;
        isAttacking = true;
        // TODO: Move enemy according to attack, call event to deal damage
        isAttacking = false;
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
