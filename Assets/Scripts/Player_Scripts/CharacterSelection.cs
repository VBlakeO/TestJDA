using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters = null;
    public GameObject selectionHud = null;
    public GameObject gameplayHud = null;
    public Camera m_camera = null;
    public Camera selectionCamera = null;
    public int id = 0;

    public bool ready = false;

    private void Start()
    {
        if (PlayerMovement.Instance)
        {
            PlayerMovement.Instance.cantLook = true;
            PlayerMovement.Instance.cantMove = true;
            PlayerMovement.Instance.cantJump = true;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        id = SavableGameData.selectedCharacter;
        ready = SavableGameData.readySelection;

        if (SavableGameData.readySelection)
        {
            StartGame();
            SelectCharacter();
        }
    }

    public void NextCharacter()
    {
        SavableGameData.selectedCharacter = LimitMath.LimitValue(SavableGameData.selectedCharacter += 1, 0, 2);

        DisableAllCharacters();
        characters[SavableGameData.selectedCharacter].SetActive(true);

        id = SavableGameData.selectedCharacter;
        ready = SavableGameData.readySelection;
    }

    public void PreviousCharacter()
    {
        SavableGameData.selectedCharacter = LimitMath.LimitValue(SavableGameData.selectedCharacter -= 1, 0, 2);

        DisableAllCharacters();
        characters[SavableGameData.selectedCharacter].SetActive(true);

        id = SavableGameData.selectedCharacter;
        ready = SavableGameData.readySelection;
    }

    private void SelectCharacter()
    {
        DisableAllCharacters();
        characters[SavableGameData.selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        m_camera.enabled = true;
        selectionCamera.enabled = false;
        selectionCamera.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement.Instance.cantLook = false;
        PlayerMovement.Instance.cantMove = false;
        PlayerMovement.Instance.cantJump = false;

        selectionHud.SetActive(false);
        gameplayHud.SetActive(true);

        SavableGameData.readySelection = true;
    }

    private void DisableAllCharacters()
    {
        foreach (GameObject character in characters)
            character.SetActive(false);
    }
}
