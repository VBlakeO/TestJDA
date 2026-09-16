using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueText : MonoBehaviour
{   
    [HideInInspector] public DialogueManager dialogueManager = null;
    public TextMeshProUGUI textMesh = null;
    
    private float speakingTime = 2f;

    public void SetDialogue(string text, float duration, Color color)
    {   
        textMesh.text = text;
        textMesh.color = color;
        speakingTime = duration;
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(speakingTime);
        textMesh.gameObject.SetActive(false);
    }
}
