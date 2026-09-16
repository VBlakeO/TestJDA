using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Dev_SaleScreen : MonoBehaviour
{
    public Slider sellSlider = null;
    [SerializeField] private Image sellSliderImage = null;
    [SerializeField] private Image sellHandleImage = null;
    [SerializeField] private GameObject sellPanelObject = null;
    [SerializeField] private TextMeshProUGUI gameSalePriceText = null;
    [Space]

    [SerializeField] private Color[] complexityColor;
    [Space]

    [SerializeField] private MarketingKing marketingKing = null;

    private int gameSalesMargin = 0;
    private int marketingSalesMargin = 0;
    private float gameSalePrice = 0f;
    private float estimatedGamePrice = 0f;
    private readonly float estimatedGamePriceOffset = 16;

    public void EnableSellPanel(float _estimatedPrice)
    {
        sellPanelObject.SetActive(true);

        float estimatedPriceVariation = 30f;
        sellSlider.minValue = _estimatedPrice - estimatedPriceVariation;
        sellSlider.maxValue = _estimatedPrice + estimatedPriceVariation;
        sellSlider.value = _estimatedPrice;

        estimatedGamePrice = _estimatedPrice;
        gameSalePrice = estimatedGamePrice;

        sellHandleImage.color = complexityColor[1];
        sellSliderImage.color = complexityColor[1];
    }

    public void DisableSellPanel()
    {
        sellPanelObject.SetActive(false);
    }

    public void SetGamePrice(float value)
    {
        gameSalePrice = value;
        gameSalePriceText.text = "$ " + (int)gameSalePrice;
        CompensationForComplexity();
    }

    public void SellGame(float estimatedReputation)
    {
        marketingSalesMargin = 0;

        for (int i = 0; i < SavableGameData.activatedMarketing.Length; i++)
        {
            if(SavableGameData.activatedMarketing[i])
                marketingSalesMargin += (int)marketingKing.marketingStrategy[i].disclosure;
        }

        SavableGameData.gamePrice.Add(gameSalePrice);
        SaveManager.Instance.InstantiateSales(gameSalePrice, gameSalesMargin + marketingSalesMargin);
        ReputationSystem.m_Instance.ApplyReputation(estimatedReputation);
        
        NewDevOp.Instance.ResetBigScreen();
        DisableSellPanel();
    }

    public void CompensationForComplexity()
    {
        int complex = 0;

        if (gameSalePrice < estimatedGamePrice)
        {
            complex = 0;
            gameSalesMargin = Random.Range(100, 115);
        }

        if (gameSalePrice >= estimatedGamePrice && gameSalePrice < estimatedGamePrice + estimatedGamePriceOffset)
        {
            complex = 1;
            gameSalesMargin = Random.Range(76, 101);
        }

        if (gameSalePrice >= estimatedGamePrice + estimatedGamePriceOffset)
        {
            complex = 2;
            gameSalesMargin = Random.Range(45, 57);
        }

        sellHandleImage.color = complexityColor[complex];
        sellSliderImage.color = complexityColor[complex];
    }
}
