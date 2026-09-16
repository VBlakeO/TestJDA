using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameProgress : MonoBehaviour
{
    [SerializeField] private Image[] progressBarImage = null;
    [SerializeField] private TextMeshProUGUI progressText = null;

    [Space]
    [SerializeField] private float totalProgress = 0;
    public float mainNecessaryProgress = 0;
    
    private void Start()
    {
        SaveManager.OnFinishLoad += LoadProgress;
        UpdateProgress(0);

        mainNecessaryProgress = SavableGameData.necessaryProgress;
        UpdateProgress(mainNecessaryProgress);
    }

    private void LoadProgress()
    {
        mainNecessaryProgress = SavableGameData.necessaryProgress;
        UpdateProgress(0);
        UpdateProgress(mainNecessaryProgress);
    }

    public void UpdateProgress(float progress)
    {
        totalProgress = progress;   
        progressBarImage[0].fillAmount = progress / mainNecessaryProgress;
        progressBarImage[1].fillAmount = progress / mainNecessaryProgress;
    
        if (progress > 0)
            progressText.text = (progress / mainNecessaryProgress * 100).ToString("F2") + "%";
        else
            progressText.text = "00,00%";
    }

    public void ResetProgress()
    {
        progressText.text = "";

        progressBarImage[0].fillAmount = 0f;
        progressBarImage[1].fillAmount = 0f;
    }
}
