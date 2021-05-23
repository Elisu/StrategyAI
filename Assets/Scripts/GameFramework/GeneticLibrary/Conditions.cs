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
            public override bool Evaluate(IAttack attacker)
            {
                return false;
            }
        }

        [DataContract]
        public class Damaged : ICondition
        {
            public override bool Evaluate(IAttack attacker)
            {
                if (attacker.ReceivedDamage > 0)
                    return true;

                return false;
            }
        }

        [DataContract]
        public class Free : ICondition
        {
            public override bool Evaluate(IAttack attacker)
            {
                if (attacker.CurrentState == State.Free)
                    return true;

                return false;
            }
        }
    }

    [DataContract]
    [KnownType(typeof(Conditions.Damaged))]
    [KnownType(typeof(Conditions.Free))]
    [KnownType(typeof(Conditions.Strongest))]
    public abstract class ICondition
    {
        public abstract bool Evaluate(IAttack attacker);
    }
}
