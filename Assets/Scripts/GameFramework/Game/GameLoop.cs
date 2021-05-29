using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


internal class GameLoop : Loop
{
    public GameInstance game;
    public IPlayerController attacker;
    public IPlayerController defender;
    public Text winnerText;
    public Text moneyText;
    public string fileToLoad = null;

    private void Start()
    {
        game = Instantiate(game);
        game.SetMap(LoadMap());
        game.GameOver += GameOver;

        attacker = Instantiate(attacker);
        defender = Instantiate(defender);

        IPlayer playerDefender = defender.LoadChampion();
        IPlayer playerAttacker = attacker.LoadChampion();

        game.SetPlayers(playerAttacker, playerDefender);

        if (attacker is HumanPlayerController humanAttacker)
            humanAttacker.SetMap(game.Map, moneyText);
        else if (defender is HumanPlayerController humanDefender)
            humanDefender.SetMap(game.Map, moneyText);

        game.Run();
    }

    private void GameOver()
    {
        winnerText.text = game.winner + "Won";
        winnerText.gameObject.SetActive(true);
    }
}
