using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null && Application.isPlaying)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                    }
                }

                return _instance;
            }
        }
    }

    virtual protected void Awake()
    {
        InstanceCheck();
    }

    virtual protected bool InstanceCheck()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Duplicate singleton of " + this.name + " encountered. Get out of here, son!!!!!!");
            Destroy(this.gameObject);
            return true;
        }
        else return false;
    }
}