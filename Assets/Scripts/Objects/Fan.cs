using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class Fan : MonoBehaviour, I_Interact
{
    [SerializeField] private bool on = true;
    [SerializeField] private GameObject wind = null;
    [SerializeField] private AudioSource audioSource = null;
    [Space]

    [SerializeField] private AudioClip fanClip = null;
    [SerializeField] private AudioClip buttonClip = null;
    [Space]

    [SerializeField] private AnimationCurve animationCurve = null;

    private float value = 1f;
    private float lastValue = 0f;
    private Animator anim = null;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        if (on)
            audioSource.Play();
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            on = !on;
            lastValue =  value == 0f ? 1f : 0f;
            wind.SetActive(on);


            audioSource.PlayOneShot(buttonClip);
           
            if (on)
            {
                audioSource.clip = fanClip;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
                audioSource.PlayOneShot(buttonClip);
            }
        }
    }

    private void FixedUpdate()
    {
        if (value == lastValue)
            return;

        lastValue = value;

        if (on && value < 1)
            value += Time.deltaTime;
        else if (!on && value > 0)
            value -= Time.deltaTime;

        value = Mathf.Clamp01(value);

        anim.speed = animationCurve.Evaluate(value);
    }

    public void Release()
    {}
}
