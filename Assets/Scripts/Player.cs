using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 startingPosition = new(2f, 1.3f, -3f);

    private Movement movement;

    private void Start()
    {
        movement = GetComponent<Movement>();
        transform.position = startingPosition;
    }

    private void Update()
    {
        CheckForMoveCommand();
    }

    private void CheckForMoveCommand()
    {
        if (movement.isMoving)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                print(hit.point);

                Vector3 clickedPosition = hit.point;
                Vector3 gridPosition = Utilities.AlignToGrid(clickedPosition);

                if (gridPosition != transform.position && Utilities.MovementIsValid(transform.position, gridPosition, movement.maxMoveDistance))
                {
                    StartCoroutine(movement.MoveTo(gridPosition));
                }
            }
        }
    }

    public bool IsMoving()
    {
        return movement.IsMoving();
    }

    public float MaxMoveDistance()
    {
        return movement.maxMoveDistance;
    }
}
