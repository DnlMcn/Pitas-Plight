using UnityEngine;

public static class Utilities
{
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
}
