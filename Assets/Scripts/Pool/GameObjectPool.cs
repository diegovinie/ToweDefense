using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObjectPooled
{
    public GameObjectPool Pool { get; set; }
}

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] public Queue<GameObject> objects = new Queue<GameObject>();
    [SerializeField] public GameObject prefab;
    public static GameObjectPool instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public GameObject Get()
    {
        if (objects.Count == 0)
            Add(1);

        return objects.Dequeue();
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject go = Get();

        go.transform.position = position;
        go.transform.rotation = rotation;

        return go;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        // Debug.Log("enqueue back");
        objects.Enqueue(go);
    }

    public void Add(int count)
    {
        // Debug.Log("Adding " + count + ", count: " + objects.Count);
        GameObject go = GameObject.Instantiate(prefab);
        go.SetActive(false);
        objects.Enqueue(go);
    }
}

