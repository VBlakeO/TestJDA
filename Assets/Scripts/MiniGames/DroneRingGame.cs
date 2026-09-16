using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DroneRingGame : MonoBehaviour
{
    public static DroneRingGame Instance = null;

    [SerializeField] private GameObject[] droneRing = null;
    [SerializeField] private float maxTimeInterval = 13f;

    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private Image timeLeftImageBase = null;
    [SerializeField] private Image timeLeftImage = null;

    [SerializeField] private int rewardId = 4;
    private AudioSource audioS = null;

    private float totalTime = 0f
    ;
    private int currentScore = 0;
    private bool playing = false;


    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        DisableAllRings();
        droneRing[0].SetActive(false);
        scoreText.gameObject.SetActive(false);
        timeLeftImageBase.gameObject.SetActive(false);
    }

    public void SetRingsLayer(string layer)
    {
        droneRing[0].gameObject.SetActive(layer == "Default");

        if (layer != "Default")
        {
            scoreText.gameObject.SetActive(false);
            timeLeftImageBase.gameObject.SetActive(false);

            ResetGame();

            droneRing[0].SetActive(false);
        }

        foreach (var ring in droneRing)
            ring.layer = LayerMask.NameToLayer(layer);
    }

    public void StartGame()
    {
        playing = true;
        currentScore = 1;

        foreach (var ring in droneRing)
            ring.SetActive(true);

        droneRing[0].SetActive(false);

        var audioS = GetComponent<AudioSource>();
        audioS.PlayOneShot(audioS.clip);

        scoreText.gameObject.SetActive(true);
        timeLeftImageBase.gameObject.SetActive(true);
    }

    public void AddScore()
    {
        scoreText.gameObject.SetActive(true);
        timeLeftImageBase.gameObject.SetActive(true);

        currentScore++;

        scoreText.text = currentScore.ToString() + " Pt";

        if (currentScore == 1)
            StartGame();

        if (currentScore >= droneRing.Length)
            ReceiveReward();
    }

    private void DisableAllRings()
    {
        foreach (var ring in droneRing)
            ring.SetActive(false);

        droneRing[0].SetActive(true);
    }

    private void ReceiveReward()
    {
        scoreText.text = GameManager.languageId == 0? "Success!" : "Sucesso!";
        timeLeftImageBase.gameObject.SetActive(false);
        playing = false;

        if (!SavableGameData.droneRewardReceived)
        {
           Amazonia.Instance.BuyItem(rewardId);
           SavableGameData.droneRewardReceived = true;
        }

        Invoke(nameof(ResetGame), 1.2f); 
    }

    private void ResetGame()
    {
        playing = false;
        currentScore = 0;
        totalTime = 0;
        timeLeftImage.fillAmount = 1f;

        scoreText.gameObject.SetActive(false);
        timeLeftImageBase.gameObject.SetActive(false);

        DisableAllRings();
    }

    private void FixedUpdate() 
    {
        if(!playing)
            return;

        if (currentScore <= 0f)
            return;

        totalTime += Time.deltaTime;

        timeLeftImage.fillAmount = 1f - totalTime /  maxTimeInterval;

        if (totalTime >= maxTimeInterval)
        {
            scoreText.text = GameManager.languageId == 0? "Failed!" : "Falhou!";

            foreach (var ring in droneRing)
                ring.SetActive(false);

            Invoke(nameof(ResetGame), 1.2f);
        }
    }
}