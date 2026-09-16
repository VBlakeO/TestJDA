using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MyStudio : BasePage
{
    public static MyStudio m_Instance;
    [Header("=========Studio=========")]
    public RendererMaterialArrayColorSet[] walls = null;
    public RendererMaterialArrayColorSet[] grounds = null;
    
    [Header("=========Studio Logo=========")]
        public GameObject colorGridObject = null;
    public Color[] colorsInGrid;
    public Color[] wallColorsInGrid; 
    public Color[] floorColorsInGrid;
    [Space]

    public Image[] colorSlots;
    public Image[] colorSlotsBackground;
    [Space]

    public Image[] IconParts;
    [Space]

    public Sprite[] iconSprites;
    public Sprite[] frameSprites;

    private int colorSlot = 0;
    private int currentIconIndex = 0;
    private int currentFrameIndex = 0;
    [Space]

    public TakeScreenshot screenshot;
    private Texture2D _studioTexture;
    private Sprite _studioSprite;
    public Image studioLogo = null;
    public Image[] bigScreen = null;

    [Header("=========End=========")]
    public TMP_InputField nameInputField = null;
    public GameObject informaationTab = null;
    [Space]

    public TextMeshProUGUI studioInformationsText = null;
    public TextMeshProUGUI gameReleasedText = null;
    public TextMeshProUGUI employeesText = null;
    public TextMeshProUGUI moneyText = null;
    [Space]

    public Button studioInfoButtonn = null;
    public Button backButton = null;
    [Space]

    public Image reputationBar = null;
    [Space]

    [SerializeField] TutorialManager tutorialManager = null;

    public PopUp_Mini popUpMini = null;
    public TranslateTool translateTool = null;

    private string tempName = null;

    #region part_1

    private void Awake()
    {
        m_Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        translateTool.ChangeLanguage(GameManager.languageId);

        studioInfoButtonn.onClick.AddListener(OpenInfoTab);
        backButton.onClick.AddListener(CloseInfoTab);
    }

    public override void OpenSoftware()
    {
        base.OpenSoftware();

        if (SavableGameData.studioName != null && SavableGameData.studioName != "")
            studioInformationsText.text = SavableGameData.studioName;
        else
            studioInformationsText.text = translateTool.GetText(5);
    }

    public void SetStudioName(string name)
    {
        tempName = name;
    }

    public void ConfirmName()
    {
        if (tempName != null && tempName != "")
        {
            screenshot.TakeScreenshoot(true);
            SavableGameData.studioName = tempName;
            studioInformationsText.text = tempName;
            Invoke("OpenConfirmationPopUp", 0.6f);
        }
        else
        {
            popUpMini.CallWarningMessage(translateTool.GetText(1), 0);
            studioInformationsText.text = translateTool.GetText(5);
        }

        nameInputField.text = null;
    }

    private void OpenConfirmationPopUp()
    {
        popUpMini.CallWarningMessage(translateTool.GetText(0) + SavableGameData.studioName + "!", 0);
        tutorialManager.CloseMessage();
        tutorialManager.tutorialCompleted = true;
    }

    public void OpenInfoTab()
    {
        informaationTab.SetActive(true);

        gameReleasedText.text = translateTool.GetText(2) + SavableGameData.gamePrice.Count;
        employeesText.text = translateTool.GetText(3) + SaveManager.Instance.employeeList.Count;
        moneyText.text = translateTool.GetText(4) + SavableGameData.CurrentMoney;

        reputationBar.fillAmount = SavableGameData.reputation * 0.01f;
    }

    public void CloseInfoTab()
    {
        informaationTab.SetActive(false);
    }

    public override void CloseSoftware()
    {
        base.CloseSoftware();
        CloseInfoTab();
    }
    #endregion part_1

    public void SetWallColor(int id)
    {
        if (walls.Length == 0)
            return;

        foreach (RendererMaterialArrayColorSet wall in walls)
        {
            wall.colors[0] = wallColorsInGrid[id];
            wall.UpadateColor();
        }
    }

    public void SetColumnsColor(int id)
    {
        if (walls.Length == 0)
            return;

        foreach (RendererMaterialArrayColorSet wall in walls)
        {
            wall.colors[1] = wallColorsInGrid[id];
            wall.UpadateColor();
        }
    }

    public void SetGroundColor(int id)
    {
        if (grounds.Length == 0)
            return;

        foreach (RendererMaterialArrayColorSet ground in grounds)
        {
            ground.colors[0] = floorColorsInGrid[id];
            ground.UpadateColor();
        }
    }

    #region LogoCreation
    // Select color
    public void SelectColorSlot(int slotId)
    {
        DisableColorSlotBackgrounds();
        colorSlotsBackground[slotId].enabled = true;
        colorGridObject.SetActive(true);
        colorSlot = slotId;
    }

    public void SelectColorInGrid(int colorId)
    {
        Color color = colorsInGrid[colorId];
        DisableColorSlotBackgrounds();

        colorSlots[colorSlot].color = color;
        IconParts[colorSlot].color = color;;
        colorGridObject.SetActive(false);
    }

    private void DisableColorSlotBackgrounds()
    {
        foreach (var colorSlot in colorSlotsBackground){
            colorSlot.enabled = false;
        }
    }

    // Select Icon, Frame, Background
    public void SelectIconSprite(int index)
    {
        currentIconIndex = LimitMath.LimitValue(currentIconIndex + index, 0, iconSprites.Length - 1);
        IconParts[0].sprite = iconSprites[currentIconIndex];
    }

    public void SelectFrameSprite(int index)
    {
        currentFrameIndex = LimitMath.LimitValue(currentFrameIndex + index, 0, frameSprites.Length - 1);
        IconParts[1].sprite = frameSprites[currentFrameIndex];
    }

    private int GetScreenSize() => (int)(Screen.width * (26.67f / 100f));

    public void LoadSprites()
    {
        _studioTexture = new Texture2D(1, 1);
        _studioTexture.LoadImage(SavableGameData.studioByte);
        _studioSprite = Sprite.Create(_studioTexture, new Rect(0, 0, GetScreenSize(), GetScreenSize()), new Vector2(0, 0), .01f);

        studioLogo.sprite = _studioSprite;

        foreach (Image item in bigScreen)
        {
            item.sprite = _studioSprite;
            item.color = Color.white;
        }
    }
    #endregion
}