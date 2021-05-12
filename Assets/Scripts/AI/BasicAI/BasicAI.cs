using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : AIPlayer
{
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
                TroopBase enemy = OwnArmy.SenseEnemyLowestHealth();
                if (enemy != null && enemy.Health < attacker.Health)
                    MacroActions.AttackGiven(enemy, attacker, out resultAction);
                else
                {
                    enemy = OwnArmy.SenseEnemyLowestDamage();
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
        return UnitFinder.PickOnBudget(OwnArmy.Money);
    }
}
