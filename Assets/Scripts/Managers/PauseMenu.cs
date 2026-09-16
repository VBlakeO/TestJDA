using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance = null;

    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject settingsMenu = null;
    [SerializeField] private GameObject graphicsMenu = null;
    [SerializeField] private GameObject audioMenu = null;
    [SerializeField] private GameObject controlsMenu = null;
    [Space]

    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private Slider musicSlider = null;
    [Space]
    
    [SerializeField] private TMP_Dropdown resolucaoDropdown = null;
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private Toggle fullScreenToggle = null;
    [Space]

    [SerializeField] private bool deletePlayerPrefs = false;

    private bool fullScreenState = false;

    private int currentResolucaoId = 0;
    private int graphicQuality = 0;
    private int windowMode = 0;

    private float sfxVolume = 0f;
    private float musicVolume = 0f;

    private bool paused = false;

    [SerializeField] private Vector2[] supportedResolutions;
    [SerializeField] private GameObject pauseCamera = null;

    [SerializeField] private TextMeshProUGUI sensibilityText = null;
    private float sensibility = 2f;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        AddResolutions();
        AdjustQualities();

        if (PlayerPrefs.HasKey("Resolutions"))
        {
            int resolutionNum = PlayerPrefs.GetInt("Resolutions");
            if (supportedResolutions.Length <= resolutionNum)
                PlayerPrefs.DeleteKey("Resolutions");
        }

        //=============== SAVES===========//
        if (PlayerPrefs.HasKey("SFX_volume"))
        {
            sfxVolume = PlayerPrefs.GetFloat("SFX_volume");
            sfxSlider.value = sfxVolume;
            //setar volumes;
        }
        else
        {
            PlayerPrefs.SetFloat("SFX_volume", 0.8f);
            sfxSlider.value = 0.8f;
            //setar volumes;
        }

        if (PlayerPrefs.HasKey("Music_volume"))
        {
            musicVolume = PlayerPrefs.GetFloat("Music_volume");
            musicSlider.value = musicVolume;
            //setar volumes;
        }
        else
        {
            PlayerPrefs.SetFloat("Music_volume", 0.8f);
            musicSlider.value = 0.8f;
            //setar volumes;
        }

        if (PlayerPrefs.HasKey("Sensibility"))
        {
            sensibility = PlayerPrefs.GetFloat("Sensibility");
            sensibilityText.text = sensibility.ToString("F1");
        }
        else
        {
            PlayerPrefs.SetFloat("Sensibility", 2f);
            sensibilityText.text = "2.0";
        }

        //=============MODO JANELA===========//
        if (PlayerPrefs.HasKey("WindowMode"))
        {
            windowMode = PlayerPrefs.GetInt("WindowMode");
            
            if (windowMode == 1)
            {
                fullScreenState = false;
                Screen.fullScreen = false;
                fullScreenToggle.isOn = false;
            }
            else
            {
                fullScreenState = true;
                Screen.fullScreen = true;
                fullScreenToggle.isOn = true;
            }
            
        }
        else
        {
            windowMode = 1;
            PlayerPrefs.SetInt("WindowMode", 1);

            fullScreenState = true;
            Screen.fullScreen = true;
            fullScreenToggle.isOn = true;
        }



        //========RESOLUCOES========//
        if (PlayerPrefs.HasKey("Resolutions"))
        {
            currentResolucaoId = PlayerPrefs.GetInt("Resolutions");
            Screen.SetResolution((int)supportedResolutions[currentResolucaoId].x, (int)supportedResolutions[currentResolucaoId].y, fullScreenState);
            //Screen.SetResolution(supportedResolutions[currentResolucaoId].width, supportedResolutions[currentResolucaoId].height, fullScreenState);
            resolucaoDropdown.value = currentResolucaoId;
        }
        else
        {
            currentResolucaoId = supportedResolutions.Length - 1;
            Screen.SetResolution((int)supportedResolutions[currentResolucaoId].x, (int)supportedResolutions[currentResolucaoId].y, fullScreenState);
            //Screen.SetResolution(supportedResolutions[currentResolucaoId].width, supportedResolutions[currentResolucaoId].height, fullScreenState);
            PlayerPrefs.SetInt("Resolutions", currentResolucaoId);
            resolucaoDropdown.value = currentResolucaoId;
        }



        //=========QUALIDADES=========//
        if (PlayerPrefs.HasKey("GraphicQuality"))
        {
            graphicQuality = PlayerPrefs.GetInt("GraphicQuality");
            QualitySettings.SetQualityLevel(graphicQuality);
            qualityDropdown.value = graphicQuality;
        }
        else
        {
            QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);
            graphicQuality = QualitySettings.names.Length - 1;
            PlayerPrefs.SetInt("GraphicQuality", graphicQuality);
            qualityDropdown.value = graphicQuality;
        }
    }

    private void AddResolutions()
    {
        resolucaoDropdown.options.Clear();

        for (int i = 0; i < supportedResolutions.Length; i++)
            resolucaoDropdown.options.Add(new TMP_Dropdown.OptionData() { text = supportedResolutions[i].x + "x" + supportedResolutions[i].y });

        resolucaoDropdown.captionText.text = "Resolutions";
    }


    private void AdjustQualities()
    {
        string[] nomes = QualitySettings.names;
        qualityDropdown.options.Clear();

        for (int i = 0; i < nomes.Length; i++)
            qualityDropdown.options.Add(new TMP_Dropdown.OptionData() { text = nomes[i] });

        qualityDropdown.captionText.text = "Quality";
    }


    public void SavePrefes()
    {
        if (fullScreenToggle.isOn == true)
        {
            windowMode = 0;
            fullScreenState = true;
        }
        else
        {
            windowMode = 1;
            fullScreenState = false;
        }

        PlayerPrefs.SetFloat("SFX_volume", sfxSlider.value);
        PlayerPrefs.SetFloat("Music_volume", musicSlider.value);
        PlayerPrefs.SetInt("GraphicQuality", qualityDropdown.value);
        PlayerPrefs.SetInt("WindowMode", windowMode);
        PlayerPrefs.SetInt("Resolutions", resolucaoDropdown.value);
        PlayerPrefs.SetFloat("Sensibility", sensibility);
        currentResolucaoId = resolucaoDropdown.value;
        ApplyPrefs();
    }

    private void ApplyPrefs() {

        sfxVolume = PlayerPrefs.GetFloat("SFX_volume");
        musicVolume = PlayerPrefs.GetFloat("Music_volume");
        sensibility = PlayerPrefs.GetFloat("Sensibility");
        //setar volumes;    
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualidadeGrafica"));
        Screen.SetResolution((int)supportedResolutions[currentResolucaoId].x, (int)supportedResolutions[currentResolucaoId].y, fullScreenState);

        PlayerMovement.Instance.mouseSensitivity = sensibility;
    }

    public void TogglePause()
    {
        paused = !paused;

        if (paused)
            Pause();
        else
            Unpause();
    }


    public void Pause()
    {
        paused = true;
        pauseCamera.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        pauseMenu.SetActive(true);
        pausePanel.SetActive(true);
    }

    public void Unpause()
    {
        paused = false;

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        graphicsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        pauseCamera.SetActive(false);
        pausePanel.SetActive(false);
        audioMenu.SetActive(false);

        pauseMenu.SetActive(true);
    }


    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenGraphicsMenu()
    {
        graphicsMenu.SetActive(true);
        audioMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void OpenAudioMenu()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }

    public void OpenControlsMenu()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }



    public void BackToPauseMenu()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SetSensibility(float value)
    {
        sensibility += value;

        if (sensibility > 8)
        {
            sensibility = 8;
        }
        else if (sensibility < 1)
        {
            sensibility = 1;
        }

        sensibilityText.text = sensibility.ToString("F1");
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }

    void OnGUI()
    {
        if(!deletePlayerPrefs)
        return;

        // if (GUI.Button(new Rect(100, 200, 200, 60), "Delete"))
        //     DeleteSave();
    }
}
