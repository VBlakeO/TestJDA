using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerGamePresentation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameName = null;
    [SerializeField] private TextMeshProUGUI studioName = null;
    [SerializeField] private Image gameLogo = null;
    [SerializeField] private Dev_PublishingTab publishingTab = null;

    public void Initialize()
    {
        if (SavableGameData.gameName.Count > 0)
            gameName.text = SavableGameData.gameName[SavableGameData.gamePublished.Count - 1];

        studioName.text = SavableGameData.studioName;
        gameLogo.sprite = publishingTab.GameSprites[SavableGameData.gamePublished.Count - 1];
    }

}
