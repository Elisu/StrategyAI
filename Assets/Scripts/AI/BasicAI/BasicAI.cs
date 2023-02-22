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
                //Wait for reinforcements
                if (Info.OwnArmy.Troops.Count < 2 )
                {
                    if (attacker.Side == Role.Defender)
                    {
                        if (attacker.Range > 1 && Info.Map[attacker.Position].Side == Role.Defender)
                            MacroActions.DoNothing(attacker, out resultAction);
                        else 
                            MacroActions.MoveToSafety(attacker, out resultAction);

                        return resultAction;
                    }                   
                }

                IRecruitable enemy = Info.EnemyArmy.SenseLowestDamageDecrease(attacker);
                if (!MacroActions.AttackGiven(enemy, attacker, out resultAction))
                {
                    enemy = Info.EnemyArmy.SenseLowestHealth();
                    if (enemy != null && enemy.Health < attacker.Health)
                        MacroActions.AttackGiven(enemy, attacker, out resultAction);

                    if (resultAction == null)
                    {
                        TroopBase troop = Info.EnemyArmy.SenseTroopLowestDamage();
                        if (troop != null && troop.Damage < attacker.Damage)
                            MacroActions.AttackGiven(enemy, attacker, out resultAction);

                    }

                }

            }
        }
        else
        {
            var closest = Info.EnemyArmy.SenseClosestTo(attacker);

            if (closest == attacker.Target)
            {
                MacroActions.DoNothing(attacker, out resultAction);
                return resultAction;
            }

            if (closest is Building)
            {
                if (Info.EnemyArmy.Towers.Count > 0)
                    MacroActions.AttackGiven(Info.EnemyArmy.Towers[0], attacker, out resultAction);
                else
                    MacroActions.AttackGiven(Info.EnemyArmy.SenseLowestDamageDecrease(attacker), attacker, out resultAction);

                if (resultAction != null)
                    return resultAction;
            }

            if (attacker is TroopBase troop)
            {
                if (troop.CurrentState == State.Fighting && troop.Target.Health >= troop.Health * 2 && Info.OwnArmy.Troops.Count < 2)
                    MacroActions.MoveToSafety(troop, out resultAction);
            }
            
        }

        if (resultAction == null)
            MacroActions.AttackClosest(attacker, out resultAction);

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

        if (Info.OwnArmy.Troops.Count >= Info.EnemyArmy.Count * 1.5)
            return -1;

        tobuy = (tobuy + 1) % UnitFinder.UnitStats.Count;
        return tobuy;

    }
}
