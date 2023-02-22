using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedAI : AIController
{
    public override Type AIPlayerType => GetPlayer().GetType();

    private AIPlayer player;

    public void SetPlayer(AIPlayer pl)
    {
        player = pl;
    }

    public override IPlayer GetPlayer() => player;
}
