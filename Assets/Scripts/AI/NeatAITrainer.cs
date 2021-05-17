using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeatAITrainer : AITrainer
{
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

}
