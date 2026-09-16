using UnityEngine;

public class Extinguisher :  MonoBehaviour, I_Interact
{
    [SerializeField] private bool on = false;

    [SerializeField] private AudioSource audioS = null;
    [SerializeField] private AudioClip sound = null;
    [SerializeField] private ParticleSystem smokeVFX = null;

    private void Start()
    {
        if (on)
        {
            smokeVFX.Play();
            if (sound)
            {
                audioS.clip = sound;
                audioS.Play();
            }
        }
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            on = !on;

            if (on)
            {
                smokeVFX.Play();
                if (sound)
                {
                    audioS.clip = sound;
                    audioS.Play();
                }
            }
            else
            {
                smokeVFX.Stop();
                audioS.Stop();
            }
        }
    }

    public void Release()
    {

    }
}
