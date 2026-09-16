using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EmailAlert : MonoBehaviour
{
    public Image alert = null;
    public TextMeshProUGUI subject = null;
    [Space]

    public Email email = null;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenEmail);
    }

    public void SetInfo(string _subject, Email _email)
    {
        subject.text = _subject;
        email = _email;
        alert.enabled = true;
    }  

    private void OpenEmail()
    {
        InMail.Instance.OpenEmail(email);
        Destroy(gameObject, 0.5f);
    }
}
