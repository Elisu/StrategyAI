using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Genetic
{
    public class Conditions
    {
        [DataContract]
        public class StrongerThanClosest : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                var enemy = info.EnemyArmy.SenseClosestTo((Attacker)attacker);

                if (enemy is Attacker enemyAttacker)
                    if (enemyAttacker.Damage > attacker.Damage)
                            return false;

                return true;
            }
        }

        [DataContract]
        public class HealthierThanClosest : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                var enemy = info.EnemyArmy.SenseClosestTo((Attacker)attacker);

                if (enemy.Health > attacker.Health)
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

        [DataContract]
        public class ClosestIsBuilding : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                var closest = info.EnemyArmy.SenseClosestTo((Attacker)attacker);

                if (closest is Building)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class ClosestIsTower : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                var closest = info.EnemyArmy.SenseClosestTo((Attacker)attacker);

                if (closest is TowerBase)
                    return true;

                return false;
            }
        }
        [DataContract]
        public class IsDefender : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                if (attacker.Side == Role.Defender)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class IsAlone : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                if (info.OwnArmy.Troops.Count < 2)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class IsWinning : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                if (info.OwnArmy.Troops.Count > info.EnemyArmy.Troops.Count)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class IsInsideCastle : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                if (info.Map[attacker.Position].Side == Role.Defender)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class IsInTowerRange : ICondition
        {
            public override bool Evaluate(IAttack attacker, GameInfo info)
            {
                IList<TowerBase> towers;

                if (attacker.Side == Role.Defender)
                    towers = info.OwnArmy.Towers;
                else
                    towers = info.EnemyArmy.Towers;

                foreach (var tower in towers)
                    if (Vector2Int.Distance(attacker.Position, tower.Position) <= tower.Range)
                        return true;

                return false;
            }
        }

    }

    [DataContract]
    [KnownType(typeof(Conditions.Damaged))]
    [KnownType(typeof(Conditions.Free))]
    [KnownType(typeof(Conditions.StrongerThanClosest))]
    [KnownType(typeof(Conditions.HealthierThanClosest))]
    [KnownType(typeof(Conditions.ClosestIsTroopBase))]
    [KnownType(typeof(Conditions.ClosestIsBuilding))]
    [KnownType(typeof(Conditions.ClosestIsTower))]
    [KnownType(typeof(Conditions.IsDefender))]
    [KnownType(typeof(Conditions.IsAlone))]
    [KnownType(typeof(Conditions.IsWinning))]
    [KnownType(typeof(Conditions.IsInsideCastle))]
    [KnownType(typeof(Conditions.IsInTowerRange))]
    public abstract class ICondition
    {
        public abstract bool Evaluate(IAttack attacker, GameInfo info);
    }
}
