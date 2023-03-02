using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlign : MonoBehaviour
{
    Transform mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");

        mainCamera = camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCamera.rotation;
    }
}
