using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private bool isPersistent;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Check the scene for an instance
                _instance = FindObjectOfType<T>();
            }

            // Secondary check to see if there is an instance is found in the scene
            if (_instance == null)
            {
                // Load a prefab from Resources/Singletons with the name of the class
                var prefab = Resources.Load<T>($"Singletons/{typeof(T).Name}");

                if (prefab != null)
                {
                    // Set instance to the instantiated object
                    _instance = Instantiate(prefab);
                }
                else
                {
                    // There is no prefab available
                    _instance = null;
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            // Set this as the singleton instance
            _instance = this as T;
        }
        else
        {
            // Check if instance is not this instance
            if (_instance != this as T)
            {
                // Destroy the unused instance
                Destroy(gameObject);
            }
        }

        if (isPersistent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}