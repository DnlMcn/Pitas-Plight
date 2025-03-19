using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public float maxMoveDistance;
    [SerializeField]
    public float height;
    [SerializeField]
    public float stepTime = 0.2f;
    [HideInInspector]
    public bool isMoving = false;

    public IEnumerator MoveTo(Vector3 newTargetPosition, int limit, GameEvent gameEventOnEnd = null)
    {
        isMoving = true;

        Vector3 startingPosition = Utilities.AlignToGrid(transform.position, height);
        Vector3 targetPosition = Utilities.AlignToGrid(newTargetPosition, height);

        int counter = 0;

        while (Mathf.Abs(targetPosition.x - startingPosition.x) > 0.01f && limit != 0 && counter < limit)
        {
            Vector3 nextStep = new(
                Mathf.MoveTowards(startingPosition.x, targetPosition.x, 1),
                startingPosition.y,
                startingPosition.z
            );

            yield return StartCoroutine(StepToPosition(startingPosition, nextStep));
            startingPosition = nextStep;

            counter++;
        }

        while (Mathf.Abs(targetPosition.z - startingPosition.z) > 0.01f && limit != 0 && counter < limit)
        {
            Vector3 nextStep = new(
                startingPosition.x,
                startingPosition.y,
                Mathf.MoveTowards(startingPosition.z, targetPosition.z, 1)
            );

            yield return StartCoroutine(StepToPosition(startingPosition, nextStep));
            startingPosition = nextStep;

            counter++;
        }

        isMoving = false;

        if (gameEventOnEnd)
        {
            gameEventOnEnd.Raise();
        }

        if (!IsGrounded())
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator ChargeInDirection(Vector3 direction, int cells, float stepTime, GameEvent gameEventOnEnd)
    {
        isMoving = true;

        Vector3 startingPosition = Utilities.AlignToGrid(transform.position, height);
        yield return StartCoroutine(StepToPosition(startingPosition, startingPosition + (direction.normalized * cells)));

        isMoving = false;

        if (gameEventOnEnd)
        {
            gameEventOnEnd.Raise();
        }

        if (!IsGrounded())
        {
            Destroy(gameObject);
        }
    }

    public bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator StepToPosition(Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0;

        while (elapsedTime < stepTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / stepTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}