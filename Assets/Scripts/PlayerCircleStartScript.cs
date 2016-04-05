using UnityEngine;
using System.Collections;

public class PlayerCircleStartScript : MonoBehaviour {

    public float RespawnTime;
    float DiedTime;
    public GameObject playerPrefab;

    // Use this for initialization
    void Start()
    {
        DiedTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((PlayerCircleScript.count == 0) && (DiedTime == 0))
        {
            DiedTime = Time.fixedTime;
        }

        if (DiedTime != 0)
        {
            if (Time.fixedTime > DiedTime + RespawnTime)
            {
                GameObject newplayer = Instantiate(playerPrefab, transform.position, Quaternion.Euler(-90, 0, 180)) as GameObject;
                DiedTime = 0;
            }
        }
    }
}
