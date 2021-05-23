using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeatAITrainer : AITrainer
{
    public override Type AIPlayerType => typeof(NeatAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        return null;
    }

    public override void GenerationDone()
    {
        throw new System.NotImplementedException();
    }

    public override AIPlayer GetRepresentative()
    {
        throw new System.NotImplementedException();
    }

    public override AIPlayer ToSave()
    {
        throw new System.NotImplementedException();
    }
}
