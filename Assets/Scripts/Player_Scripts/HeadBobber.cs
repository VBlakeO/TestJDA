using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    [SerializeField] private float walkingBobbingSpeed = 11f;
    [SerializeField] private float bobbingAmount = 0.05f;
    [SerializeField] private float idleSpeed = 3f;
    [Space]
    
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private AudioList audioList = null;

    public float sinTimer = 0f;
    private float timer = 0f;
    private float defaultPosY = 0f;

    private bool footstep = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerMovement.cantMove)
            return;

        if (Mathf.Abs(playerMovement.currentDir.x) > 0.1f || Mathf.Abs(playerMovement.currentDir.y) > 0.1f)
        {
            //Player is moving
            timer += Time.deltaTime * walkingBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
            sinTimer = Mathf.Sin(timer);

            if (sinTimer > Mathf.Abs(0.8f) && !footstep)
            {
                PlayFootstepSound();
                footstep = true;
            }
        }
        else
        {
            //Idle
            timer = 0;
            sinTimer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.MoveTowards(transform.localPosition.y, defaultPosY, Time.deltaTime * idleSpeed), transform.localPosition.z);
        }  

        if (sinTimer < Mathf.Abs(0.3f) && footstep)
            footstep = false;
    }

    private void PlayFootstepSound() => audioList.PlayRandonAudioClip();
}