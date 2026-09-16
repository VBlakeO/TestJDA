using UnityEngine;

public class RadioFreed : MonoBehaviour
{
    [SerializeField] private Employee_AI freed = null;

    private void OnEnable() => freed.OnDance += PlayRadio;

    private void PlayRadio(bool on) => RadioGaga.Instance.Connect(on);

    private void OnDisable() => freed.OnDance -= PlayRadio;
}
