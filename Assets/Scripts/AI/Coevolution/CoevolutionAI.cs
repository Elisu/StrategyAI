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

    TryAction[] possibleActions;

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


        possibleActions[indexOfBest].Invoke(attacker, out resultAction);
        return resultAction;
    }


    protected override void RunOver(GameStats stats)
    {
        List<Statistics> towerStats = new List<Statistics>();
        List<Statistics> meleeStats = new List<Statistics>();
        List<Statistics> rangedStats = new List<Statistics>();

        towers.SetFtiness(stats, role);
        meleeUnits.SetFtiness(stats, role);
        rangedUnits.SetFtiness(stats, role);        
            
    }

    protected override int PickToBuy()
    {
        throw new System.NotImplementedException();
    }
}
