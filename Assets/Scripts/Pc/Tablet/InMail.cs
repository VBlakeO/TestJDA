using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InMail : MonoBehaviour
{
    public static InMail Instance = null;

    [SerializeField] private Email[] firstEmail = null;
    [SerializeField] private Email[] commonEmails = null;
    [SerializeField] private Email fakeNewsEmail = null;
    [Space]

    [SerializeField] private GameObject emailAlertPrefab = null;
    [Space]

    [SerializeField] private Transform tabletContent = null;
    [Space]

    [SerializeField] private TextMeshProUGUI emailText = null;
    [SerializeField] private GameObject emailPanel = null;
    [SerializeField] private Scrollbar scrollbar = null;
    [Space]

    [SerializeField] private AudioSource audioS = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        NewDevOp.Instance.OnSellingGame += PrepareEmail;
        CloseEmail();
    }

    private void PrepareEmail()
    {   
        if (SavableGameData.gamePublished.Count == 1)
            Invoke(nameof(SendEmail), 2f);
        else if (Random.Range(0f, 101f) > 60)
            Invoke(nameof(SendEmail), 2f);
    }

    private void SendEmail()
    {
        Email _email = commonEmails[Random.Range(0, commonEmails.Length)];
     
        if (SavableGameData.gamePublished.Count == 1)
        {
            int _Type = Random.Range(0f, 101f) < 70 ? 0 : 1;
            _email = firstEmail[_Type];
        }

        string _subjectGameName = _email.subject[GameManager.languageId].Replace("gameName", SavableGameData.gameName[^1]);
        string _subjectStudioName = _subjectGameName.Replace("studioName", SavableGameData.studioName);

        EmailAlert emailAlert = Instantiate(emailAlertPrefab, tabletContent).GetComponent<EmailAlert>();
        emailAlert.SetInfo(_subjectStudioName, _email);
        NewEmailSound();
    }

    public void SendFakeNewsEmail()
    {
        EmailAlert emailAlert = Instantiate(emailAlertPrefab, tabletContent).GetComponent<EmailAlert>();
        emailAlert.SetInfo(fakeNewsEmail.subject[GameManager.languageId], fakeNewsEmail);
        NewEmailSound();
    }

    public void OpenEmail(Email _email)
    {
        string _emailGameName = _email.email[GameManager.languageId].Replace("gameName", SavableGameData.gameName[^1]);
        string _emailStudioName = _emailGameName.Replace("studioName", SavableGameData.studioName);

        emailText.text = _emailStudioName;
        //emailText.rectTransform.sizeDelta = new Vector2(emailText.rectTransform.sizeDelta.x, emailText.preferredHeight);
        emailPanel.SetActive(true);
        emailText.rectTransform.sizeDelta = new Vector2(emailText.rectTransform.sizeDelta.x, emailText.preferredHeight);
    }

    private void NewEmailSound()
    {
        audioS.PlayOneShot(audioS.clip);
    }

    public void CloseEmail()
    {
        scrollbar.value = 1f;
        emailPanel.SetActive(false);
    }
}
