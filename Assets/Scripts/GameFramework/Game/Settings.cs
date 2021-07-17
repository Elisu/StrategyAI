using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Settings : MonoBehaviour
{
    public IPlayerController selectedAttacker;
    public IPlayerController selectedDefender;

    public HumanPlayerController human;

    public string attackerChampion;
    public string defenderChampion;

    public TrainingSettings attackerSetting;
    public TrainingSettings defenderSetting;

    public void OnStartClick()
    {
        DontDestroyOnLoad(this);

        if (attackerSetting.myToggle.isOn)
            selectedAttacker = human;
        else
            selectedAttacker = attackerSetting.GetSelected();

        if (defenderSetting.myToggle.isOn)
            selectedDefender = human;
        else
            selectedDefender = defenderSetting.GetSelected();

        attackerChampion = attackerSetting.GetChampion();
        defenderChampion = defenderSetting.GetChampion();

        SceneManager.LoadScene(1);
    }

}
