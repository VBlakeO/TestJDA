using UnityEngine;
using DG.Tweening;

public class EgameAwards : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private GameObject fakePlayer = null;
    [Space]

    [SerializeField] private Animator presenter = null;
    [SerializeField] private Animator[] audience = null;


    [SerializeField] private GameObject balloons = null;

    [Header("Screens")] 
    [SerializeField] private CanvasGroup[] screenGroups = null;
    [SerializeField] private float transitionTime = 2f;
    [SerializeField] private CanvasGroup currentGroup = null;
    [SerializeField] private CanvasGroup fade = null;
    [Space]

    [SerializeField] private PlayerGamePresentation[] playerPresentation = null;
    private int currentId = 0;

    [SerializeField] private AudioSource audioS = null;
    [SerializeField] private AudioClip applauseClip = null;

    public void Start()
    {
        Invoke(nameof(Initialize), 1);
    }

    private void Initialize()
    {
        playerMovement.gameObject.SetActive(false);
        fakePlayer.SetActive(true);

        currentGroup = screenGroups[0];
        ShowScreen();

        foreach (Animator anim in audience)
            anim.speed = 0f;

        foreach (PlayerGamePresentation presentation in playerPresentation)
            presentation.Initialize();
    }

    private void ShowScreen()
    {
        float floatValue = currentGroup.alpha;

        DOTween.To(() => floatValue, x => floatValue = x, 1, transitionTime).OnUpdate(() => {currentGroup.alpha = floatValue;})
        .OnComplete(() => 
        {Invoke(nameof(HideTransition), transitionTime);});
    }

    private void HideTransition()
    {
        if(currentGroup != screenGroups[^1])
        {
            HideScreen();
        }
    }

    private void HideScreen()
    {
        float floatValue = 1f;

        DOTween.To(() => floatValue, x => floatValue = x, 0f, transitionTime).OnUpdate(() => {currentGroup.alpha = floatValue;})
        .OnComplete(() => 
        {
            NextScreen();
        });
    }

    private void NextScreen()
    {
        if (currentGroup == screenGroups[^1])
            return;

        if (currentId < screenGroups.Length - 1)
        {
            currentId++;
            currentGroup = screenGroups[currentId];
            ShowScreen();
        }
        

        if (currentId == screenGroups.Length - 1)
        {
            presenter.Play("GoToApplause");

            ReleaseBalloons();

            foreach (Animator anim in audience )
                anim.speed = 1f;
        }
    }

    private void ReleaseBalloons()
    {
        balloons.SetActive(true);
        audioS.PlayOneShot(applauseClip);

        Invoke(nameof(FadeEffect), 4f);

    }

    private void FadeEffect()
    {
        fade.DOFade(1f, 3);
    }

}
