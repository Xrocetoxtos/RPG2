using UnityEngine;

public class EternalObjects : MonoBehaviour
{
    public static EternalObjects instance;
    //Deze klasse is op het object EternalObjects en is puur om ervoor te zorgen dat objecten blijven bestaan
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
