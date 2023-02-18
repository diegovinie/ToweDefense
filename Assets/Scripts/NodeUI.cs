using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    private Node target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTarget(Node _target)
    {
        target = _target;

        ui.SetActive(true);
        transform.position = target.GetBuildPosition();
    }

    public void Hide()
    {
        ui.SetActive(false);
    }
}
