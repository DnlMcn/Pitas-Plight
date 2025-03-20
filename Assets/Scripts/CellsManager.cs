using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsManager : MonoBehaviour
{
    List<Transform> transforms;

    public bool IsCellOccupied(Vector3 position)
    {
        foreach (Transform t in transforms)
        {
            if (t.position.Equals(position))
            {
                return true;
            }
        }

        return false;
    }

    public void AddToTransforms(Transform transform)
    {
        transforms.Add(transform);
    }

    public void RemoveFromTransforms(Transform transform)
    {
        transforms.Remove(transform);
    }
}
