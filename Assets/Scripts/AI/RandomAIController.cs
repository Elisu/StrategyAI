using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIController : AIController
{
    public override Type AIPlayerType => typeof(RandomAI);

    public override IPlayer GetPlayer()
    {
        return new RandomAI();
    }
}
