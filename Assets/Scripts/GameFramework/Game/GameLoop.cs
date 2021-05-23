using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


internal class GameLoop : Loop
{
    public GameInstance game;
    public AITrainer humanPlayer;
    public AITrainer aiPlayer;
    public Text winnerText;
    public string fileToLoad = null;

    private void Start()
    {
        game = Instantiate(game);
        game.SetMap(LoadMap());
        game.GameOver += GameOver;

        AIPlayer player;
        if (string.IsNullOrEmpty(fileToLoad))
            player = aiPlayer.GetRepresentative();
        else
            player = LoadPlayerFromFile(fileToLoad, aiPlayer.AIPlayerType);


        game.Run(player, humanPlayer.GetRepresentative());
    }

    private void GameOver()
    {
        winnerText.text = game.winner + "Won";
        winnerText.gameObject.SetActive(true);
    }
}
