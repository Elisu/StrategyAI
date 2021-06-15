using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAIController : AIController
{
    public override Type AIPlayerType => typeof(BasicAI);    

    public override IPlayer GetPlayer()
    {
        return new BasicAI();
    }
}
