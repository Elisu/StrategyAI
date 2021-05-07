using System.Collections;
using System.Collections.Generic;
using UnityEngine;


internal class GameLoop : MonoBehaviour
{
    public GameInstance game;
    public AITrainingHandler attacker;
    public AITrainingHandler defender;

    private void Start()
    {
        game = Instantiate(game);
        game.Run(attacker.GetRepresentative(), defender.GetRepresentative());
    }
}
