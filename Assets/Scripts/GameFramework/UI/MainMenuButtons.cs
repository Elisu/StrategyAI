using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public TrainingDropDownFill attackerFill;
    public TrainingDropDownFill defenderFill;

    public void OnStartTrainingClick()
    {
        TrainingSettings.selectedAttacker = attackerFill.GetSelected();
        TrainingSettings.selectedDefender = defenderFill.GetSelected();

        SceneManager.LoadScene(2);
    }
}
