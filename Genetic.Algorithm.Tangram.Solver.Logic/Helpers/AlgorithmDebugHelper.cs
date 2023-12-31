﻿using Genetic.Algorithm.Tangram.Solver.Logic.Chromosome;
using GeneticSharp;

namespace Genetic.Algorithm.Tangram.Solver.Logic.Helpers
{
    public class AlgorithmDebugHelper
    {
        public AlgorithmDebugHelper()
        {
            LatestFitness = double.MinValue;
        }

        public double LatestFitness { private set; get; }

        public void Algorithm_Ran(object? sender, EventArgs e)
        {
            var algorithmResult = sender as GeneticAlgorithm;

            if (algorithmResult == null)
                return;

            var bestChromosome = algorithmResult
                .BestChromosome as TangramChromosome;

            if (bestChromosome == null || !bestChromosome.Fitness.HasValue)
                return;

            var bestFitness = bestChromosome
                .Fitness
                .Value;

            if (bestFitness >= LatestFitness)
            {
                LatestFitness = bestFitness;
                ConsoleHelper.ShowChromosome(bestChromosome);
            }
        }

        public void Algorithm_TerminationReached(object? sender, EventArgs e)
        {
            var algorithmResult = sender as GeneticAlgorithm;

            if (algorithmResult != null)
            {
                var bestChromosome = algorithmResult
                    .BestChromosome as TangramChromosome;

                ConsoleHelper.ShowChromosome(bestChromosome);
            }
        }
    }
}
