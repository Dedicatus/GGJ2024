using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _mInstance = null;

    public static T Instance
    {
        get
        {
            if (_mInstance == null)
            {
                Debug.LogWarning("Instance call before init!");
            }
            return _mInstance;
        }
    }

    protected virtual void Awake()
    {
        if (_mInstance == null)
        {
            _mInstance = this as T;
        }
        else
        {
            Debug.LogError("More than one instance in scene!");
            Destroy(_mInstance.gameObject);
        }
    }
}