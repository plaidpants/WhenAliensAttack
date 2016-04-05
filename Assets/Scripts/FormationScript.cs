using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FormationScript : MonoBehaviour {

	public float speed;
	public float min;
	public float max;
    bool movedown = false;
    public int count;
    public float UFOPerSeconds;
    public float speedUp;
    public GameObject UFOPrefab1;
    public GameObject UFOPrefab2;
    static float elapsedTime = 0f;
    int stepSoundCount = 0;
    public float stepSoundSpeed;
    public float stepDownAmount;
    public AudioClip StepSound0;
    public AudioClip StepSound1;
    public AudioClip StepSound2;
    public AudioClip StepSound3;
    float startNewLevelTime = 0;
    public float startNewLevelDelayTime;

    public bool alienFormationMeshUpdateTrigger1 = false;
    public bool alienFormationMeshUpdateTrigger2 = false;

    //combine meshes of alien formation
    public Mesh alienFormationMesh1 = null;
    public Mesh alienFormationMesh2 = null;

    bool whichisactive = true;

    public void ToggleRight()
    {
        speed = -Mathf.Abs(speed);
        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0); ;
        movedown = true;
    }

    public void ToggleLeft()
    {
        speed = Mathf.Abs(speed);
        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0); ;
        movedown = true;
    }

    public void SpeedUp()
    {
        count--;

        if (count == 0)
        {
            startNewLevelTime = Time.time;
        }

        if (speed > 0)
        {
            speed = speed + speedUp;
            GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0); ;
        }
        else
        {
            speed = speed - speedUp;
            GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0); ;
        }
    }

    void MoveDownCheck()
    {
        if (transform.position.x > max)
        {
            speed = -Mathf.Abs(speed);
            GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0);
            movedown = true;
        }
        else if (transform.position.x < min)
        {
            speed = Mathf.Abs(speed);
            GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0);
            movedown = true;
        }

        if (movedown)
        {
            movedown = false;
            GetComponent<Rigidbody>().MovePosition(new Vector3(
                    transform.position.x,
                    transform.position.y - stepDownAmount,
                    transform.position.z));
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

    void StepSoundCheck(float deltaTime)
    {
        elapsedTime += deltaTime;

        if ((elapsedTime * Mathf.Abs(speed) > stepSoundSpeed) && (count != 0))
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
        }
    }

    void UFOCheck(float deltaTime)
    {
        float probability = UFOPerSeconds * deltaTime;
        if (Random.value < probability)
        {
            if (UFOController.count == 0)
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

    void FixedUpdate () {

        MoveDownCheck();

        //NewSceneCheck(Time.fixedTime);

        //StepSoundCheck(Time.fixedDeltaTime);

        //UFOCheck(Time.fixedDeltaTime);

        //MeshCheck();
   }

    void Update()
    {
        //MoveDownCheck();

        NewSceneCheck(Time.time);

        StepSoundCheck(Time.deltaTime);

        UFOCheck(Time.deltaTime);

        MeshCheck();
    }

    public void UpdateMesh1()
    {    
        // save current position so we can set it to zero so the localToWorldMatrix works correctly
        Vector3 position = transform.position;
        transform.position = Vector3.zero;

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

        alienFormationMeshUpdateTrigger1 = false;
    }

    public void UpdateMesh2()
    {
        // save current position so we can set it to zero so the localToWorldMatrix works correctly
        Vector3 position = transform.position;
        transform.position = Vector3.zero;

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
        alienFormationMeshUpdateTrigger2 = false;
    }

    void Start()
    {
        // start the formation moving
        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0); ;

        UpdateMesh1();
        UpdateMesh2();
        transform.GetComponent<MeshFilter>().mesh = alienFormationMesh1;
    }
}
