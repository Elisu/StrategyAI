using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class CoevolutionAI : AIPlayer
{
    public int populationSize;
    public int individualLength;

    Individual towers;
    Individual meleeUnits;
    Individual rangedUnits;

    public CoevolutionAI(Individual tw, Individual melee, Individual ranged)
    {
        towers = tw;
        meleeUnits = melee;
        rangedUnits = ranged;
    }

    private CoevolutionAI DeepCopy()
    {
        Individual tw = new Individual(this.towers);
        Individual melee = new Individual(this.meleeUnits);
        Individual ranged = new Individual(this.rangedUnits);

        return new CoevolutionAI(tw, melee, ranged);
    }

    public override AIPlayer Clone()
    {
        return DeepCopy();
    }

    protected override IAction FindAction(Attacker attacker)
    {
        IAction resultAction = null;

        Individual current;

        if (attacker is TowerBase)
            current = towers;
        else if (attacker is Troop<Archers>)
            current = rangedUnits;
        else
            current = meleeUnits;

        int[] votes = new int[current.PossibleActions];

        foreach (Rule rule in current)
        {
            if (rule.AllTrue(attacker))
                votes[rule.ActionIndex]++;
        }

        int indexOfBest = 0;

        for (int i = 0; i < votes.Length; i++)
        {
            if (votes[indexOfBest] < votes[i])
                indexOfBest = i;
        }


        //current.possibleActions[indexOfBest].Invoke(attacker);
        return null;
    }


    protected override void RunOver()
    {
        List<IRecruitable> dead = OwnArmy.GetDead();

        List<Statistics> statsTowers = new List<Statistics>();
        List<Statistics> statsMelee = new List<Statistics>();
        List<Statistics> statsRanged = new List<Statistics>();

        for (int i = 0; i < dead.Count; i++)
        {
            IRecruitable corpse = dead[i];

            if (corpse is TowerBase)
                statsTowers.Add(corpse.GetStats());
            else if (corpse is Troop<Archers>)
                statsMelee.Add(corpse.GetStats());
            else
                statsRanged.Add(corpse.GetStats());
        }

        towers.Fitness = 0;
        meleeUnits.Fitness = 0;
        rangedUnits.Fitness = 0;

        
            
    }
}
