using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldObject : MonoBehaviour
{
    private Inventory playerInventory;
    private Transform dropLocation;
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
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
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
        dropLocation = GameObject.Find("DropLocation").transform;
        transform.position = dropLocation.position;
        objectIsThere = true;
        playerInventory.RemoveItem(this);
    }
}
