using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Events;

public class GridCellHoverHighlight : MonoBehaviour
{
    public GameObject playerOne;

    public float highlightHeight = 5.6f;
    public Transform activePlayerTransform;
    private Player activePlayer;
    private new Camera camera;
    public CellsManager cellsManager;

    public GameObject validCellHighlight;
    public GameObject invalidCellHighlight;

    private Vector3 lastGridPosition;
    private GameObject lastSpawnedHighlight;

    void OnEnable()
    {
        EventBus<UpdateActivePlayer>.OnEvent += UpdateActivePlayer;
    }

    void OnDisable()
    {
        EventBus<UpdateActivePlayer>.OnEvent += UpdateActivePlayer;
    }

    void Start()
    {
        activePlayerTransform = playerOne.transform;
        activePlayer = playerOne.GetComponent<Player>();
        camera = Utilities.GetMainCamera();
    }

    void UpdateActivePlayer(UpdateActivePlayer evt)
    {
        activePlayerTransform = evt.gameObject.transform;
        activePlayer = evt.gameObject.GetComponent<Player>();
    }

    void Update()
    {
        if (activePlayer.IsMoving() || !activePlayerTransform)
        {
            DestroyLastHighlight();
        }
        else if (activePlayerTransform)
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
                    gridPosition.x == activePlayerTransform.position.x
                    && gridPosition.z == activePlayerTransform.position.z
                )
                {
                    DestroyLastHighlight();
                }
                else if (gridPosition != lastGridPosition)
                {
                    DestroyLastHighlight();

                    if (!cellsManager.MovementIsValid(activePlayerTransform.position, gridPosition, activePlayer.MaxMoveDistance()) || activePlayer.IsPlayerLocked())
                    {
                        lastSpawnedHighlight = Instantiate(
                            invalidCellHighlight,
                            gridPosition,
                            Quaternion.identity
                        );
                    }
                    else
                    {
                        lastSpawnedHighlight = Instantiate(
                            validCellHighlight,
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
