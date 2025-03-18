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
    private new Camera camera;

    private void Start()
    {
        camera = Utilities.GetMainCamera();
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
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;

            int groundLayerMask = LayerMask.GetMask("Ground");

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundLayerMask))
            {
                Vector3 gridPosition = Utilities.AlignToGrid(hitInfo.point, transform.position.y);

                if (gridPosition != transform.position && Utilities.MovementIsValid(transform.position, gridPosition, movement.maxMoveDistance))
                {
                    print("Movement is valid, starting coroutine.");
                    StartCoroutine(movement.MoveTo(gridPosition, Mathf.FloorToInt(movement.maxMoveDistance)));
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
