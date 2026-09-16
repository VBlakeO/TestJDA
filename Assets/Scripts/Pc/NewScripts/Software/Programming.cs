using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Programming : BasePage
{
    public static Programming Instance;

    #region public
    [Header("===Programming===")]
    public int minimumProgressNeeded = 150;
    [Space]

    [SerializeField] private TextMeshProUGUI sourceCodeText = null;
    [SerializeField] private TextMeshProUGUI miniSourceCodeText = null;
    [Space]

    [TextArea(5, 20)] public string sourceCodeString = null;
    [Space]

    [Header("===Visual===")]
    [SerializeField] private TextMeshProUGUI softwareTitle = null;
    [SerializeField] private Color[] TextColorVariation = null;

    [Header("===Scroll===")]
    [Space]
    [SerializeField] private RectTransform rectTransform = null;
    [SerializeField] private ScrollRect scrollRect = null;
    [Space]

    [SerializeField] private float scrollSpeed = 4.5f;
    [SerializeField] private NewDevOp developer = null;
    [HideInInspector] public bool externalWorkInProgress = false;

    private float progress = 0;
    public UnityAction OnCreateProject = null;

    [SerializeField] private AudioList audioList = null;
    #endregion

    [SerializeField] private GameProgress gameProgress = null;

    #region private
    private bool programingFinishing = false;
    private int sourceCodeClickCount = 0;
    private int sourceCodeScrollDelay = 0;
    private float scrollPanelSizeY = 0f;
    #endregion

    //======================//=======================//======================//

    public bool IsProgrammingAllowed()
    {
        // A programação é permitida se:
        // - O jogador está em um PC
        // - O tempo não está pausado
        // - O progresso atual é menor que o progresso necessário
        // - A programação não estiver finalizada
        // - O desenvolvedor está trabalhando em um jogo ou está trabalhando externamente
        return pc_Manager.on_PC && Time.timeScale > 0 && progress < gameProgress.mainNecessaryProgress && !programingFinishing && (developer.HasGameInProgress() || externalWorkInProgress);
    }

    public bool HasWorkInProgress()
    {
        return progress < SavableGameData.necessaryProgress && !externalWorkInProgress;
    }

    private bool ReservedKeys()
    {
        return !Input.GetKeyDown(KeyCode.Mouse0) &&
        !Input.GetKeyDown(KeyCode.Mouse1) &&
        !Input.GetKeyDown(KeyCode.Escape) &&
        !Input.GetKeyDown(KeyCode.KeypadEnter) &&
        !Input.GetKeyDown(KeyCode.Space);
    }

    private void Awake()
    {
        Instance = this;
    }

    public override void OpenSoftware()
    {
        base.OpenSoftware();

        if (!SoftwareScreen.activeInHierarchy) return;

        if (scrollPanelSizeY == 0)
            scrollPanelSizeY = rectTransform.sizeDelta.y;
    }

    private void Update()
    {
        if (!SoftwareScreen.activeInHierarchy)
            return;

        if (!IsProgrammingAllowed())
            return;

        if (Input.anyKeyDown && ReservedKeys())
            UpdateCode();

        // Cheating
        if (Input.GetKeyDown(KeyCode.F1))
            EndProgramming();
    }

    public void NewGame(string name, float progressNeeded)
    {
        pc_Manager.StartProject();

        softwareTitle.text = "Programming -" + " " + name;

        gameProgress.mainNecessaryProgress = progressNeeded;
        SavableGameData.necessaryProgress = progressNeeded;

        programingFinishing = false;

        OnCreateProject?.Invoke();

        ChangeColorText();
    }


    #region Programming
    private void UpdateCode()
    {
        UpdateCodeProgress(1);
        sourceCodeClickCount++;
        sourceCodeScrollDelay++;

        // Scroll the text
        if (scrollRect)
            scrollRect.verticalNormalizedPosition = 0;

        const int charsToAdd = 10;
        int textLength = sourceCodeText.text.Length;
       
        for (int i = textLength; i <= textLength + charsToAdd; i++)
            sourceCodeText.text += sourceCodeString[i];

        // Update the mini text
        miniSourceCodeText.text = sourceCodeText.text;

        // Change text color if click threshold is reached
        const int clickThreshold = 10;
        if (sourceCodeScrollDelay > 30)
            rectTransform.sizeDelta += new Vector2(0, scrollSpeed);

        if (sourceCodeClickCount >= clickThreshold)
            ChangeColorText();

        audioList.PlayRandonAudioClip(); 
    }

    public void UpdateCodeProgress(float value)
    {
        gameProgress.UpdateProgress(progress += value);

        if (progress >= gameProgress.mainNecessaryProgress)
            EndProgramming();
    }

    private void ResetDisplay()
    {
        sourceCodeText.text = "";
        miniSourceCodeText.text = "";
        softwareTitle.text = "Programming";
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, scrollPanelSizeY);
    }

    public void EndProgramming()
    {
        progress = 0;
        SavableGameData.necessaryProgress = 0;

        if (externalWorkInProgress)
        {
            externalWorkInProgress = false;
            pc_Manager.OpenSoftware(2);
            NewLinkedOff.Instance.PlayerPayment();
        }
        else
        {
            StartCoroutine(BackToDevelopment());
            pc_Manager.FinishedProject();
        }

        programingFinishing = true;

        audioList.PlayOnceAudioClip(0);

        ResetDisplay();
        developer.ResetBigScreen();
        gameProgress.UpdateProgress(progress);
        gameProgress.ResetProgress();
    }
    #endregion

    #region Visual
    private void ChangeColorText()
    {
        sourceCodeText.color = TextColorVariation[Random.Range(0, TextColorVariation.Length)];
        miniSourceCodeText.color = TextColorVariation[Random.Range(0, TextColorVariation.Length)];
        sourceCodeClickCount = 0;
    }

    private IEnumerator BackToDevelopment()
    {
        WaitForSeconds wfs = new(1f);
        yield return wfs;
        pc_Manager.OpenSoftware(0);
        developer.FinishedProgramming();
    }
    #endregion
}
