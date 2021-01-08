using UnityEngine;

public static class Helpers
{
    /// <summary>
    /// Tests is a Vector3 is inside a cone 
    /// </summary>
    /// <param name="inputPoint">Position of the object we want to test</param>
    /// <param name="outputPoint">The 'tip' of the cone</param>
    /// <param name="direction">The vector direction of the cone</param>
    /// <param name="cutoff">Radius of the cone</param>
    /// <returns></returns>
    public static bool TestCone(Vector3 inputPoint, Vector3 outputPoint, Vector3 direction, float cutoff)
    {
        float cosAngle = Vector3.Dot(
            (inputPoint - outputPoint).normalized,
            direction);
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
        return angle < cutoff;
    }
}