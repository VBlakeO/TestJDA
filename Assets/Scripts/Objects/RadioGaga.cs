using UnityEngine;

public class RadioGaga : MonoBehaviour, I_Interact
{
    public static RadioGaga Instance = null;

    [SerializeField] private bool on = false;

    [SerializeField] private Animator anim = null;
    [SerializeField] private AudioSource audioS = null;
    [SerializeField] private AudioClip music = null;
    [SerializeField] private GameObject notesVFX = null;
    [SerializeField] private GameObject beatsVFX = null;

    private void Start() 
    {
        Instance = this;

        if (!on)
            return;

        anim.SetBool("Radio", on);

        notesVFX.SetActive(on);
        beatsVFX.SetActive(on);

        if (on)
            audioS.PlayOneShot(music);
        else
            audioS.Stop();
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Connect(on = !on);
    }

    public void Connect(bool _on)
    {
        anim.SetBool("Radio", _on);

        notesVFX.SetActive(_on);
        beatsVFX.SetActive(_on);

        if (_on)
            audioS.PlayOneShot(music);
        else
            audioS.Stop();
    }

    public void Release()
    {
        throw new System.NotImplementedException();
    }
}
