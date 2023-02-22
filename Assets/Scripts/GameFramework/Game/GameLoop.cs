using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


internal class GameLoop : Loop
{
    public bool DEBUG = false;
    public GameInstance game;
    public IPlayerController attacker;
    public IPlayerController defender;

    public Text winnerText;
    public Text moneyText;
    public UnitButtonLoader loader;

    [SerializeField]
    protected string defenderSave;
    [SerializeField]
    protected string attackerSave;

    private void Start()
    {
        if (!DEBUG)
        {
            var setting = FindObjectOfType<Settings>();

            if (setting != null)
            {
                attacker = setting.selectedAttacker;
                defender = setting.selectedDefender;
                attackerSave = setting.attackerChampion;
                defenderSave = setting.defenderChampion;

                Destroy(setting.gameObject);
            }
        }

        if (attacker.GetType() == typeof(HumanPlayerController) || defender.GetType() == typeof(HumanPlayerController))
            loader.gameObject.SetActive(true);


        game = Instantiate(game);
        game.SetMap(LoadMap());
        game.GameOver += GameOver;

        attacker = Instantiate(attacker);
        defender = Instantiate(defender);

        IPlayer playerDefender = defender.Load(defenderSave);
        IPlayer playerAttacker = attacker.Load(attackerSave);

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
