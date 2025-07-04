using System;
using UnityEngine;
using UnityEngine.Events;
using static Events;

public class Player : MonoBehaviour
{
    public Vector3 startingPosition;

    private Movement movement;
    private new Camera camera;
    private bool outOfTurn = true;
    public UnityEvent endPlayerTurn;
    public int playerId;

    public CellsManager cellsManager;

    void OnDisable()
    {
        cellsManager.RemoveFromTransforms(transform);
    }

    private void Start()
    {
        camera = Utilities.GetMainCamera();
        movement = GetComponent<Movement>();
        transform.position = startingPosition;
        cellsManager.AddToTransforms(transform);
    }

    private void Update()
    {
        CheckForMoveCommand();
    }

    public void StartTurn()
    {
        outOfTurn = false;
        print("Starting player " + playerId + "'s turn");
    }

    public bool IsPlayerLocked()
    {
        return outOfTurn;
    }

    private void CheckForMoveCommand()
    {
        if (movement.isMoving || outOfTurn)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 gridPosition = Utilities.AlignToGrid(hitInfo.point, transform.position.y);

                if (gridPosition != transform.position && cellsManager.MovementIsValid(transform.position, gridPosition, movement.maxMoveDistance))
                {
                    StartCoroutine(movement.MoveTo(gridPosition, Mathf.FloorToInt(movement.maxMoveDistance), allowAirstep: true));

                    outOfTurn = true;
                    print("Ending player " + playerId + "'s turn");
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
