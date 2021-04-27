using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public GameInstance game;
    public IPlayer attacker;
    public IPlayer defender;

    private void Start()
    {
        game = Instantiate(game);
        game.Run(attacker, defender);
    }
}
