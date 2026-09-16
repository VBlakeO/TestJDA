using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public sealed class Dev_SettingsTab : DevelopmentTabs
{
    [Header("=========Categories=========")]
    [Tooltip("Genre:0, Theme:1, Gameplay:2, Dimension:3, Plataform:4")]
    public CategoryGroup[] CategoryGroups;
    public List<Category> ExtraThemes;
    [Space]

    [Header("=======Setting Tab=======")]
    public Image iconImage = null;
    [Space]

    [SerializeField] private TextMeshProUGUI pageText = null;
    [SerializeField] private TextMeshProUGUI categoryText = null;
    [SerializeField] private TextMeshProUGUI toolCostText = null;
    [SerializeField] private TextMeshProUGUI totalDevelopmentCostText = null;
    [Space]
    
    [SerializeField] private SimpleScale difficultyBar = null; 
    [SerializeField] private RectTransform difficultyRect = null;
    [SerializeField] private RectTransform complexityRect = null;
    [Space]

    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private MarketingKing marketingKing = null;
    [SerializeField] private EmployeeManager employeeManager = null;
    [Space]
    
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private TranslateTool translateTool = null;
    
    //Genre:0, Theme:1, Gameplay:2, Dimension:3, Plataform:4;
    private int[] gamePresets = new int[5]; // The game composition
    private Category category = null;
    private float totalDevelopmentCost = 0f;

    public int CurrentPage {get; set;}
    public int ToolsComplexity {get; private set;}
    public float DevelopmentComplexity {get; private set;}

    private void OnEnable() 
    {
        toolCostText.enabled = false;
        difficultyBar.gameObject.SetActive(true);
        HandleCategoryChange();
    }

    private void Start()
    {
        translateTool.ChangeLanguage(GameManager.languageId);
    }


    public void ChangePage(bool next)
    {
        CurrentPage = next? LimitMath.LimitValue(CurrentPage += 1, 0, 4) : LimitMath.LimitValue(CurrentPage -= 1, -1, 4);
        HandlePageChange();
    }

    public void HandlePageChange()
    {
        pageText.text = translateTool.GetText(CurrentPage);
        difficultyBar.gameObject.SetActive(CurrentPage < 2);
        toolCostText.enabled = CurrentPage > 1 && CurrentPage < 5; 
        HandleCategoryChange();
        DifficultyBarEffect();
    }

    public void ChangeCategory(bool next)
    {
        int index = next? 1 : -1;

        if(CurrentPage > -1) 
            gamePresets[CurrentPage] = LimitMath.LimitValue(gamePresets[CurrentPage] += index, 0, CategoryGroups[CurrentPage].categories.Count - 1);
        
        HandleCategoryChange();
    }

    public void HandleCategoryChange()
    {
        if (CurrentPage > -1)
            category = CategoryGroups[CurrentPage].categories[gamePresets[CurrentPage]];

        UpdateVisualCategory(category.m_Icon, category.m_Namme[translateTool.CurrentLanguage], category);
        SetDevelopementCost(category.m_Cost);
        DifficultyBarEffect();
    }

    public void UpgradeThemes(int tier)
    {
        switch (tier)
        {
            case 1:
                for (int i = 0; i < 3; i++)
                    CategoryGroups[1].categories.Add(ExtraThemes[i]);
            break;

            case 2:
                for (int i = 3; i < 6; i++)
                    CategoryGroups[1].categories.Add(ExtraThemes[i]);
            break;

            case 3:
                for (int i = 6; i < 9; i++)
                    CategoryGroups[1].categories.Add(ExtraThemes[i]);
            break;
        }
    }

    private void SetDevelopementCost(float value)
    {
        //Addicionar custo de funcionario
        float developmentCost = 
        CategoryGroups[2].categories[gamePresets[2]].m_Cost +
        CategoryGroups[3].categories[gamePresets[3]].m_Cost +
        CategoryGroups[4].categories[gamePresets[4]].m_Cost;

        float marketingCost = marketingKing.GetMarketingPrice() * 0.3f;
        float employeeCost = employeeManager.GetHiredEmployeeCost() * 0.3f;

        totalDevelopmentCost = developmentCost + marketingCost + employeeCost;
        HandleDevelopementCost(value);
    }

    public void HandleDevelopementCost(float value)
    {
        toolCostText.text = translateTool.GetText(5) + value.ToString();
        toolCostText.GetComponent<SimpleScale>().ScaleObjectDelay();
        
        totalDevelopmentCostText.text = translateTool.GetText(6) + "\n $ " + totalDevelopmentCost.ToString();
        totalDevelopmentCostText.enabled = tabs[0].activeInHierarchy || tabs[1].activeInHierarchy;
        totalDevelopmentCostText.GetComponent<SimpleScale>().ScaleObjectDelay();
    }

    private void UpdateVisualCategory(Sprite icon, string name, Category category)
    {
        iconImage.sprite = icon;
        categoryText.text = name;
    }


    public void DifficultyBarEffect()
    {
        if (difficultyBar.gameObject.activeInHierarchy)
        {
            difficultyRect.sizeDelta = new Vector2(CategoryGroups[0].categories[gamePresets[0]].m_Difficulty * 875, 43);
            complexityRect.sizeDelta = new Vector2(CategoryGroups[1].categories[gamePresets[1]].m_Difficulty * 875, 43);

            difficultyRect.anchoredPosition = new Vector2(0, 0);
            complexityRect.anchoredPosition = new Vector2(difficultyRect.sizeDelta.x, 0);
        }

        ToolsComplexity = gamePresets[2] + gamePresets[3] + gamePresets[4]; 
        DevelopmentComplexity = CategoryGroups[0].categories[gamePresets[0]].m_Difficulty + CategoryGroups[1].categories[gamePresets[1]].m_Difficulty;
        
        difficultyBar.ScaleObjectDelay();
    }

    public void ResetDifficultyBarEffect()
    {
        difficultyRect.sizeDelta = new Vector2(CategoryGroups[0].categories[0].m_Difficulty * 875, 43);
        complexityRect.sizeDelta = new Vector2(CategoryGroups[1].categories[0].m_Difficulty * 875, 43);

        difficultyRect.anchoredPosition = new Vector2(0, 0);
        complexityRect.anchoredPosition = new Vector2(difficultyRect.sizeDelta.x, 0);
    }

    public void ResetSettingTab()
    {
        CurrentPage = 0;
        gamePresets = new int[5];

        ResetDifficultyBarEffect();
        HandlePageChange();
    }

    public int[] GetGamePresets()
    {
        return gamePresets;
    }

    public float GetTotalCust()
    {
        return totalDevelopmentCost;
    }

    private void OnDisable() 
    {
        difficultyBar.gameObject.SetActive(false);
    }
}