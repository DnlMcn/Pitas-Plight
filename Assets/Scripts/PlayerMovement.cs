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
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                print(hit.point);

                Vector3 clickedPosition = hit.point;
                Vector3 gridPosition = new(
                    Mathf.Round(clickedPosition.x),
                    height,
                    Mathf.Round(clickedPosition.z)
                );

                // Start moving the player to the clicked position
                if (gridPosition != transform.position && MovementIsValid(gridPosition))
                {
                    StartCoroutine(MovePlayer(gridPosition));
                }
            }
            else
            {
                print("No collision detected");
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
        Vector3 normalizedPosition = new(transform.position.x, 0, transform.position.z);
        Vector3 normalizedTargetPosition = new(targetPosition.x, 0, targetPosition.z);
        float distance = Vector3.Distance(normalizedPosition, normalizedTargetPosition);

        print("my position: " + normalizedPosition);
        print("target: " + normalizedTargetPosition);
        print("distance: " + distance);

        return distance <= maxMoveDistance;
    }

    private IEnumerator MovePlayer(Vector3 newTargetPosition)
    {
        isMoving = true;

        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPosition, newTargetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = newTargetPosition;
        isMoving = false;
    }
}
