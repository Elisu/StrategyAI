using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class RulesAITrainer : AITrainingHandler
{
    public int PopulationSize;
    public int IndividualLength;

    StrategyGroup all;

    protected override List<AIPlayer> CreatPopulation()
    {
        TryAction[] actions = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions = new Condition[2] { Conditions.Damaged, Conditions.Free };
        all = new StrategyGroup(PopulationSize, IndividualLength, actions, conditions);

        List<AIPlayer> pop = new List<AIPlayer>();

        for (int i = 0; i < all.PopulationSize; i++)
            pop.Add(new RulesAI(all[i], actions));

        return pop;
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
