using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : AIBase
{

    protected override void FindAction(IAttack attacker)
    { 
        if (attacker.CurrentState == State.Free)
        {
            if (!MacroActions.AttackInRange(attacker))
            {
                ITroop enemy = ownTroops.SenseEnemyLowestHealth();
                if (enemy != null && enemy.Health < attacker.Health)
                    MacroActions.AttackGiven(enemy, attacker);
                else if (!MacroActions.AttackWithLowestHealth(attacker))
                    MacroActions.AttackClosest(attacker);
            }
        }
        else 
        {
            if (attacker is ITroop troop)
            {
                if (troop.CurrentState == State.Fighting && troop.Target.Health >= troop.Health * 2)
                    MacroActions.MoveToSafety(troop);
                
            }
        }
    }
}
