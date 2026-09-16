using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance = null;

    [SerializeField] private DialogueText prefab;
    [SerializeField] private int quantidadeInicial = 10;
    [SerializeField] private float speakingTime = 2f;
    [SerializeField] private Transform content = null;
    [Space]

    [SerializeField] private DialogueText[] dialogueText;

    [SerializeField] private Color[] color = new Color[6];
    private int currentDialogue = 0;


    void Start()
    {
        Instance = this;
    }

    public DialogueText GetTextMesh()
    {
        if (currentDialogue < dialogueText.Length)
        {
           DialogueText _dialogueText =  dialogueText[currentDialogue];
           _dialogueText.gameObject.SetActive(true);

            currentDialogue++;
            return _dialogueText;
        }
        else
        {
            currentDialogue = 0;

            DialogueText _dialogueText = dialogueText[currentDialogue];
            _dialogueText.gameObject.SetActive(true);

            currentDialogue++;
            return _dialogueText;
        }
    }

    public void Speak(string speech, int colorId)
    {
        DialogueText text = GetTextMesh();
        text.SetDialogue(speech, speakingTime, color[colorId]);
    }
}
