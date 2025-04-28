using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargerEnemy : MonoBehaviour
{
    public float chargeDistance = 5f;
    public float chargeStepDuration = 0.2f;
    public int maxStepsPerTurn = 1;
    bool isPreparingAttack;
    bool isAttacking;
    Vector3 attackDirection = Vector3.zero;
    TurnTaker turnTaker;

    public Material enemyMaterial;
    Color enemyMaterialOriginalColor;
    public GameObject attackPreview;
    GameObject currentAttackPreview;

    public Transform playerTransform;
    Movement movement;

    public CellsManager cellsManager;

    void Start()
    {
        movement = GetComponent<Movement>();
        turnTaker = GetComponent<TurnTaker>();
        enemyMaterialOriginalColor = enemyMaterial.color;
        cellsManager.AddToTransforms(transform);
    }

    public void EnemyTurn()
    {
        UpdateTargetPlayer();

        if (!isAttacking && isPreparingAttack)
        {
            Destroy(currentAttackPreview);
            StartCoroutine(Attack());
            print("Enemy attacking");
        }
        else if (!isAttacking && !isPreparingAttack && IsPlayerInAttackRange())
        {
            StartCoroutine(PrepareAttack());
            StartCoroutine(ShowEnemyIntentions());
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
        EventBus<EndTurn>.Raise(new());
    }

    IEnumerator Attack()
    {
        isPreparingAttack = false;
        isAttacking = true;
        yield return StartCoroutine(movement.ChargeInDirection(attackDirection, Mathf.RoundToInt(chargeDistance), chargeStepDuration));
        isAttacking = false;
    }

    void UpdateTargetPlayer()
    {
        playerTransform = cellsManager.GetClosestPlayer(transform.position);
    }

    IEnumerator ShowEnemyIntentions()
    {
        Renderer rend = GetComponent<Renderer>();
        // Create a temporary instance of the material so only this enemy is affected.
        Material tempMaterial = new(rend.sharedMaterial);
        rend.material = tempMaterial;

        float timer = 0f;
        while (isPreparingAttack)
        {
            // Flash red
            timer += Time.deltaTime;
            tempMaterial.color = Color.Lerp(enemyMaterialOriginalColor, Color.red, Mathf.PingPong(timer * 2f, 1));
            yield return null;

            // Show attack preview
            float rotation = 0f;
            if (attackDirection.Equals(Vector3.right) || attackDirection.Equals(Vector3.left))
            {
                rotation = 90f;
            }

            if (!currentAttackPreview)
            {
                currentAttackPreview = Instantiate(attackPreview, transform.position + (attackDirection.normalized * 2), Quaternion.Euler(90, rotation, 0));
            }
        }

        tempMaterial.color = enemyMaterialOriginalColor;
    }


    void MoveTowardsPlayer()
    {
        StartCoroutine(movement.MoveTo(playerTransform.position, maxStepsPerTurn));
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

    void OnCollisionEnter(Collision collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    void OnDestroy()
    {
        enemyMaterial.color = enemyMaterialOriginalColor;
        cellsManager.RemoveFromTransforms(transform);
        if (currentAttackPreview)
        {
            Destroy(currentAttackPreview);
        }
    }
}
