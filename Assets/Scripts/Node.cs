using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;
    private Renderer rend;
    private Color startColor;
    private GameObject turret;
    BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        GameObject turretToBuild = buildManager.GetTurretToBuild();

        if (turretToBuild == null) return;

        if (turret != null)
        {
            Debug.Log("can't build there! - TODO Display on screen");
            return;
        }

        turret = (GameObject)Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);

        buildManager.ClearTurretToBuild();
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (buildManager.GetTurretToBuild() == null) return;

        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
