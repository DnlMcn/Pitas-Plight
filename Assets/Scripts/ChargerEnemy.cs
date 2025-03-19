using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : MonoBehaviour
{
    public float chargeDistance = 5f;
    public float chargeStepDuration = 0.2f;
    public int maxStepsPerTurn = 1;
    bool isPreparingAttack;
    bool isAttacking;
    Vector3 attackDirection = Vector3.zero;

    public Transform playerTransform;
    Movement movement;
    public GameEvent endEnemyTurn;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    public void EnemyTurn()
    {
        if (!isAttacking && isPreparingAttack)
        {
            StartCoroutine(Attack());
            print("Enemy attacking");
        }
        else if (!isAttacking && !isPreparingAttack && IsPlayerInAttackRange())
        {
            StartCoroutine(PrepareAttack());
            print("Enemy preparing attack");
        }
        else if (!isAttacking && !isPreparingAttack && !IsPlayerInAttackRange())
        {
            MoveTowardsPlayer();
        }
    }

    IEnumerator PrepareAttack()
    {
        isPreparingAttack = true;
        yield return null;
        endEnemyTurn.Raise();
    }

    IEnumerator Attack()
    {
        isPreparingAttack = false;
        isAttacking = true;
        yield return StartCoroutine(movement.ChargeInDirection(attackDirection, Mathf.RoundToInt(chargeDistance), chargeStepDuration, endEnemyTurn));
        isAttacking = false;
    }

    void MoveTowardsPlayer()
    {
        StartCoroutine(movement.MoveTo(playerTransform.position, maxStepsPerTurn, endEnemyTurn));
    }

    bool IsPlayerInAttackRange()
    {
        if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hit, chargeDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                attackDirection = Vector3.right;
                Debug.DrawRay(transform.position, Vector3.right * chargeDistance, Color.green, 2);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.right * chargeDistance, Color.yellow, 2);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.right * chargeDistance, Color.red, 2);
        }

        if (Physics.Raycast(transform.position, Vector3.left, out hit, chargeDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                attackDirection = Vector3.left;
                Debug.DrawRay(transform.position, Vector3.left * chargeDistance, Color.green, 2);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.left * chargeDistance, Color.yellow, 2);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.left * chargeDistance, Color.red, 2);
        }

        if (Physics.Raycast(transform.position, Vector3.forward, out hit, chargeDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                attackDirection = Vector3.forward;
                Debug.DrawRay(transform.position, Vector3.forward * chargeDistance, Color.green, 2);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.forward * chargeDistance, Color.yellow, 2);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.forward * chargeDistance, Color.red, 2);
        }

        if (Physics.Raycast(transform.position, Vector3.back, out hit, chargeDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                attackDirection = Vector3.back;
                Debug.DrawRay(transform.position, Vector3.back * chargeDistance, Color.green, 2);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.back * chargeDistance, Color.yellow, 2);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.back * chargeDistance, Color.red, 2);
        }

        attackDirection = Vector3.zero;
        return false;
    }
}
