using UnityEngine;

public class Console : MonoBehaviour
{
    bool cursor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
           GameManager.m_Instance.SwitchLanguage();
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {

        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            cursor = !cursor;

            if (cursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
