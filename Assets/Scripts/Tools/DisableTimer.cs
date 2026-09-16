using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    const float timer = 2.0f;
    
    private void OnEnable()
    {
        Invoke(nameof(Disable), timer);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
