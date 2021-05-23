using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Genetic
{
    [DataContract]
    public class Rule
    {
        [DataMember]
        ICondition[] conditions;

        [DataMember]
        public int ActionIndex { get; set; }
        [DataMember]
        public int ActionCount { get; protected set; }

        public Rule(int possibleActionsCount, ICondition[] possibleConditions)
        {
            conditions = new ICondition[possibleConditions.Length];
            ActionCount = possibleActionsCount;

            for (int i = 0; i < possibleConditions.Length; i++)
            {
                conditions[i] = possibleConditions[UnityEngine.Random.Range(0, conditions.Length)];
            }

            ActionIndex = UnityEngine.Random.Range(0, possibleActionsCount);
        }

        /// <summary>
        /// Creates a copy 
        /// </summary>
        /// <param name="rule"></param>
        public Rule(Rule rule)
        {
            conditions = new ICondition[rule.conditions.Length];

            for (int i = 0; i < rule.conditions.Length; i++)
                conditions[i] = rule.conditions[i];

            ActionIndex = rule.ActionIndex;
            ActionCount = rule.ActionCount;
        }

        public bool AllTrue(Attacker recruit)
        {
            foreach (ICondition cond in conditions)
                if (!cond.Evaluate(recruit))
                    return false;

            return true;
        }

    }
}

