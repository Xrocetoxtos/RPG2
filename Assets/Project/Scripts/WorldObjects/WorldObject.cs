using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldObject : MonoBehaviour
{
    //basisinformatie voor alle worldObjects
    public int objectNumber;
    public string objectTitle;
    public string objectDescription;
    public ObjectType objectType;

    //staat van het object
    public int objectScene;
    public Transform objectTransform;
    public bool objectIsThere = true;

    //deurgegevens
    public Door door;

    private void Awake()
    {
        objectTransform = transform;
        if (objectType ==ObjectType.Door)
        {
            Debug.Log("deur");
            door = GetComponentInParent<Door>();
        }
        if(objectScene == SceneManager.GetActiveScene().buildIndex && objectIsThere)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void PutDownObject(Transform place)
    {
        objectTransform = place;
        objectIsThere = true;
        gameObject.SetActive(true);
    }
}
