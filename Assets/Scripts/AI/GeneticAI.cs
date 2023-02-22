//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using UnityEngine;

//public class Genetic
//{
//    public delegate bool TryAction(Attacker recruit, out IAction action);

//    public delegate Individual[] Selection(Individual[] pop);
//    public delegate Individual MutateFunc(Individual ind);
//    public delegate Tuple<Individual, Individual> CrossFunc(Individual a, Individual b);

//    public delegate int FitnessCalculation(GameStats stats, Role role);

//    public static Individual[] CreatePopulation(int popSize, int indSize, int possibleActionsCount, ICondition[] conditions)
//    {
//        Individual[] population = new Individual[popSize];

//        for (int i = 0; i < popSize; i++)
//            population[i] = new Individual(indSize, possibleActionsCount, conditions);

//        return population;
//    }
    
//    [DataContract]
//    public class Individual : IEnumerable<Rule>
//    {
//        public int Fitness { get; set; }
//        public int Length => rules.Length;

//        [DataMember]
//        public int PossibleActions { get; private set; }

//        [DataMember]
//        Rule[] rules;

//        FitnessCalculation customFitness;

//        public Individual(int indLength, int possibleActionCount, ICondition[] conditions, FitnessCalculation fitnessMath = null)
//        {
//            customFitness = fitnessMath;
//            rules = new Rule[indLength];
//            PossibleActions = possibleActionCount;

//            for (int i = 0; i < indLength; i++)
//            {
//                rules[i] = new Rule(possibleActionCount, conditions);
//            }
//        }

//        /// <summary>
//        /// Copy contructor for individual
//        /// </summary>
//        /// <param name="ind"></param>
//        public Individual(Individual ind)
//        {
//            rules = new Rule[ind.Length];
//            PossibleActions = ind.PossibleActions;
//            customFitness = ind.customFitness;

//            for (int i = 0; i < ind.Length; i++)
//                rules[i] = new Rule(ind.rules[i]);
//        }

//        public Individual(List<Rule> rules)
//        {
//            this.rules = rules.ToArray();
//            PossibleActions = rules[0].ActionCount;
//        }

//        public IEnumerator<Rule> GetEnumerator()
//        {
//            foreach (Rule rule in rules)
//            {
//                yield return rule;
//            }
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        public Rule this[int index] => rules[index];

//        public void SetFtiness(GameStats stats, Role role)
//        {
//            if (customFitness != null)
//                Fitness = customFitness.Invoke(stats, role);
//            else
//                Fitness = GetFitness(stats, role);

//            //Debug.Log(string.Format("Fitness: {0}", Fitness));
//        }

//        private int GetFitness(GameStats stats, Role role)
//        {
//            int fitnessResult = 0;

//            List<Statistics> ownStats = stats.GetMyStats(role);

//            if (stats.Winner == role)
//                fitnessResult += 15000;

//            foreach (Statistics stat in ownStats)
//                fitnessResult += stat.dealtDamage + stat.destroyedBuildings * 100 + stat.killedEnemies * 1000;

//            return fitnessResult;
//        }
//    }

//    [DataContract]
//    public class Rule
//    {
//        [DataMember]
//        ICondition[] conditions;
//        [DataMember]
//        public int ActionIndex { get; set; }
//        [DataMember]
//        public int ActionCount { get; protected set; }

//        public Rule(int possibleActionsCount, ICondition[] possibleConditions)
//        {
//            conditions = new ICondition[possibleConditions.Length];
//            ActionCount = possibleActionsCount;

//            for (int i = 0; i < possibleConditions.Length; i++)
//            {
//                conditions[i] = possibleConditions[UnityEngine.Random.Range(0, conditions.Length)];
//            }

//            ActionIndex = UnityEngine.Random.Range(0, possibleActionsCount);
//        }

//        /// <summary>
//        /// Creates a copy 
//        /// </summary>
//        /// <param name="rule"></param>
//        public Rule(Rule rule)
//        {
//            conditions = new ICondition[rule.conditions.Length];

//            for (int i = 0; i < rule.conditions.Length; i++)
//                conditions[i] = rule.conditions[i];

//            ActionIndex = rule.ActionIndex;
//            ActionCount = rule.ActionCount;
//        }

//        public bool AllTrue(Attacker recruit)
//        {
//            foreach (ICondition cond in conditions)
//                if (!cond.Evaluate(recruit))
//                    return false;

//            return true;
//        }

//    }

  
//    public class Conditions
//    {
//        [DataContract]
//        public class Strongest : ICondition
//        {
//            public override bool Evaluate(IAttack attacker)
//            {
//                throw new NotImplementedException();
//            }
//        }

//        [DataContract]
//        public class Damaged : ICondition
//        {
//            public override bool Evaluate(IAttack attacker)
//            {
//                if(attacker.ReceivedDamage > 0)
//                return true;

//                return false;
//            }
//        }

//        [DataContract]
//        public class Free : ICondition
//        {
//            public override bool Evaluate(IAttack attacker)
//            {
//                if (attacker.CurrentState == State.Free)
//                    return true;

//                return false;
//            }
//        }
//    }

//    [DataContract]
//    [KnownType(typeof(Conditions.Damaged))]
//    [KnownType(typeof(Conditions.Free))]
//    [KnownType(typeof(Conditions.Strongest))]
//    public abstract class ICondition
//    {
//        public abstract bool Evaluate(IAttack attacker);
//    }

//    public class Strategy
//    {
//        public int IndividualLength { get; protected set; }
//        public int PopulationSize => population.Length;

//        public int GenerationRunCount => population.Length * 1;

//        public TryAction[] possibleActions;
//        public ICondition[] usedConditions;
//        public Individual[] population;

//        public Strategy(int popSize, int indLength, TryAction[] actions, ICondition[] conditions)
//        {
//            possibleActions = actions;
//            usedConditions = conditions;
//            population = CreatePopulation(popSize, indLength, actions.Length, conditions);
//        }

//        public Individual this[int index]
//        {
//            get => population[index];
//        }

//        public void GeneticOperations(Selection selection, CrossFunc cross, MutateFunc mutate )
//        {
//            //Debug.LogWarning("GENETIC");
//            Individual[] selected = selection(population);

//            if (selected == null)
//                return;

//            selected = Crossover(selected, cross);
//            selected = Mutation(selected, mutate);
//            population = selected;
//        }

//        private Individual[] Crossover(Individual[] pop, CrossFunc cross)
//        {
//            Individual[] crossed = new Individual[pop.Length];

//            for (int i = 1; i < pop.Length; i += 2)
//            {
//                if (UnityEngine.Random.value < 0.4)
//                {
//                    Tuple<Individual, Individual> offs = cross(pop[i - 1], pop[i]);
//                    crossed[i - 1] = offs.Item1;
//                    crossed[i] = offs.Item2;
//                }
//                else
//                {
//                    crossed[i - 1] = pop[i - 1];
//                    crossed[i] = pop[i];
//                }

//            }

//            //If length not even add last individual
//            if (pop.Length % 2 == 1)
//                crossed[pop.Length - 1] = pop[pop.Length - 1];

//            return crossed;
//        }

//        private Individual[] Mutation(Individual[] pop, MutateFunc mutate)
//        {
//            List<Individual> mutated = new List<Individual>();

//            foreach (Individual ind in pop)
//            {
//                if (UnityEngine.Random.value < 0.25)
//                    mutated.Add(mutate(ind));
//                else
//                    mutated.Add(new Individual(ind));
//            }

//            return mutated.ToArray();
//        }

//    }

//    public static Individual[] RouletteWheel(Individual[] pop)
//    {
//        Individual[] selected = new Individual[pop.Length];
//        double fitnessSum = 0;
//        double[] fitnesses = new double[pop.Length];

//        for (int i = 0; i < pop.Length; i++)
//            fitnessSum += pop[i].Fitness;

//        if (fitnessSum == 0)
//            return null;

//        for (int i = 0; i < pop.Length; i++)
//            fitnesses[i] = pop[i].Fitness / fitnessSum;

//        for (int i = 0; i < pop.Length; i++)
//        {
//            double ball = UnityEngine.Random.value;
//            double sum = 0;

//            for (int j = 0; j < fitnesses.Length; j++)
//            {
//                sum += fitnesses[j];

//                if (sum > ball)
//                {
//                    selected[i] = new Individual(pop[j]);
//                    break;
//                }
//            }

//        }

//        return selected;
//    }

//    public static Individual ActionMutation(Individual ind)
//    {
//        Individual mutated = new Individual(ind);

//        foreach (Rule rule in mutated)
//        {
//            if (UnityEngine.Random.value < 0.5 )
//                rule.ActionIndex = UnityEngine.Random.Range(0, rule.ActionCount);
//        }

//        return mutated;
//    }

//    public static Tuple<Individual, Individual> UniformCrossover (Individual a, Individual b)
//    {
//        List<Rule> first = new List<Rule>();
//        List<Rule> second = new List<Rule>();

//        for (int i = 0; i < Mathf.Min(a.Length, b.Length); i++)
//        {
//            if (UnityEngine.Random.value > 0.5)
//            {
//                first.Add(new Rule(a[i]));
//                second.Add(new Rule(b[i]));
//            }
//            else
//            {
//                first.Add(new Rule(b[i]));
//                second.Add(new Rule(a[i]));
//            }
//        }

//        int end;
//        Individual rest;

//        if (a.Length < b.Length)
//        {
//            end = a.Length;
//            rest = b;
//        }
//        else
//        {
//            end = b.Length;
//            rest = a;
//        }

//        for (int i = end; i < rest.Length; i++)
//        {
//            if (UnityEngine.Random.value > 0.5)
//                first.Add(new Rule(rest[i]));            
//            else
//                second.Add(new Rule(rest[i]));
//        }
            
//        return new Tuple<Individual, Individual>(new Individual(first), new Individual(second));
//    }


//}
