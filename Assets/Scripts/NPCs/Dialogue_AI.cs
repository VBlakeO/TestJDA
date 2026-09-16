using UnityEngine;

public class Dialogue_AI : MonoBehaviour
{
    [SerializeField] private TranslateTool translateTool = null;
    private DialogueManager dialogueManager = null;
    private Employee employee = null;

    private void Start() 
    {
        dialogueManager = DialogueManager.Instance;
        employee = GetComponent<Employee>();
    }

    private void OnEnable() 
    {
        translateTool.ChangeLanguage(GameManager.languageId);
    }

    public void Greeting()
    {
        string text = translateTool.GetText(Random.Range(0, 2)).ToString();
        dialogueManager.Speak(text, employee.id);
    }

    public void UponWaking()
    {
        string text = translateTool.GetText(2).ToString();
        dialogueManager.Speak(text, employee.id);
    }

    public void WhenLoaded()
    {
        string text = translateTool.GetText(3).ToString();
        dialogueManager.Speak(text, employee.id);
    }
}
