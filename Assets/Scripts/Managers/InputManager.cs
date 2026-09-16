using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeyCode escape = KeyCode.Escape;
    [SerializeField] private KeyCode changeCamera = KeyCode.F5;

    void Update()
    {
        if (Input.GetKeyDown(escape))
        {
            if (PC_Manager.Instance.on_PC) 
                PC_Manager.Instance.ReactivatePlayer();
            else
                PauseMenu.Instance.TogglePause();
        }

        if (Input.GetKeyDown(changeCamera))
        {
            if (PC_Manager.Instance.on_PC) 
                PC_Manager.Instance.ChangeCamera();
        }
    }
}
