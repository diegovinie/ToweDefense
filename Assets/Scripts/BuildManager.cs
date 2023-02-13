using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameObject turretToBuild;
    public GameObject standardTurretPrefab;
    public GameObject missileLauncherPrefab;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // turretToBuild = standardTurretPrefab;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
    }

    public void ClearTurretToBuild()
    {
        turretToBuild = null;
    }
}
