using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 startingPosition = new(2f, 1.3f, -3f);
    float height = 1.3f;

    float moveTime = 0.2f;
    private bool isMoving = false;

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
                if (gridPosition != transform.position)
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
