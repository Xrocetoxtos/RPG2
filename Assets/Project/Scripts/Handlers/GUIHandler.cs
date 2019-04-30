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

    public Image black;
    public TextMeshProUGUI guiMessage;
    public TextMeshProUGUI guiMessage2;
    private float guiTextTimer = 0f;
    private float guiTextTimerMax = 2f;

    public Material redCircle;
    public Material greenCircle;

    [SerializeField] private GameObject deepWaterMask;

    [Header("Quests")]
    public GameObject questWindow;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;

    private void Awake()
    {
        gameHandler = GetComponent<GameHandler>();
        black = GameObject.Find("BlackBackground").GetComponent<Image>();
        deepWaterMask = GameObject.Find("DeepWaterMask");
        guiMessage = GameObject.Find("GUIMessage").GetComponent<TextMeshProUGUI>();
        guiMessage.SetText("");
        guiMessage2 = GameObject.Find("GUIMessage2").GetComponent<TextMeshProUGUI>();
        guiMessage2.SetText("");
        enemyHealthCanvas.SetActive(false);

        questWindow = GameObject.Find("QuestWindow");
        questTitle = GameObject.Find("QuestTitle").GetComponent<TextMeshProUGUI>();
        questTitle = GameObject.Find("QuestDescription").GetComponent<TextMeshProUGUI>();

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

    public void UnderWater()
    {
        deepWaterMask.SetActive(true);
    }

    public void AboveWater()
    {
        deepWaterMask.SetActive(false);
    }
}
