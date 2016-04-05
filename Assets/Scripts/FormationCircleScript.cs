using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FormationCircleScript : MonoBehaviour {
    public float rotationSpeed;
    public float min;
    public float max;
    private bool movedown = false;
    public int count;
    public float UFOPerSeconds;
    public float speedUpAmount;
    private bool speedChanged = true;
    public GameObject UFOPrefab1;
    public GameObject UFOPrefab2;
    public GameObject AlienPrefab1;
    public GameObject AlienPrefab2;
    public GameObject AlienPrefab3;
    public GameObject AlienPrefab4;
    public GameObject AlienPrefab5;
    static float elapsedTime = 0f;
    int stepSoundCount = 0;
    int toggleCount = 0;
    public float stepSoundSpeed;
    public float stepDownAmount;
    public AudioClip StepSound0;
    public AudioClip StepSound1;
    public AudioClip StepSound2;
    public AudioClip StepSound3;
    float startNewLevelTime = 0;
    public float startNewLevelDelayTime;
    public float radius;
    public Vector3 centrePos;
    public int numPoints;
    public float levelspacing;
    public int numlevels;
    public int toggleCountMax;

    public bool alienFormationMeshUpdateTrigger1 = false;
    public bool alienFormationMeshUpdateTrigger2 = false;

    //combine meshes of alien formation
    public Mesh alienFormationMesh1 = null;
    public Mesh alienFormationMesh2 = null;

    bool whichisactive = true;

    public void Toggle()
    {
        rotationSpeed = -rotationSpeed;
        movedown = true;
        speedChanged = true;
    }

    public void SpeedUp()
    {
        count--;

        if (count == 0)
        {
            startNewLevelTime = Time.time;
        }

        if (rotationSpeed > 0)
        {
            rotationSpeed = rotationSpeed + speedUpAmount;
        }
        else
        {
            rotationSpeed = rotationSpeed - speedUpAmount;
        }

        speedChanged = true;
    }

    void MoveCheck()
    {
        if (movedown)
        {
            GetComponent<Rigidbody>().MovePosition(new Vector3(
                    transform.position.x,
                    transform.position.y - stepDownAmount,
                    transform.position.z));
            movedown = false;
        }

        if (speedChanged)
        {
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, rotationSpeed, 0);
            speedChanged = false;
        }
    }

    void NewSceneCheck(float time)
    {
        if ((startNewLevelTime != 0) && (time > startNewLevelTime + startNewLevelDelayTime))
        {
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void StepSoundCheck(float deltaTime)
    {
        elapsedTime += deltaTime;

        if ((elapsedTime * Mathf.Abs(rotationSpeed) > stepSoundSpeed) && (count != 0))
        {
            switch (stepSoundCount)
            {
                case 0:
                    {
                        GetComponent<AudioSource>().PlayOneShot(StepSound0);
                        break;
                    }
                case 1:
                    {
                        GetComponent<AudioSource>().PlayOneShot(StepSound1);
                        break;
                    }
                case 2:
                    {
                        GetComponent<AudioSource>().PlayOneShot(StepSound2);
                        break;
                    }
                case 3:
                    {
                        GetComponent<AudioSource>().PlayOneShot(StepSound3);
                        break;
                    }
                default:
                    {
                        stepSoundCount = 0;
                        break;
                    }
            }
            stepSoundCount++;
            if (stepSoundCount > 3)
            {
                stepSoundCount = 0;
            }
            elapsedTime = 0;

            toggleCount++;
            if (toggleCount % toggleCountMax == 0)
            {
                Toggle();
            }
        }
    }

    void UFOCheck(float deltaTime)
    {
        float probability = UFOPerSeconds * deltaTime;
        if (Random.value < probability)
        {
            if (UFOControllerCircle.count == 0)
            {
                if (Random.value < 0.5)
                {
                    GameObject UFO = Instantiate(UFOPrefab1, Vector3.zero, Quaternion.identity) as GameObject;
                }
                else
                {
                    GameObject UFO = Instantiate(UFOPrefab2, Vector3.zero, Quaternion.identity) as GameObject;
                }
            }
        }
    }

    void MeshCheck()
    {
        if (stepSoundCount % 2 == 0)
        {
            if (alienFormationMeshUpdateTrigger1 == true)
            {
                UpdateMesh1();
            }
            transform.GetComponent<MeshFilter>().sharedMesh = alienFormationMesh1;
        }
        else
        {
            if (alienFormationMeshUpdateTrigger2 == true)
            {
                UpdateMesh2();
            }
            transform.GetComponent<MeshFilter>().sharedMesh = alienFormationMesh2;
        }
    }

    void FixedUpdate()
    {
        MoveCheck();

        NewSceneCheck(Time.fixedTime);

        StepSoundCheck(Time.fixedDeltaTime);

        UFOCheck(Time.fixedDeltaTime);

        MeshCheck();
    }
    
    void Update()
    {
//        MoveCheck();

//        NewSceneCheck(Time.time);

//        StepSoundCheck(Time.deltaTime);

//        UFOCheck(Time.deltaTime);

//        MeshCheck();
    }

    public void UpdateMesh1()
    {
        // save current position so we can set it to zero so the localToWorldMatrix works correctly
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        if (whichisactive != false)
        {
            // flip the meshes
            foreach (Transform child in transform)
            {
                foreach (Transform grandchild in child.transform)
                {
                    grandchild.gameObject.SetActive(!grandchild.gameObject.activeSelf);
                }
            }
            whichisactive = false;
        }

        // gather the formation meshes together and make a single mesh to draw
        MeshFilter[] meshFilters1 = GetComponentsInChildren<MeshFilter>(); // this only returns the active meshes
        CombineInstance[] combine1 = new CombineInstance[meshFilters1.Length];
        int i = 1; // skip first mesh as it is our own

        while (i < meshFilters1.Length)
        {
            combine1[i].mesh = meshFilters1[i].sharedMesh;
            combine1[i].transform = meshFilters1[i].transform.localToWorldMatrix;
            i++;
        }

        // clear the current one if needed
        if (alienFormationMesh1 != null)
        {
            alienFormationMesh1.Clear();
        }
        else
        {
            alienFormationMesh1 = new Mesh();
        }

        alienFormationMesh1.CombineMeshes(combine1);

        //restore position
        transform.position = position;
        transform.rotation = rotation;

        alienFormationMeshUpdateTrigger1 = false;
    }

    public void UpdateMesh2()
    {
        // save current position so we can set it to zero so the localToWorldMatrix works correctly
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        if (whichisactive != true)
        {
            // flip the meshes
            foreach (Transform child in transform)
            {
                foreach (Transform grandchild in child.transform)
                {
                    grandchild.gameObject.SetActive(!grandchild.gameObject.activeSelf);
                }
            }
            whichisactive = true;
        }

        // gather the formation meshes together and make a single mesh to draw
        MeshFilter[] meshFilters2 = GetComponentsInChildren<MeshFilter>(); // this only returns the active meshes
        CombineInstance[] combine2 = new CombineInstance[meshFilters2.Length];
        int i = 1; // skip first mesh as it is our own

        while (i < meshFilters2.Length)
        {
            combine2[i].mesh = meshFilters2[i].sharedMesh;
            combine2[i].transform = meshFilters2[i].transform.localToWorldMatrix;
            i++;
        }

        // clear the current one if needed
        if (alienFormationMesh2 != null)
        {
            alienFormationMesh2.Clear();
        }
        else
        {
            alienFormationMesh2 = new Mesh();
        }

        alienFormationMesh2.CombineMeshes(combine2);

        //restore position
        transform.position = position;
        transform.rotation = rotation;

        alienFormationMeshUpdateTrigger2 = false;
    }

    // Use this for initialization
    void Start()
    {
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

                Quaternion rot;
                if (pointNum * 4 == numPoints)
                {
                    rot = Quaternion.FromToRotation(Vector3.forward, center - pos) * Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
                }

                // no need to assign the instance to a variable unless you're using it afterwards:
                switch ((levelNum / 1) % 5)
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
                    case 3:
                        {
                            GameObject alien = Instantiate(AlienPrefab4, pos, rot) as GameObject;
                            alien.transform.SetParent(transform);
                            break;
                        }
                    case 4:
                        {
                            GameObject alien = Instantiate(AlienPrefab5, pos, rot) as GameObject;
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

        // create initial single mesh for performance
        UpdateMesh1();
        UpdateMesh2();
        transform.GetComponent<MeshFilter>().mesh = alienFormationMesh1;
    }
}
