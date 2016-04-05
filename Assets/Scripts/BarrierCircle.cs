using UnityEngine;
using System.Collections;

public class BarrierCircle : MonoBehaviour
{
    public GameObject BarrierPrefab;
    public float radius;
    public int numPoints;
    public Vector3 center;

    // Use this for initialization
    void Start ()
    {
        for (int pointNum = 0; pointNum < numPoints; pointNum++)
        {
            // "i" now represents the progress around the circle from 0-1
            // we multiply by 1.0 to ensure we get a fraction as a result.
            float i = (pointNum * 1.0f) / numPoints;

            // get the angle for this step (in radians, not degrees)
            float angle = i * Mathf.PI * 2;

            // the X & Y position for this angle are calculated using Sin & Cos
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            Vector3 pos = new Vector3(y, 0, x) + center;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

            // no need to assign the instance to a variable unless you're using it afterwards:
            GameObject barrier = Instantiate(BarrierPrefab, pos, rot) as GameObject;
            barrier.transform.SetParent(transform);
        }
    }
}
