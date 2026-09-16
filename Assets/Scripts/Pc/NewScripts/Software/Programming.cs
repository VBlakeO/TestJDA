using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Programming : BasePage
{
    private const int ColorChangeKeystrokes = 10;

    public static Programming Instance;

    #region public
    [Header("===Programming===")]
    public int minimumProgressNeeded = 150;
    [SerializeField] private ProgrammingConfig config = null;
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
    [SerializeField] private ScrollRect scrollRect = null;
    [Space]

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

    private SourceCodeFeed _sourceCodeFeed = null;
    private KeystrokeRateLimiter _keystrokeLimiter = null;
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

        _sourceCodeFeed = new SourceCodeFeed(sourceCodeString, config.charsPerKeystroke, config.maxVisibleLines, config.maxVisibleCharacters);
        _keystrokeLimiter = new KeystrokeRateLimiter(config.maxKeystrokesPerSecond);
    }

    private void Update()
    {
        if (!SoftwareScreen.activeInHierarchy)
            return;

        if (!IsProgrammingAllowed())
            return;

        if (Input.anyKeyDown && ReservedKeys() && _keystrokeLimiter.TryRegister(Time.time))
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
    // The player is the lead developer, so each keystroke is worth more than an employee tick
    private void UpdateCode()
    {
        UpdateCodeProgress(config.leaderMultiplier);
        sourceCodeClickCount++;

        if (scrollRect)
            scrollRect.verticalNormalizedPosition = 0;

        _sourceCodeFeed.Advance();
        sourceCodeText.text = _sourceCodeFeed.VisibleText;
        miniSourceCodeText.text = sourceCodeText.text;

        if (sourceCodeClickCount >= ColorChangeKeystrokes)
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
        _sourceCodeFeed.Reset();
        _keystrokeLimiter.Reset();
        sourceCodeText.text = "";
        miniSourceCodeText.text = "";
        softwareTitle.text = "Programming";
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