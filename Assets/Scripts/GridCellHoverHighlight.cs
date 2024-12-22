using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridCellHoverHighlight : MonoBehaviour
{
    public float highlightHeight = 5.6f;
    public Transform playerTransform;

    public GameObject validCellHighlight;
    public GameObject invalidCellHighlight;

    private Vector3 lastGridPosition;
    private GameObject lastSpawnedHighlight;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 clickedPosition = hit.point;
                Vector3 gridPosition = new(
                    Mathf.Round(clickedPosition.x),
                    highlightHeight,
                    Mathf.Round(clickedPosition.z)
                );

                /// Don't show highlight on the cell the player is on
                if (gridPosition.x == playerTransform.position.x && gridPosition.z == playerTransform.position.z)
                {
                    DestroyLastHighlight();
                }
                else if (gridPosition != lastGridPosition)
                {
                    DestroyLastHighlight();
                    lastSpawnedHighlight = Instantiate(validCellHighlight, gridPosition, Quaternion.identity);
                }

                lastGridPosition = gridPosition;
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
