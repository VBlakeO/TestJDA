using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameBot : MonoBehaviour
{
    public Dev_CreationTab devCreationTab = null;
    public Programming programming = null;
    public NewDevOp devOp = null;
    
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
            CreateNewGame();
    }

    void CreateNewGame()
    {
        programming.OnCreateProject += ConcludPrograming;

        devCreationTab.SetGameName("newGame");
        devOp.CompleteProjectPlanning();
    }

    void ConcludPrograming()
    {
        programming.EndProgramming();
        programming.OnCreateProject -= ConcludPrograming;
    }


}
