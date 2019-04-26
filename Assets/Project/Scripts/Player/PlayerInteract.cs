using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    private GUIHandler guiHandler;
    Inventory inventory;

    [SerializeField] private float grabDistance;
    [SerializeField] private float seeDistance;
    [SerializeField] private Camera cam;

    //crosshair kan straks naar playerinteract. ook in awake.
    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite[] crosshairImage;

    //deze 3 bools zijn om de gui de juiste dingen te laten zeggen.
    private bool justPickedUp=false;
    private bool justInteracted = false;
    private bool justExamined = false;

    private void Awake()
    {
        GameObject gh = GameObject.Find("GameHandler");
        cam = transform.Find("PlayerCamera").gameObject.GetComponent<Camera>();
        gameHandler = gh.GetComponent<GameHandler>();
        guiHandler = gh.GetComponent<GUIHandler>();
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        crosshair.sprite = crosshairImage[0];
        inventory = GetComponent<Inventory>();

    }
    private void Update()
    {
        SeeObject();
    }

    //interactie met objecten
    public void SeeObject()
    {
        // een raycast vanuit de camera vooruit schieten en dan seeDistance ver
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, seeDistance))
        {
            crosshair.sprite = crosshairImage[SetCrosshairState(hit.distance)];
                
            // als dat iets raakt, check of het Worldobject is
            if (hit.collider.gameObject.tag == "InteractObject")
            {
                WorldObject worldObject = hit.collider.gameObject.GetComponent<WorldObject>();
                GUIShowObject(worldObject, hit);
            }
        }
    }

    private void GUIShowObject(WorldObject worldObject, RaycastHit hit)
    {
        //als dichtbij is: melding dat je er wat mee kunt doen.
        guiHandler.ViewGUImessage(guiHandler.guiMessage, worldObject.objectTitle);
        string message2 = "";
        if (hit.distance <= grabDistance)
        {
            switch (worldObject.objectType)
            {
                case ObjectType.NPC:
                    message2 = "Press E to talk to.";
                    break;
                case ObjectType.Item:
                    message2 = "Press E to pick up.";
                    break;
                case ObjectType.Interactable:
                    message2 = "Press E to interact.";
                    break;
                case ObjectType.Door:
                    {
                        if (worldObject.GetComponentInParent<Door>().isOpen)
                            message2 = "Press E to close.";
                        else
                            message2 = "Press E to open.";
                        break;
                    }
            }
            InputInteractWorldObject(worldObject);
        }
        if (!justPickedUp && !justExamined && !justInteracted)
        {
            guiHandler.ViewGUImessage(guiHandler.guiMessage2, message2);
        }
        justPickedUp = false;
        justExamined = false;
        justInteracted = false;
    }

    private void InputInteractWorldObject(WorldObject worldObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (worldObject.objectType)
            {
                case ObjectType.NPC:
                    InteractWorldObject(worldObject);
                    break;
                case ObjectType.Item:
                    PickupWorldObject(worldObject);
                    break;
                case ObjectType.Interactable:
                    ExamineWorldObject(worldObject);
                    break;
                case ObjectType.Door:
                    OpenCloseDoor(worldObject);
                    break;
            }
        }
    }

    private void InteractWorldObject(WorldObject worldObject)
    {
        justInteracted = true;
    }

    private void PickupWorldObject(WorldObject worldObject)
    {
        inventory.AddItem(worldObject);
        guiHandler.ViewBothGUIMessages(worldObject.objectTitle, "picked up.");
        worldObject.gameObject.SetActive(false);
        justPickedUp = true;
    }

    private void ExamineWorldObject(WorldObject worldObject)
    {
        justExamined = true;
    }

    private void OpenCloseDoor(WorldObject worldObject)
    {
        Door door = worldObject.GetComponentInParent<Door>();
        if (door.isOpen)
        {
            door.CloseDoor();
        }
        else
        {
            door.OpenDoor();
        }
    }

    private int SetCrosshairState(float distance)
    {
        if (distance < 1) return 7;
        else if (distance < 3) return 6;
        else if (distance < 5) return 5;
        else if (distance < 8) return 4;
        else if (distance < 12) return 3;
        else if (distance < 16) return 2;
        else if (distance < 20) return 1;
        return 0;
    }
}