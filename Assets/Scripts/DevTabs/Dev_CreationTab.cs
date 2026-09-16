using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Dev_CreationTab : DevelopmentTabs
{
    #region public
    [SerializeField] private TMP_InputField inputField = null;
    [Space]

    [SerializeField] private GameObject colorGridObject = null;
    [SerializeField] private Color[] colorsInGrid;
    [Space]

    [SerializeField] private Image[] colorSlots;
    [SerializeField] private Image[] colorSlotsBackground;
    [Space]

    [SerializeField] private Image[] IconParts;
    [Space]

    [SerializeField] private Sprite[] iconSprites;
    [SerializeField] private Sprite[] frameSprites;
    public Sprite[] backgroundSprites;
    #endregion

    #region private
    private string gameName = "";

    private int colorSlot = 0;

    private int currentIconIndex = 0;
    private int currentFrameIndex = 0;
    private int currentBackgroundIndex = 0;
    #endregion

    public string GameName { get => gameName; private set => gameName = value; }

    public void SetGameName(string _name)
    {
        gameName = _name;
    }

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

    public void SelectBackgroundSprite(int index)
    {
        currentBackgroundIndex = LimitMath.LimitValue(currentBackgroundIndex + index, 0, backgroundSprites.Length - 1);
        IconParts[2].sprite = backgroundSprites[currentBackgroundIndex];
        IconParts[3].sprite = backgroundSprites[currentBackgroundIndex];
    }


    public void ResetCreationTab()
    {
        colorSlot = 0;
        GameName = "";
        inputField.text = "";

        IconParts[0].color = Color.white;
        IconParts[1].color = Color.white;
        IconParts[2].color = Color.white;

        IconParts[0].sprite = iconSprites[0];
        IconParts[1].sprite = frameSprites[0];
        IconParts[2].sprite = backgroundSprites[0];
        IconParts[3].sprite = backgroundSprites[0];

        foreach (var colorSlot in colorSlots)
        {
            colorSlot.color = Color.white;
        }
    }
}
