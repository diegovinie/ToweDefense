using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] Transform origin;
    [SerializeField] LayerMask excludedLayer;
    public bool lightEnabled;
    Light lightPoint;
    [SerializeField] Vector3 direction = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        lightPoint = GetComponent<Light>();
        lightPoint.enabled = false;
    }
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        // int layerMask = 1 << 7;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;


        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(origin.transform.position, origin.transform.TransformDirection(direction), out hit, Mathf.Infinity, ~excludedLayer))
        {
            Debug.DrawRay(origin.transform.position, origin.transform.TransformDirection(direction) * hit.distance, Color.red);
            // Debug.Log("Did Hit " + hit.collider);

            transform.position = hit.point;

            lightPoint.enabled = lightEnabled ? true : false;
        }
        else
        {
            Debug.DrawRay(origin.transform.position, origin.transform.TransformDirection(direction) * 1000, Color.blue);
            lightPoint.enabled = false;
        }
    }
}
