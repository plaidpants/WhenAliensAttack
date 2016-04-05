using UnityEngine;
using System.Collections;

public class Aliens : MonoBehaviour {

	public Vector3 centrePos;
	public float levelspacing;
	public float radius;
	public int numlevels;
	public int numPoints;
	public GameObject AlienPrefab1;
	public GameObject AlienPrefab2;
	public GameObject AlienPrefab3;

	// Use this for initialization
	void Start () {
		// create tunnel of aliens
        for (int levelNum = 0; levelNum < numlevels; levelNum++)
        {
            for (int pointNum = 0; pointNum < numPoints; pointNum++)
            {
                // "i" now represents the progress around the circle from 0-1
                // we multiply by 1.0 to ensure we get a fraction as a result.
                float i = (pointNum * 1.0f) / numPoints;

                // get the angle for this step (in radians, not degrees)
                float angle = i * Mathf.PI * 2;

                // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
                float x = Mathf.Sin(angle) * radius;
                float y = Mathf.Cos(angle) * radius;

                Vector3 center = new Vector3(centrePos.x, levelNum * levelspacing + centrePos.y, centrePos.z);
                Vector3 pos = new Vector3(y, 0, x) + center;
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

                // no need to assign the instance to a variable unless you're using it afterwards:
                switch ((levelNum / 2) % 3)
                {
                    case 0:
                        {
                            GameObject alien = Instantiate(AlienPrefab1, pos, rot) as GameObject;
                            alien.transform.SetParent(transform);
                            break;
                        }
                    case 1:
                        {
                            GameObject alien = Instantiate(AlienPrefab2, pos, rot) as GameObject;
                            alien.transform.SetParent(transform);
                            break;
                        }
                    case 2:
                        {
                            GameObject alien = Instantiate(AlienPrefab3, pos, rot) as GameObject;
                            alien.transform.SetParent(transform);
                            break;
                        }
                    default:
                        {
                            GameObject alien = Instantiate(AlienPrefab1, pos, rot) as GameObject;
                            alien.transform.SetParent(transform);
                            break;
                        }
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
