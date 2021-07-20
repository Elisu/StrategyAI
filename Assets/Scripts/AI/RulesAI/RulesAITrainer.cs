using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetic;
using System;
using System.IO;
using System.Globalization;

public class RulesAITrainer : AITrainer
{
    public int PopulationSize;
    public int IndividualLength;

    public float crossoverProbability = 0.3f;
    public float mutationProbability = 0.02f;
    public float actionMutationProbability = 0.08f;

    Strategy all;
    ShopperPopulation shoppers;
    IMacroAction[] possibleActions;

    RulesAI champion;
    readonly List<int> fitnessesRecord = new List<int>();
    readonly List<Tuple<int, int, int, int, Role>> accumulatedStats = new List<Tuple<int, int, int, int, Role>>();

    public override Type AIPlayerType => typeof(RulesAI);

    protected override List<AIPlayer> CreatePopulation()
    {
        ICondition[] conditions = new ICondition[12] { new Conditions.Damaged(),
                                                      new Conditions.Free(),
                                                      new Conditions.StrongerThanClosest(),
                                                      new Conditions.ClosestIsTroopBase(),
                                                      new Conditions.ClosestIsBuilding(),
                                                      new Conditions.ClosestIsTower(),
                                                      new Conditions.IsDefender(),
                                                      new Conditions.HealthierThanClosest(),
                                                      new Conditions.IsAlone(),
                                                      new Conditions.IsWinning(),
                                                      new Conditions.IsInsideCastle(),
                                                      new Conditions.IsInTowerRange() };

        possibleActions = new IMacroAction[7] { new SerializableMacroActions.AttackClosest(), 
                                                new SerializableMacroActions.AttackWithLowestHealth(), 
                                                new SerializableMacroActions.AttackWithLowestDamage(), 
                                                new SerializableMacroActions.AttackWeakestAgainstMe(),
                                                new SerializableMacroActions.AttackInRange(),
                                                new SerializableMacroActions.DoNothing(),
                                                new SerializableMacroActions.MoveToSafety() };

        all = new Strategy(PopulationSize, IndividualLength, possibleActions.Length, conditions, crossoverProbability, mutationProbability, actionMutationProbability);

        shoppers = new ShopperPopulation(PopulationSize, crossoverProbability, mutationProbability);

        List<AIPlayer> pop = new List<AIPlayer>();

        for (int i = 0; i < all.PopulationSize; i++)
            pop.Add(new RulesAI(all[i], possibleActions, shoppers[i]));

        return pop;
    }

    public override void GenerationDone()
    {
        foreach (AIPlayer player in population)
        {
            ((RulesAI)player).EvaluateFitness();
            accumulatedStats.AddRange(((RulesAI)player).FitnessStats);
        }

        accumulatedStats.Add(new Tuple<int, int, int, int, Role>(-1, -1, -1, -1, Role.Neutral));

        champion = (RulesAI)FindChampion();

        population.Clear();
 
        all.Evolve(true);
        shoppers.Evolve(true);

        for (int i = 0; i < all.PopulationSize; i++)
            population.Add(new RulesAI(all[i], possibleActions, shoppers[i]));

        fitnessesRecord.Add(champion.Fitness);
        Debug.LogWarning(string.Format("Best fitness: {0}", champion.Fitness));
    }

    private AIPlayer FindChampion()
    {
        AIPlayer best = population[0];

        foreach (AIPlayer player in population)
            if (((RulesAI)player).Fitness > ((RulesAI)best).Fitness)
                best = player;

        return best;
    }
    public override AIPlayer GetChampion() => champion;

    protected override void TrainingFinished()
    {
        using (var stream = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.dataPath), this.name + "-bestRuns-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ffff", CultureInfo.InvariantCulture))))
        {
            foreach (int fitness in fitnessesRecord)
                stream.WriteLine(fitness);
        }

        using (var stream = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.dataPath), this.name + "-accumulatedStats-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ffff", CultureInfo.InvariantCulture))))
        {
            int gen = 1;

            foreach (var stat in accumulatedStats)
            {
                if (stat.Item1 != -1)
                    stream.WriteLine("Gen: {0} - {1} {2} {3} {4} {5}", gen, stat.Item1, stat.Item2, stat.Item3, stat.Item4, stat.Item5);
                else
                    gen++;
            }
        }

        fitnessesRecord.Clear();
        accumulatedStats.Clear();
    }
}
