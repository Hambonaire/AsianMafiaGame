using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerrrrr : MonoBehaviour {


    // Use this for initialization
    public Transform playerTransform;

    public float speedV = 2.0f;
    private float pitch = 0.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        transform.rotation = playerTransform.rotation;
        transform.position = playerTransform.position;
        */
        pitch += speedV * Input.GetAxis("Mouse Y");

        //transform.eulerAngles = new Vector3(playerTransform.eulerAngles.y, playerTransform.eulerAngles.x , playerTransform.eulerAngles.z);
    }
}
