using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetic;
using System;

public class CoevolutionTrainer : AITrainer
{
    public int PopulationSize;
    public int IndividualLength;

    Strategy towers;
    Strategy meleeUnits;
    Strategy rangedUnits;

    public override Type AIPlayerType => typeof(CoevolutionAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        ICondition damaged = new Conditions.Damaged();
        ICondition free = new Conditions.Free();

        IMacroAction attackClosest = new SerializableMacroActions.AttackClosest();
        IMacroAction attackWithLowestHealth = new SerializableMacroActions.AttackWithLowestHealth();
        IMacroAction attackWithLowestDamage = new SerializableMacroActions.AttackWithLowestDamage();
        IMacroAction attackInRange = new SerializableMacroActions.AttackInRange();
        IMacroAction doNothing = new SerializableMacroActions.DoNothing();

        IMacroAction[] actions = new IMacroAction[4] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth };
        ICondition[] conditions = new ICondition[2] { damaged, free };
        towers = new Strategy(PopulationSize, IndividualLength, actions, conditions);

        //Melee
        IMacroAction[] actions2 = new IMacroAction[5] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing };
        ICondition[] conditions2 = conditions;
        meleeUnits = new Strategy(PopulationSize, IndividualLength, actions2, conditions2);

        //Ranged
        IMacroAction[] actions3 = new IMacroAction[5] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing };
        ICondition[] conditions3 = conditions;
        rangedUnits = new Strategy(PopulationSize, IndividualLength, actions3, conditions3);

        List<AIPlayer> pop = new List<AIPlayer>();

        for (int i = 0; i < towers.PopulationSize; i++)
            for (int j = 0; j < meleeUnits.PopulationSize; j++)
                for (int k = 0; k < rangedUnits.PopulationSize; k++)
                    pop.Add(new CoevolutionAI(towers[i], meleeUnits[j], rangedUnits[k], new List<IMacroAction[]>() { actions, actions2, actions3 }));

        return pop;
    }

    public override void GenerationDone()
    {
        towers.GeneticOperations(Operators.RouletteWheel, Operators.UniformCrossover, Operators.ActionMutation);
        meleeUnits.GeneticOperations(Operators.RouletteWheel, Operators.UniformCrossover, Operators.ActionMutation);
        rangedUnits.GeneticOperations(Operators.RouletteWheel, Operators.UniformCrossover, Operators.ActionMutation);

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
