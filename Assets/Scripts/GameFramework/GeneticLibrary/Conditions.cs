using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Genetic
{
    public class Conditions
    {
        [DataContract]
        public class Strongest : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                foreach (var troop in info.EnemyArmy.Troops)
                    if (troop.Damage > attacker.Damage)
                        return false;

                return true;
            }
        }

        [DataContract]
        public class Damaged : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                if (attacker.ReceivedDamage > 0)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class Free : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                if (attacker.CurrentState == State.Free)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class ClosestIsTroopBase : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                var closest = info.EnemyArmy.SenseClosestTo((Attacker)attacker);

                if (closest is TroopBase)
                    return true;

                return false;
            }
        }

    }

    [DataContract]
    [KnownType(typeof(Conditions.Damaged))]
    [KnownType(typeof(Conditions.Free))]
    [KnownType(typeof(Conditions.Strongest))]
    [KnownType(typeof(Conditions.ClosestIsTroopBase))]
    public abstract class ICondition
    {
        public abstract bool Evaluate(IAttack attacker, GameInfo info);
    }
}
