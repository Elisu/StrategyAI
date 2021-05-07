using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class CoevolutionTrainer : AITrainingHandler
{
    public int PopulationSize;
    public int IndividualLength;

    StrategyGroup towers;
    StrategyGroup meleeUnits;
    StrategyGroup rangedUnits;

    protected override List<AIPlayer> CreatPopulation()
    {
        TryAction[] actions = new TryAction[4] { MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions = new Condition[2] { Conditions.Damaged, Conditions.Free };
        towers = new StrategyGroup(PopulationSize, IndividualLength, actions, conditions);

        //Melee
        TryAction[] actions2 = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions2 = new Condition[2] { Conditions.Damaged, Conditions.Free };
        meleeUnits = new StrategyGroup(PopulationSize, IndividualLength, actions2, conditions2);

        //Ranged
        TryAction[] actions3 = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions3 = new Condition[2] { Conditions.Damaged, Conditions.Free };
        rangedUnits = new StrategyGroup(PopulationSize, IndividualLength, actions3, conditions3);

        List<AIPlayer> pop = new List<AIPlayer>();

        for (int i = 0; i < towers.PopulationSize; i++)
            for (int j = 0; j < meleeUnits.PopulationSize; j++)
                for (int k = 0; k < rangedUnits.PopulationSize; k++)
                    pop.Add(new CoevolutionAI(towers[i], meleeUnits[j], rangedUnits[k]));

        return pop;
    }

    public override void GenerationDone()
    {
        towers.GeneticOperations(RouletteWheel, UniformCrossover, ActionMutation);
        meleeUnits.GeneticOperations(RouletteWheel, UniformCrossover, ActionMutation);
        rangedUnits.GeneticOperations(RouletteWheel, UniformCrossover, ActionMutation);
        
    }

    public override AIPlayer GetRepresentative()
    {
        throw new System.NotImplementedException();
    }
}
