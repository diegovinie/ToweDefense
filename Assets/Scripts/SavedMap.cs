using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SavedMap : MonoBehaviour
{
    [System.Serializable]
    public struct ReachableSlot
    {
        public int x;
        public int z;
        public bool reachable;
    }

    public string mapName = "Saved map";
    public string description = "";
    public int width;
    public int height;
    public bool available = true;
    public List<ReachableSlot> nodes = new List<ReachableSlot>();
    public List<Map.Slot> waypoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(bool[,] slots, List<Map.Slot> _waypoints, int _width, int _height)
    {
        waypoints = _waypoints;
        width = _width;
        height = _height;

        for (int i = 0; i < width; i++)
        {
            for (int k = 0; k < height; k++)
            {
                nodes.Add(new ReachableSlot{ x = i, z = k, reachable = slots[i, k] });
            }
        }
        
        available = false;
    }
}
