using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : AIBase
{

    protected override void FindAction()
    {
        for (int i = 0; i < ownTroops.Count; i++)
        {
            IRecruitable recruit = (IRecruitable)ownTroops[i]; 

            if (recruit.CurrentState == State.Free)
            {
                if (!MacroActions.AttackInRange(ownTroops[i]))
                {
                    ITroop enemy = enemyTroops.GetTroopHighestHealth();
                    if (enemy != null && enemy.Health < recruit.Health)
                        MacroActions.AttackGiven(enemy, ownTroops[i]);
                    else
                        MacroActions.AttackWithLowestHealth(ownTroops[i]);
                }
            }
            else 
            {
                if (ownTroops[i] is ITroop troop)
                {
                    if (troop.CurrentState == State.Fighting && troop.Target.Health >= troop.Health * 2)
                        MacroActions.MoveToSafety(troop);
                    
                }
            }
        }

    }
}
