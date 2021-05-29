using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization;


public class BasicAI : AIPlayer
{
    const int lowUnitTresholt = 1;
    int tobuy = 0;

    public override AIPlayer Clone()
    {
        return new BasicAI();
    }

    protected override IAction FindAction(Attacker attacker)
    {
        IAction resultAction = null;

        if (attacker.CurrentState == State.Free)
        {
            if (!MacroActions.AttackInRange(attacker, out resultAction))
            {
                TroopBase enemy = Info.EnemyArmy.SenseLowestHealth();
                if (enemy != null && enemy.Health < attacker.Health)
                    MacroActions.AttackGiven(enemy, attacker, out resultAction);
                else
                {
                    enemy = Info.EnemyArmy.SenseLowestDamage();
                    if (enemy != null && enemy.Damage < attacker.Damage)
                        MacroActions.AttackGiven(enemy, attacker, out resultAction);
                    else
                        MacroActions.AttackClosest(attacker, out resultAction);
                }
            }
        }
        else 
        {
            if (attacker is TroopBase troop)
            {
                if (troop.CurrentState == State.Fighting && troop.Target.Health >= troop.Health * 2)
                    MacroActions.MoveToSafety(troop, out resultAction);                
               
            }
        }

        return resultAction;
    }

    protected override void RunOver(GameStats stats)
    {
        return;
    }

    protected override int PickToBuy()
    {
        if (Info.OwnArmy.Count <= lowUnitTresholt)
            return UnitFinder.LowestPriceIndex;

        if (Side == Role.Defender && !Info.OwnArmy.Troops.Any(x => x.Range > 1))
        {
            for (int i = 0; i < UnitFinder.unitStats.Count; i++)
            {
                if (UnitFinder.unitStats[i].Range > 1 && UnitFinder.unitStats[i].Price <= Info.OwnArmy.Money)
                    return i;

            }
        }

        tobuy = (tobuy + 1) % UnitFinder.unitStats.Count;
        return tobuy;

    }
}
