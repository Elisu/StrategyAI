using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public TrainingSettings attackerFill;
    public TrainingSettings defenderFill;

    public void OnStartTrainingClick()
    {
        Settings.selectedAttacker = attackerFill.GetSelected();
        Settings.selectedDefender = defenderFill.GetSelected();

        SceneManager.LoadScene(2);
    }
}
