
using UnityEngine;
using DG.Tweening;

public class EgameAwardsInvitation : MonoBehaviour
{
    [SerializeField] private GameObject egameAwards = null;
    [SerializeField] private CanvasGroup fade = null;
    [SerializeField] private float delay = 6f;

    public void AcceptInvite()
    {   
        FadeEffect();
    }

    private void FadeEffect()
    {
        fade.DOFade(1f, delay/2).OnComplete(() => 
        {
            fade.DOFade(0f, delay); 
            ActiveAward();
        });
    }

    private void ActiveAward()
    {
        egameAwards.SetActive(true);
    }
}
