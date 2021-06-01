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

                IRecruitable enemy = Info.EnemyArmy.SenseLowestDefenseAgainst(attacker);
                if (!MacroActions.AttackGiven(enemy, attacker, out resultAction))
                {
                    enemy = Info.EnemyArmy.SenseLowestHealth();
                    if (enemy != null && enemy.Health < attacker.Health)
                        if (!MacroActions.AttackGiven(enemy, attacker, out resultAction))
                        {
                            TroopBase troop = Info.EnemyArmy.SenseTroopLowestDamage();
                            if (troop != null && troop.Damage < attacker.Damage)
                                if (!MacroActions.AttackGiven(enemy, attacker, out resultAction))
                                    MacroActions.AttackClosest(attacker, out resultAction);
                        }
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
        if (Info.OwnArmy.Troops.Count <= lowUnitTresholt)
            return UnitFinder.LowestPriceIndex;

        if (Side == Role.Defender && !Info.OwnArmy.Troops.Any(x => x.Range > 1))
        {
            for (int i = 0; i < UnitFinder.UnitStats.Count; i++)
            {
                if (UnitFinder.UnitStats[i].Range > 1 && UnitFinder.UnitStats[i].Price <= Info.OwnArmy.Money)
                    return i;

            }
        }

        tobuy = (tobuy + 1) % UnitFinder.UnitStats.Count;
        return tobuy;

    }
}
