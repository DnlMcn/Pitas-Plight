using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 startingPosition = new(2f, 1.3f, -3f);

    private Movement movement;
    private new Camera camera;
    private bool outOfTurn = false;
    public GameEvent endPlayerTurn;

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

    public void EndTurn() {
        outOfTurn = true;
        print("Ending player turn");
    }

    public void StartTurn() {
        outOfTurn = false;
        print("Starting player turn");
    }

    public bool isPlayerLocked() {
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
            RaycastHit hitInfo;

            int groundLayerMask = LayerMask.GetMask("Ground");

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundLayerMask))
            {
                Vector3 gridPosition = Utilities.AlignToGrid(hitInfo.point, transform.position.y);

                if (gridPosition != transform.position && Utilities.MovementIsValid(transform.position, gridPosition, movement.maxMoveDistance))
                {
                    StartCoroutine(movement.MoveTo(gridPosition, Mathf.FloorToInt(movement.maxMoveDistance), endPlayerTurn));
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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
