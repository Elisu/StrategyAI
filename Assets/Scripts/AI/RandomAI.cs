using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : AIPlayer
{
    System.Random rnd = new System.Random();

    delegate bool Action(Attacker attacker, out IAction resultAction);

    Action[] possibleActions;

    public RandomAI()
    {
        possibleActions = new Action[] { MacroActions.AttackClosest, MacroActions.AttackInRange,
                                         MacroActions.AttackWeakestAgainstMe, MacroActions.AttackWithLowestDamage,
                                         MacroActions.AttackWithLowestHealth, MacroActions.DoNothing };
    }

    public override AIPlayer Clone()
    {
        return new RandomAI();
    }

    protected override IAction FindAction(Attacker attacker)
    {
        IAction result = null;

        var changeAction = rnd.Next(0, 11);
        if (attacker.CurrentState == State.Free || changeAction < 4)
            possibleActions[rnd.Next(0, possibleActions.Length)].Invoke(attacker, out result);

        return result;
    }

    protected override int PickToBuy()
    {
        return rnd.Next(0, UnitFinder.UnitStats.Count);
    }

    protected override void RunOver(GameStats stats)
    {
        return;
    }
}
