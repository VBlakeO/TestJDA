using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Outdoor : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private TextMeshProUGUI descriptionText = null;
    [SerializeField] private GameObject cover = null;
    [Space]

    [SerializeField] private Dev_PublishingTab devPublishingTab = null;
    private NewDevOp devOp = null;

    // Start is called before the first frame update
    void Start()
    {
        devOp = NewDevOp.Instance;
        devOp.OnSellingGame += UpdateInfo;
        UpdateInfo();
    }

    void UpdateInfo()
    {
        if (!SavableGameData.activatedMarketing[1])
            return;

        if (SavableGameData.gameName.Count == 0)
        return;

        cover.SetActive(false);

        devPublishingTab.LoadSprites();
        logo.sprite = devPublishingTab.GameSprites[SavableGameData.gameName.Count - 1];
        descriptionText.text = devOp.GetGameDescription(SavableGameData.gameName.Count - 1);
    }
}
