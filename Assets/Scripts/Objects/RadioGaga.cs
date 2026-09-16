using UnityEngine;

public class RadioGaga : MonoBehaviour, I_Interact
{
    private const string RadioAnimParam = "Radio";

    public static RadioGaga Instance = null;

    [SerializeField] private bool on = false;
    [SerializeField] private Animator anim = null;
    [SerializeField] private AudioSource audioS = null;
    [SerializeField] private AudioClip music = null;
    [SerializeField] private GameObject notesVFX = null;
    [SerializeField] private GameObject beatsVFX = null;

    // Set before any Start so other scripts never find a null radio
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (on)
            ApplyState();
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Connect(!on);
    }

    // The dancing employee also drives the radio, so the state must be stored here to keep the player toggle in sync
    public void Connect(bool _on)
    {
        if (on == _on)
            return;

        on = _on;
        ApplyState();
    }

    private void ApplyState()
    {
        anim.SetBool(RadioAnimParam, on);
        notesVFX.SetActive(on);
        beatsVFX.SetActive(on);

        if (on)
            audioS.PlayOneShot(music);
        else
            audioS.Stop();
    }

    // The radio is fixed in the office, so there is nothing to release
    public void Release()
    {
    }
}