using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridCellHoverHighlight : MonoBehaviour
{
    public float highlightHeight = 5.6f;
    public Transform playerTransform;
    private Player player;
    private new Camera camera;

    public GameObject validCellHighlight;
    public GameObject invalidCellHighlight;

    private Vector3 lastGridPosition;
    private GameObject lastSpawnedHighlight;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        playerTransform = playerObject.transform;
        player = playerObject.GetComponent<Player>();
        camera = Utilities.GetMainCamera();
    }

    void Update()
    {
        if (player.IsMoving())
        {
            DestroyLastHighlight();
        }
        else
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;

            Vector3 hitPoint = Vector3.zero;
            int groundLayerMask = LayerMask.GetMask("Ground");

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundLayerMask))
            {
                hitPoint = hitInfo.point;
                Vector3 gridPosition = Utilities.AlignToGrid(hitPoint, highlightHeight);

                /// Don't show highlight on the cell the player is on
                if (
                    gridPosition.x == playerTransform.position.x
                    && gridPosition.z == playerTransform.position.z
                )
                {
                    DestroyLastHighlight();
                }
                else if (gridPosition != lastGridPosition)
                {
                    DestroyLastHighlight();

                    if (Utilities.MovementIsValid(transform.position, gridPosition, player.MaxMoveDistance()))
                    {
                        lastSpawnedHighlight = Instantiate(
                            validCellHighlight,
                            gridPosition,
                            Quaternion.identity
                        );
                    }
                    else
                    {
                        lastSpawnedHighlight = Instantiate(
                            invalidCellHighlight,
                            gridPosition,
                            Quaternion.identity
                        );
                    }
                }

                lastGridPosition = gridPosition;
            }
        }
    }

    void DestroyLastHighlight()
    {
        if (lastSpawnedHighlight)
        {
            Destroy(lastSpawnedHighlight);
        }
    }
}
