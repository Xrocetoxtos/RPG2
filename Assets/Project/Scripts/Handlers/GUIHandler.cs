using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIHandler : MonoBehaviour
{
    private GameHandler gameHandler;
    public HealthBar enemyHealth;
    public EnergyBar enemyEnergy;
    public GameObject enemyHealthCanvas;
    public TextMeshProUGUI enemyNameText;

    public GameObject mainPanel;
    public Image black;
    public TextMeshProUGUI guiMessage;
    public TextMeshProUGUI guiMessage2;
    private float guiTextTimer = 0f;
    private float guiTextTimerMax = 2f;

    public Material redCircle;
    public Material greenCircle;

    [SerializeField] private GameObject deepWaterMask;

    [Header("Dialog")]
    public GameObject dialogWindow;
    public TextMeshProUGUI dialogPartner;
    public TextMeshProUGUI dialogTitle;
    public TextMeshProUGUI dialogDescription;
    public TextMeshProUGUI dialogButton1;
    public TextMeshProUGUI dialogButton2;

    [Header("Inventory")]
    public GameObject inventoryWindow;

    private void Awake()
    {
        mainPanel = GameObject.Find("MainPanel");
        gameHandler = GetComponent<GameHandler>();
        black = GameObject.Find("BlackBackground").GetComponent<Image>();
        deepWaterMask = GameObject.Find("DeepWaterMask");
        guiMessage = GameObject.Find("GUIMessage").GetComponent<TextMeshProUGUI>();
        guiMessage.SetText("");
        guiMessage2 = GameObject.Find("GUIMessage2").GetComponent<TextMeshProUGUI>();
        guiMessage2.SetText("");
        enemyHealthCanvas.SetActive(false);
        InitDialogGUI();
        InitInventoryGUI();
    }

    private void InitDialogGUI()
    {
        dialogWindow = GameObject.Find("DialogWindow");
        dialogPartner = GameObject.Find("DialogPartner").GetComponent<TextMeshProUGUI>();
        dialogTitle = GameObject.Find("DialogTitle").GetComponent<TextMeshProUGUI>();
        dialogDescription = GameObject.Find("DialogDescription").GetComponent<TextMeshProUGUI>();
        dialogButton1 = GameObject.Find("Button1Text").GetComponent<TextMeshProUGUI>();
        dialogButton2 = GameObject.Find("Button2Text").GetComponent<TextMeshProUGUI>();
        dialogWindow.SetActive(false);
    }

    private void InitInventoryGUI()
    {
        inventoryWindow = GameObject.Find("InventoryWindow(Clone)");
        inventoryWindow.transform.SetParent(GameObject.Find("Canvas").transform);
        RectTransform rt = inventoryWindow.GetComponent<RectTransform>();

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;

        inventoryWindow.SetActive(false);

    }

    private void Update()
    {
        ReconsiderGUI();
    }

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
        if (guiTextTimer > guiTextTimerMax && !gameHandler.isPaused)
        {
            guiTextTimer = 0;
            ViewBothGUIMessages("", "");
        }
    }

    public void InventoryView()
    {
        if(gameHandler.isPaused)
        {
            gameHandler.UnlockCursor();
        }
        else
        {
            gameHandler.LockCursor();
        }
        inventoryWindow.SetActive(gameHandler.isPaused);
        mainPanel.SetActive(!gameHandler.isPaused);
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
