using System.Collections;
using UnityEngine;

public static class Utilities
{
    static float gridHeight = 0; 

    /// <summary>
    /// Rounds a Vector3 to the nearest grid-aligned position.
    /// </summary>
    /// <param name="position">The Vector3 to round.</param>
    /// <param name="height">The value the returned Vector3 should have in the Y axis.</param>
    /// <returns>A grid-aligned Vector3.</returns>
    public static Vector3 AlignToGrid(Vector3 position, float height)
    {
        float x = Mathf.Round(position.x);
        float z = Mathf.Round(position.z);

        return new Vector3(x, height, z);
    }

    /// <summary>
    /// Rounds a Vector3 to the nearest grid-aligned position. Doesn't align the Y axis.
    /// </summary>
    /// <param name="position">The Vector3 to round.</param>
    /// <returns>A grid-aligned Vector3.</returns>
    public static Vector3 AlignToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x);
        float z = Mathf.Round(position.z);

        return new Vector3(x, position.y, z);
    }

    public static bool FloatsEq(float a, float b)
    {
        return Mathf.Abs(a - b) < 0.01f;
    }

    public static bool MovementIsValid(Vector3 current, Vector3 target, float maxMoveDistance)
    {
        int currentX = Mathf.RoundToInt(current.x);
        int currentZ = Mathf.RoundToInt(current.z);
        int targetX = Mathf.RoundToInt(target.x);
        int targetZ = Mathf.RoundToInt(target.z);

        int distance = Mathf.Abs(targetX - currentX) + Mathf.Abs(targetZ - currentZ);

        return distance <= maxMoveDistance;
    }

    /// <summary>
    /// Returns a `bool` indicating whether `other` is within `range` of `self` and sharing either a row or a column with it.
    /// </summary>
    /// <param name="self">The caller's position.</param>
    /// <param name="other">The target's position.</param>
    /// <param name="range">The maximum distance between `self` and `other` before returning `false`.</param>
    public static bool IsInLinearRange(Vector3 self, Vector3 other, float range)
    {
        if (FloatsEq(self.x, other.x))
        {
            return Mathf.Abs(self.z - other.z) <= range;
        }
        else if (FloatsEq(self.z, other.z))
        {
            return Mathf.Abs(self.x - other.x) <= range;
        }
        else
        {
            return false;
        }
    }

    public static Camera GetMainCamera()
    {
        return GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public static IEnumerator Wait(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
    }
}
