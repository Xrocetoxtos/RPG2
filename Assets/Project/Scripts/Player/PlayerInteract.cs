﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;

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
        cam = transform.Find("PlayerCamera").gameObject.GetComponent<Camera>();
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        crosshair.sprite = crosshairImage[0];
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
            WorldObject worldObject = hit.collider.gameObject.GetComponent<WorldObject>();
            // als dat iets raakt, check of het Worldobject is
            if (worldObject != null)
            {
                GUIShowObject(worldObject, hit);
            }
        }
    }

    private void GUIShowObject(WorldObject worldObject, RaycastHit hit)
    {
        //als dichtbij is: melding dat je er wat mee kunt doen.
        gameHandler.ViewGUImessage(gameHandler.guiMessage, worldObject.objectTitle);
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
            }
            InputInteractWorldObject(worldObject);
        }
        if (!justPickedUp && !justExamined && !justInteracted)
        {
            gameHandler.ViewGUImessage(gameHandler.guiMessage2, message2);
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
            }
        }
    }

    private void InteractWorldObject(WorldObject worldObject)
    {
        justInteracted = true;
    }

    private void PickupWorldObject(WorldObject worldObject)
    {
        gameHandler.playerInventory.AddItem(worldObject);
        gameHandler.ViewBothGUIMessages(worldObject.objectTitle, "picked up.");
        worldObject.gameObject.SetActive(false);
        justPickedUp = true;
    }

    private void ExamineWorldObject(WorldObject worldObject)
    {
        justExamined = true;
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