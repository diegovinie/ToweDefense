using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenericPooled<P>
{
    public P Pool { get; set; }
}

public abstract class GenericPool<T> : MonoBehaviour where T : Component
{
    public Queue<T> objects = new Queue<T>();
    [SerializeField] public T prefab;
    [SerializeField] Transform group;
    public static GenericPool<T> instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public T Get()
    {
        if (objects.Count == 0)
            Add(1);

        return objects.Dequeue();
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        T ob = Get();

        ob.transform.position = position;
        ob.transform.rotation = rotation;

        return ob;
    }

    public void Return(T ob)
    {
        ob.gameObject.SetActive(false);
        objects.Enqueue(ob);
    }

    public void Add(int count)
    {
        T ob = GameObject.Instantiate(prefab);
        ob.gameObject.SetActive(false);
        objects.Enqueue(ob);

        if (group != null)
            ob.transform.SetParent(group);
    }
}
