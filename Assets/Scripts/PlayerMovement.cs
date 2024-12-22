using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 startingPosition = new(2f, 1.3f, -3f);
    float height = 1.3f;

    float moveTime = 0.2f;
    private bool isMoving = false;
    private float maxMoveDistance = 2.5f;

    private void Start()
    {
        transform.position = startingPosition;
    }

    private void Update()
    {
        CheckForMoveCommand();
    }

    private void CheckForMoveCommand()
    {
        if (isMoving)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                print(hit.point);

                Vector3 clickedPosition = hit.point;
                Vector3 gridPosition = Utilities.AlignToGrid(clickedPosition, height);

                if (gridPosition != transform.position && MovementIsValid(gridPosition))
                {
                    StartCoroutine(MovePlayer(gridPosition));
                }
            }
        }
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public bool MovementIsValid(Vector3 targetPosition)
    {
        int currentX = Mathf.RoundToInt(transform.position.x);
        int currentZ = Mathf.RoundToInt(transform.position.z);
        int targetX = Mathf.RoundToInt(targetPosition.x);
        int targetZ = Mathf.RoundToInt(targetPosition.z);

        int distance = Mathf.Abs(targetX - currentX) + Mathf.Abs(targetZ - currentZ);

        return distance <= maxMoveDistance;
    }

    private IEnumerator MovePlayer(Vector3 newTargetPosition)
    {
        isMoving = true;

        Vector3 startingPosition = Utilities.AlignToGrid(transform.position);
        Vector3 targetPosition = Utilities.AlignToGrid(newTargetPosition);

        while (Mathf.Abs(targetPosition.x - startingPosition.x) > 0.01f)
        {
            Vector3 nextStep = new(
                Mathf.MoveTowards(startingPosition.x, targetPosition.x, 1),
                startingPosition.y,
                startingPosition.z
            );

            yield return StartCoroutine(StepToPosition(startingPosition, nextStep));
            startingPosition = nextStep;
        }

        while (Mathf.Abs(targetPosition.z - startingPosition.z) > 0.01f)
        {
            Vector3 nextStep = new(
                startingPosition.x,
                startingPosition.y,
                Mathf.MoveTowards(startingPosition.z, targetPosition.z, 1)
            );

            yield return StartCoroutine(StepToPosition(startingPosition, nextStep));
            startingPosition = nextStep;
        }

        isMoving = false;
    }

    private IEnumerator StepToPosition(Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}
