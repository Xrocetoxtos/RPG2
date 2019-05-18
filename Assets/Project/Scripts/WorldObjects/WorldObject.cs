using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldObject : MonoBehaviour
{
    //basisinformatie voor alle worldObjects
    public int objectNumber;
    public string objectTitle;
    public string objectDescription;
    public ObjectType objectType;
    public Sprite objectSprite;

    //GUI info
    public string useButton;
    public string dropButton;

    //staat van het object
    public int objectScene;
    public Transform objectTransform;
    public bool objectIsThere = true;

    //componenten
    public Door doorObject;
    public NPC npcObject;
    public Food foodObject;
    public Artifact artifactObject;
    public Weapon weaponObject;
    public Attire attireObject;
    public Interactable interactableObject;

    private void Awake()
    {
        ThisScene();
        GetComponents();
    }

    private void ThisScene()
    {
        if (objectScene == SceneManager.GetActiveScene().buildIndex && objectIsThere)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void GetComponents()
    {
        doorObject = GetComponentInParent<Door>();
        npcObject = GetComponent<NPC>();
        foodObject = GetComponent<Food>();
        artifactObject = GetComponent<Artifact>();
        weaponObject = GetComponent<Weapon>();
        attireObject = GetComponent<Attire>();
        interactableObject = GetComponent<Interactable>();

        objectType = ObjectType.Item;
        if (npcObject)
        {
            objectType = ObjectType.NPC;
            return;
        }
        if (doorObject)
        {
            objectType = ObjectType.Door;
            return;
        }
        if (interactableObject)
        {
            objectType = ObjectType.Interactable;
            return;
        }
    }

    public void DropItem()
    {
        gameObject.SetActive(true);
        GameObject player = GameObject.Find("Player");
        Vector3 placeDown = Vector3.forward;
        transform.position = player.transform.position + placeDown;
        objectIsThere = true;
        player.GetComponent<Inventory>().RemoveItem(this);
    }
}
