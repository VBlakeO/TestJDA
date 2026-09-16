using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class NewDevOp : BasePage
{
    public static NewDevOp Instance = null;

    #region Public
    [Header("=========Development=========")]
    public DevelopmentTabs[] developmentTabs = null;
    public GameObject navigationTabsButtons = null;
    public GameObject developmentSidebar = null;
    [Space]

    public Dev_SaleScreen devSaleScreen = null;
    [Space]

    [Header("=======Message=======")]
    public TextMeshProUGUI errorMessageText = null;
    [Space]

    public const float baseGamePrice = 60f;
    [Space]

    public PopUp_Mini popUpMini = null;
    public Programming programming = null;
    public TakeScreenshot screenshot = null;
    public ReputationSystem reputationSystem = null;
    public DevOp_ButtonsManager buttonsManager = null;
    [SerializeField] private ReputationSystem reputation;
    [Space]

    [Header("BigScreen")]
    public Sprite defaultImg = null;
    public Image bigScreenImage = null;
    public TextMeshProUGUI gameNameText = null;

    public UnityAction OnSellingGame = null;
    public UnityAction <int>OnGamesSoldChange = null;
    public GameProgress gameProgress= null;

    public TranslateTool translateTool = null;
    #endregion

    #region Private
    private GameInfo gameInfo = null;

    private float _estimatedPrice = 0f;
    private float _estimatedProgrammingProgress = 0f;

    private int currentTab;

    private Dev_SettingsTab devSettingsTab = null;
    private Dev_CreationTab devCreationTab = null;
    [HideInInspector]
    public Dev_PublishingTab devPublishingTab = null;
    #endregion

    public string GameN; 

    private void Awake() {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        translateTool.ChangeLanguage(GameManager.languageId);
        
        developmentTabs[0].TryGetComponent<Dev_SettingsTab>(out devSettingsTab);
        developmentTabs[1].TryGetComponent<Dev_CreationTab>(out devCreationTab);
        developmentTabs[2].TryGetComponent<Dev_PublishingTab>(out devPublishingTab);

        reputationSystem.OnTierChange += devSettingsTab.UpgradeThemes;

        float percentage = SavableGameData.reputation / reputationSystem.ReputationRequired * 100f;
        devSettingsTab.UpgradeThemes(reputationSystem.GetReputationTier(percentage));
    }

    public override void OpenSoftware()
    {
        base.OpenSoftware();
        ResetAll();
        buttonsManager.NewGameButton.interactable = true;
    }

    public void OpenTabs(int index)
    {
        if (programming.externalWorkInProgress)
        {
            popUpMini.action += CloseAllTabs;
            popUpMini.CallWarningMessage(translateTool.GetText(2), 0);
            return;
        }

        if (index == 0 && SavableGameData.gameReady.Count > 0 && !SavableGameData.gameReady[SavableGameData.gameReady.Count - 1])
        {
           popUpMini.action += CloseAllTabs;

            string warning = translateTool.GetText(3).Replace("&0", SavableGameData.gameName[SavableGameData.gameName.Count - 1]);
            popUpMini.CallWarningMessage(warning, 0);
            return;
        }

        // Fecha todas as abas
        CloseAllTabs();

        // Ativa ou desativa os botões de navegação, dependendo do índice
        navigationTabsButtons.SetActive(index < 2);

        // Ativa a aba de desenvolvimento correspondente ao índice, desde que o índice esteja dentro do intervalo válido
        developmentTabs[index].myTab.SetActive(index < developmentTabs.Length);
    }

    public void CloseAllTabs()
    {
        foreach (var tab in developmentTabs){
             tab.myTab.SetActive(false);
        }

        navigationTabsButtons.SetActive(false);
        EnableSideBar(true);
    }

    public void EnableSideBar(bool enable)
    {
        developmentSidebar.SetActive(enable);      
    }

    public bool HasGameInProgress()
    {
        if (SavableGameData.gameReady.Count > 0)
            return !SavableGameData.gameReady.LastOrDefault();
        return false;
    }
    
    //=============================Begin_Settings=============================//
    public void ChangePage(bool next)
    {
        currentTab = next? LimitMath.LimitValue(currentTab += 1, 0, 5):
        LimitMath.LimitValue(currentTab -= 1, 0, 5);

        developmentTabs[0].myTab.SetActive(currentTab < 5); //SettingTab
        developmentTabs[1].myTab.SetActive(currentTab == 5); //CreationTab

        if(currentTab == 5)
            devSettingsTab.CurrentPage = -1;

        if (developmentTabs[0].myTab.activeInHierarchy)
            devSettingsTab.ChangePage(next);
    }

    public void ChangeCategory(bool next)
    {
        if (developmentTabs[0].enabled)
            devSettingsTab.ChangeCategory(next);
    }

    public string GetGameDescription(int descriptionId)
    {
        int[] dev =  SavableGameData.savadGamesInfo[descriptionId];
        string gName = SavableGameData.gameName[descriptionId];
        string genre = devSettingsTab.CategoryGroups[0].categories[dev[0]].m_Namme[translateTool.CurrentLanguage];
        string theme = devSettingsTab.CategoryGroups[1].categories[dev[1]].m_Namme[translateTool.CurrentLanguage];
        string gamePlay = devSettingsTab.CategoryGroups[2].categories[dev[2]].m_Namme[translateTool.CurrentLanguage];
        string dimension = devSettingsTab.CategoryGroups[3].categories[dev[3]].m_Namme[translateTool.CurrentLanguage];
        string platform = devSettingsTab.CategoryGroups[4].categories[dev[4]].m_Namme[translateTool.CurrentLanguage];
        string studioName = SavableGameData.studioName != null? SavableGameData.studioName : "Default Studio";

        string artigo;

        if (dev[0] == 1 || dev[0] == 4 || dev[0] == 5)
            artigo = "uma";
        else
            artigo = "um";

        string preposition;
        
        if (dev[1] == 6 || dev[1] == 7)
            preposition = " ";
        else
            preposition = " de ";


        GameN = translateTool.CurrentLanguage switch
        {
            0 => gName + " is a new " + gamePlay + " game developed by " + studioName + ", an amazing " + genre + " " + theme + " " + dimension + " straight to your " + platform,
            1 => gName + " é um novo jogo " + gamePlay + " desenvolvido por " + studioName + ", " + artigo + " incrivel " + genre + preposition + theme + " " + dimension + " direto para o seu " + platform + ".",
            _ => gName + " is a new " + gamePlay + " game developed by " + studioName + ", an amazing " + genre + " " + theme + " " + dimension + " straight to your " + platform,
        };
        return GameN;
    }

    public bool CompleteProjectPlanning()
    {
        if (HasGameInProgress())
        {
            popUpMini.action += CloseAllTabs;
            string warning = translateTool.GetText(3).Replace("&0", SavableGameData.gameName[SavableGameData.gameName.Count - 1]);
            popUpMini.CallWarningMessage(warning, 0);
            return false;
        }

        if (devCreationTab.GameName == "")
        {
            CallErrorMessage(translateTool.GetText(0));
            return false;
        }

        if (devSettingsTab.GetTotalCust() > SavableGameData.currentMoney)
        {
            CallErrorMessage(translateTool.GetText(1));
            return false;
        }

        screenshot.TakeScreenshoot(false);
        SavableGameData.WithdrawMoney(devSettingsTab.GetTotalCust());

        Invoke(nameof(SubmitProjectForProgramming), 0.2f);

        SavableGameData.savadGamesInfo.Add(devSettingsTab.GetGamePresets());
      
        return true;
    }
     
    private void SubmitProjectForProgramming()
    {
        float baseReputationPerGame = reputation.reputationInfo.reputationPerGame;
        _estimatedPrice = baseGamePrice + (baseGamePrice * devSettingsTab.DevelopmentComplexity) + (10 * devSettingsTab.ToolsComplexity);
        float _estimatedReputation = baseReputationPerGame + (baseReputationPerGame * devSettingsTab.DevelopmentComplexity) + (10 * devSettingsTab.ToolsComplexity);
        _estimatedProgrammingProgress = programming.minimumProgressNeeded + (programming.minimumProgressNeeded * devSettingsTab.DevelopmentComplexity) + (10 * devSettingsTab.ToolsComplexity);

        SavableGameData.estimatedPrice.Add((int)_estimatedPrice);
        StoreEstimatedReputation(_estimatedReputation);

        pc_Manager.OpenSoftware(1);
        gameProgress.UpdateProgress(0);
        programming.NewGame(devCreationTab.GameName, _estimatedProgrammingProgress);

        PreSave();
        UpdateBigScreen();
    }

    // Saves from older versions have no entries for their games, so the list is padded to keep indexes aligned with gameName
    private void StoreEstimatedReputation(float estimatedReputation)
    {
        List<float> _stored = SavableGameData.estimatedReputation;
        float _baseReputation = reputation.reputationInfo.reputationPerGame;

        while (_stored.Count < SavableGameData.gameName.Count)
            _stored.Add(_baseReputation);

        _stored.Add(estimatedReputation);
    }

    private float GetEstimatedReputation(int gameIndex)
    {
        List<float> _stored = SavableGameData.estimatedReputation;

        return gameIndex < _stored.Count ? _stored[gameIndex] : reputation.reputationInfo.reputationPerGame;
    }

    public void FinishedProgramming()
    {
        SavableGameData.gameReady[SavableGameData.gameReady.Count - 1] = true;
        devPublishingTab.scrollRect.verticalNormalizedPosition = 0;

        buttonsManager.MyGamesButtonEvent();
    }

    public void CallOpenSavedGames()
    {
        devPublishingTab.OpenSavedGames();
    }

    public void SetGameInfo(GameInfo _gameInfo)
    {
        gameInfo = _gameInfo;
    }

    public void EnableSellPanel(bool active, int id)
    {
        devSaleScreen.EnableSellPanel(SavableGameData.estimatedPrice[id]);
    }

    public void SellGame()
    {
        // A complexidade do jogo deve alterar o reconhecimento recebido.
        devSaleScreen.SellGame(GetEstimatedReputation(gameInfo.gameIndex) + GetActiveMarketingReputation());
        
        devPublishingTab.GameTexture = new Texture2D[SavableGameData.gameByte.Count];
        devPublishingTab.GameSprites = new Sprite[devPublishingTab.GameTexture.Length];
        
        OnSellingGame?.Invoke();
        OnGamesSoldChange?.Invoke(SavableGameData.gamePublished.Count);
        ResetAll();
    
        gameInfo.SavePublished();
        CallOpenSavedGames();
    }

    // Recomputed on every sale so the bonus of previous games never carries over
    private float GetActiveMarketingReputation()
    {
        float _marketingReputation = 0f;

        for (int i = 0; i < SavableGameData.activatedMarketing.Length; i++)
        {
            if (SavableGameData.activatedMarketing[i])
                _marketingReputation += SavableGameData.marketingValue[i];
        }

        return _marketingReputation;
    }

    public void UpdateDevSetting()
    {
        devSettingsTab.HandlePageChange();
        devSettingsTab.HandleCategoryChange();
    }

    private void PreSave()
    {
        SavableGameData.gameName.Add(devCreationTab.GameName);
        SavableGameData.gamePublished.Add(false);
        SavableGameData.gameReady.Add(false);
        devCreationTab.ResetCreationTab();
    }

    public void ResetAll()
    {
        CloseAllTabs();

        currentTab = 0;
        devSaleScreen.sellSlider.value = 0;

        devSettingsTab.ResetSettingTab();
        devCreationTab.ResetCreationTab();
    }

    public void UpdateBigScreen()
    {
        print("UpdateBigScreen");
        if (SavableGameData.gamePublished.Count > 0 && SavableGameData.gamePublished[^1])
            return;

        if (devPublishingTab.GameSprites.Length > 0 && SavableGameData.gameName.Count > 0)
        {
            bigScreenImage.sprite = devPublishingTab.GameSprites[^1];
            gameNameText.text = SavableGameData.gameName[^1];
        }
    }

    public void ResetBigScreen()
    {
        print("ResetBigScreen");
        bigScreenImage.sprite = defaultImg;
        gameNameText.text = "";
    }

    private void CallErrorMessage(string error)
    {
        errorMessageText.text = error;
        errorMessageText.enabled = true;

        CancelInvoke();
        Invoke("DisableErrorMessage", 1f);
    }

    private void DisableErrorMessage()
    {
        errorMessageText.enabled = false;
        errorMessageText.text = "";
    }
}

[System.Serializable]
public class CategoryGroup
{
    public List<Category> categories;
}

[System.Serializable]
public static class LimitMath
{
    public static int LimitValue(int value, int minValue, int maxValue)
    {
        if (value > maxValue){
            return minValue;
        }
        else if (value < minValue){
            return maxValue;
        }
        else{
            return value;
        }
    }
}