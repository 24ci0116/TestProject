using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public GameObject player;
    Transform myTransform;
    public GameObject water;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        myTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerPosY = player.transform.position.y;

        if(0 <= playerPosY)
        {
            myTransform.position = new Vector3(0, playerPosY , -10);
        }
        else
        {
            myTransform.position = new Vector3(0, 0, -10);
        }
    }
}
