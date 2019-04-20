using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript instance = null;

    // Start is called before the first frame update
    private void Awake()
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
