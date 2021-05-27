using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


internal class GameLoop : Loop
{
    public GameInstance game;
    public AITrainer attacker;
    public AITrainer defender;
    public Text winnerText;
    public string fileToLoad = null;

    private void Start()
    {
        game = Instantiate(game);
        game.SetMap(LoadMap());
        game.GameOver += GameOver;

        AIPlayer playerDefender = defender.LoadChampion();
        AIPlayer playerAttacker = attacker.LoadChampion();
        game.Run(playerAttacker, playerDefender);
    }

    private void GameOver()
    {
        winnerText.text = game.winner + "Won";
        winnerText.gameObject.SetActive(true);
    }
}
