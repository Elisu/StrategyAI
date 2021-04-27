using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : AIPlayer
{

    protected override void FindAction(Attacker attacker)
    { 
        if (attacker.CurrentState == State.Free)
        {
            if (!MacroActions.AttackInRange(attacker))
            {
                TroopBase enemy = ownTroops.SenseEnemyLowestHealth();
                if (enemy != null && enemy.Health < attacker.Health)
                    MacroActions.AttackGiven(enemy, attacker);
                else
                {
                    enemy = ownTroops.SenseEnemyLowestDamage();
                    if (enemy != null && enemy.Damage < attacker.Damage)
                        MacroActions.AttackGiven(enemy, attacker);
                    else
                        MacroActions.AttackClosest(attacker);
                }
            }
        }
        else 
        {
            if (attacker is TroopBase troop)
            {
                if (troop.CurrentState == State.Fighting && troop.Target.Health >= troop.Health * 2)
                    MacroActions.MoveToSafety(troop);                
               
            }
        }
    }
}
