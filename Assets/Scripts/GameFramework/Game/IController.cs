using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IController<PlayerType> : MonoBehaviour where PlayerType : IPlayer
{
    protected PlayerType player;

    public Army OwnArmy => player.OwnArmy;

    public virtual void Activate(PlayerType player, Instance game)
    {
        this.player = player;
    }
}
