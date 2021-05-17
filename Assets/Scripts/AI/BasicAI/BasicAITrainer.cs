using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAITrainer : AITrainer
{
    protected override List<AIPlayer> CreatPopulation()
    {
        List<AIPlayer> pop = new List<AIPlayer> { new BasicAI() };

        return pop;
    }

    public override void GenerationDone()
    {
        return;
    }

    public override AIPlayer GetRepresentative()
    {
        return new BasicAI();
    }
}
