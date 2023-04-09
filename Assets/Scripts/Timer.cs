using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [System.Serializable]
    public class Timeout
    {
        public float timeout;
        public float elapsedTime;
        public bool active = true;
        public System.Action OnTimeout;

        public Timeout(float _timeout, System.Action callback)
        {
            timeout = _timeout;
            elapsedTime = 0f;
            OnTimeout = callback;
        }
    }

    public List<Timeout> timeouts = new List<Timeout>();

    public static Timer instance;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Timeout to in timeouts)
        {
            if (!to.active) continue;

            to.elapsedTime += Time.deltaTime;

            if (to.elapsedTime > to.timeout)
            {
                if (to.OnTimeout != null)
                    to.OnTimeout();

                to.active = false;
            }
        }

        for (int i = timeouts.Count - 1; i >= 0; i--)
        {
            if (!timeouts[i].active)
                timeouts.RemoveAt(i);
        }
    }

    public void SetTimeout(float timeout, System.Action callback)
    {
        Timeout t = new Timeout(timeout, callback);
        timeouts.Add(t);
    }
}
