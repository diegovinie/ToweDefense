using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusable : MonoBehaviour
{
    public CameraController cameraController;
    public float clickDelay = .5f;
    private float clickTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (clickTimer > 0f) clickTimer -= Time.deltaTime;
        if (clickTimer < 0f) clickTimer = 0f;
    }

    void OnMouseDown()
    {
        if (clickTimer == 0f)
        {
            clickTimer = clickDelay;
        }
        else
        {
            if (clickTimer > 0f)
            {
                cameraController.FocusOnTarget(gameObject.transform);
            }
            clickTimer = 0f;
        }
    }
}
