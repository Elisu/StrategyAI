using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAITrainer : AITrainer
{
    public override Type AIPlayerType => typeof(BasicAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        List<AIPlayer> pop = new List<AIPlayer> { new BasicAI() };

        return pop;
    }

    public override void GenerationDone()
    {
        return;
    }

    public override AIPlayer LoadChampion()
    {
        return new BasicAI();
    }

    public override AIPlayer GetChampion()
    {
        return null;
    }
}
