using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsManager : MonoBehaviour
{
    List<Transform> transforms;

    void OnEnable()
    {
        transforms = new();
    }

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

    public bool MovementIsValid(Vector3 initialPosition, Vector3 targetPosition, float maxMoveDistance)
    {
        return Utilities.MovementIsInRange(initialPosition, targetPosition, maxMoveDistance) && !IsCellOccupied(targetPosition);
    }

    public List<Transform> GetPlayerTransforms()
    {
        List<Transform> players = new();
        foreach (Transform transform in transforms)
        {
            if (transform.gameObject.CompareTag("Player"))
            {
                players.Add(transform);
            }
        }
        return players;
    }

    public Transform GetClosestPlayer(Vector3 position)
    {
        List<Transform> players = GetPlayerTransforms();

        if (players.Count == 0)
        {
            return null;
        }

        Transform closestTransform = players[0].transform;
        float closestDistance = Vector3.Distance(position, closestTransform.position);

        for (int i = 1; i < players.Count - 1; i++)
        {
            Transform newTransform = players[i].transform;
            float newDistance = Vector3.Distance(position, newTransform.position);

            if (newDistance < closestDistance)
            {
                closestTransform = newTransform;
                closestDistance = newDistance;
            }
        }

        return closestTransform;
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
