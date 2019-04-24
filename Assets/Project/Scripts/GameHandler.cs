﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;

    public GameObject player;
    private PlayerState playerState;
    public PlayerLook playerLook;

    [Header("Data")]
    public bool isPaused = false;

    [Header("GUI")]
    public Image black;
    public TextMeshProUGUI guiMessage;
    public TextMeshProUGUI guiMessage2;
    private float guiTextTimer = 0f;
    private float guiTextTimerMax = 2f;

    [SerializeField] private GameObject deepWaterMask;

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

        InitReferences();
        LockCursor();
    }

    private void InitReferences()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerState = player.GetComponent<PlayerState>();
            playerLook = player.transform.Find("PlayerCamera").gameObject.GetComponent<PlayerLook>();
        }

        //GUI elementen
        black = GameObject.Find("BlackBackground").GetComponent<Image>();
        deepWaterMask = GameObject.Find("DeepWaterMask");
        AboveWater();
        guiMessage = GameObject.Find("GUIMessage").GetComponent<TextMeshProUGUI>();
        guiMessage.SetText("");
        guiMessage2 = GameObject.Find("GUIMessage2").GetComponent<TextMeshProUGUI>();
        guiMessage2.SetText("");
    }

    private void Update()
    {
        ReconsiderGUI();
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause))
        {
            Paused();
        }
        TestHealthBar();
    }

    private void TestHealthBar()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerState.healthSystem.DamageHealth(5);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            playerState.healthSystem.HealHealth(5);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            playerState.healthSystem.DamageEnergy(5);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerState.healthSystem.HealEnergy(5);
        }
    }

    // Game States
    public void Paused()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            ViewBothGUIMessages("", "");
        }
        else
        {
            ViewGUImessage(guiMessage, "Game paused.");
            ViewGUImessage(guiMessage2, "");
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    //GUI
    public void ViewBothGUIMessages(string text, string text2)
    {
        ViewGUImessage(guiMessage, text);
        ViewGUImessage(guiMessage2, text2);
    }

    public void ViewGUImessage(TextMeshProUGUI tekstveld, string text, float timerMax = 3f)
    {
        tekstveld.SetText(text);

        guiTextTimer = 0f;
        guiTextTimerMax = timerMax;
    }

    private void ReconsiderGUI()
    {
        guiTextTimer += Time.deltaTime;
        if (guiTextTimer > guiTextTimerMax && !isPaused)
        {
            guiTextTimer = 0;
            ViewBothGUIMessages("", "");
        }
    }

    public void UnderWater()
    {
        deepWaterMask.SetActive(true);
    }

    public void AboveWater()
    {
        deepWaterMask.SetActive(false);
    }

}