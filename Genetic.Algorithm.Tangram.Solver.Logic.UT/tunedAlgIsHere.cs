﻿using GeneticSharp;
using NetTopologySuite.Geometries;
using Genetic.Algorithm.Tangram.GameParts.Generators;
using Genetic.Algorithm.Tangram.Solver.Logic.Chromosome;
using Genetic.Algorithm.Tangram.Solver.Domain.Board;
using Genetic.Algorithm.Tangram.Solver.Domain.Block;
using Genetic.Algorithm.Tangram.GameParts.Blocks;
using Genetic.Algorithm.Tangram.Solver.Domain.Generators;
using Genetic.Algorithm.Tangram.Common.Extensions.Extensions;
using Genetic.Algorithm.Tangram.Solver.Logic.Fitness;
using Genetic.Algorithm.Tangram.Solver.Logic.Population;
using Genetic.Algorithm.Tangram.Solver.Logic.Crossovers;
using Genetic.Algorithm.Tangram.Solver.Logic.Mutations;
using Genetic.Algorithm.Tangram.Solver.Logic.OperatorStrategies;
using Genetic.Algorithm.Tangram.GameParts;

namespace Genetic.Algorithm.Tangram.Solver.Logic.UT
{
    public static class MediumBoardData
    {
        public static GamePartsConfigurator DemoData()
        {
            // common
            var scaleFactor = 1;

            // allowed angles
            var angles = new int[]
            {
                -270,
                -180,
                -90,
                0,
                90,
                180,
                270
            };

            // board
            var fieldHeight = 1d;
            var fieldWidth = 1d;
            var boardColumnsCount = 5;
            var boardRowsCount = 4;
            var fields = GamePartsFactory
                .GeneratorsFactory
                .FieldsGenerator
                .GenerateFields(
                    scaleFactor,
                    fieldHeight,
                    fieldWidth,
                    boardColumnsCount,
                    boardRowsCount);

            var boardDefinition = new BoardShapeBase(
                fields,
                boardColumnsCount,
                boardRowsCount,
                scaleFactor);

            // blocks
            var blocks = new List<BlockBase>()
            {
                DarkBlue.Create(),
                LightBlue.Create(),
                Purple.Create(), // a problem with shapes like C, not all allowed locations are generated
                Blue.Create(),
            };

            // calculate allowed locations of blocks
            // ( blocks may be use as well, but the chromosome behaviour
            // is going to be more complex)
            var modificator = new AllowedLocationsGenerator();
            var preconfiguredBlocks = modificator.Preconfigure(
                blocks,
                boardDefinition,
                angles);

            // dynamic initial population size idea
            var dynamicPopulationSize = blocks
                .Select(p => p.AllowedLocations.Length)
                .ToList();

            var multipliedDynamicPopulationSize = 1;
            foreach (var item in dynamicPopulationSize)
            {
                multipliedDynamicPopulationSize = multipliedDynamicPopulationSize * item;
            }

            // solver
            var generationChromosomesNumber = Math.Max(multipliedDynamicPopulationSize, 30000);
            var mutationProbability = 0.2f;
            var crossoverProbability = 1.0f - mutationProbability;

            // use together with the population class
            var chromosome = new TangramChromosome(
                preconfiguredBlocks,
                boardDefinition,
                angles);

            // all combinations of genes as an initial set of chromosomes
            var chromosomes = new List<IChromosome>();

            var allLocations = preconfiguredBlocks
                .Select(p => p.AllowedLocations.ToArray())
                .ToArray();

            var allPermutations = allLocations
                .Permutate();

            var blocksAsArray = blocks.ToArray();
            var preloadFitness = double.MinValue;
            var preloadSolutions = new List<Tuple<double, TangramChromosome>>();
            var tangramFitness = new TangramFitness(boardDefinition, blocks);

            foreach (var permutation in allPermutations)
            {
                var newChromosome = new TangramChromosome(
                    preconfiguredBlocks,
                    boardDefinition,
                    angles);

                foreach (var (gene, index) in permutation.WithIndex())
                {
                    var newBlockAsGene = new BlockBase(
                        gene,
                        blocksAsArray[index].Color,
                        false);

                    newChromosome.ReplaceGene(
                        index,
                        new Gene(newBlockAsGene));
                }

                var newFitness = tangramFitness.Evaluate(newChromosome);
                if (newFitness >= preloadFitness)
                {
                    preloadFitness = newFitness;
                    preloadSolutions.Add(
                        Tuple.Create(
                            newFitness,
                            newChromosome));
                }
                chromosomes.Add(newChromosome);
            }

            var chromosomesAmount = chromosomes.Count;
            var chromosomesWithFitnessBelowFive = preloadSolutions
                .Where(p => p.Item1 > -5f)
                .ToList();

            var onlyCompleteSolutions = preloadSolutions
                .Where(ppp => ppp.Item1 == 0f)
                .ToList();

            var preloadedPopulation = new PreloadedPopulation(
                multipliedDynamicPopulationSize,
                generationChromosomesNumber,
                chromosomes);
            //preloadedPopulation.GenerationStrategy = new TrackingGenerationStrategy();
            //population.CreateInitialGeneration();

            var selection = new RouletteWheelSelection(); // new EliteSelection(generationChromosomesNumber);//RouletteWheelSelection
            var crossover = new TangramCrossover();
            var mutation = new TangramMutation();
            var fitness = new TangramFitness(boardDefinition, blocks);
            var reinsertion = new ElitistReinsertion();
            var operatorStrategy = new VaryRatioOperatorsStrategy(); // DefaultOperatorsStrategy(); //TplOperatorsStrategy
            var termination = new FitnessThresholdTermination(-0.01f); // new FitnessStagnationTermination(100);

            var solverBuilder = AlgorithmSettings.Solver.SolverFactory.CreateNew();
            var solver = solverBuilder
                .WithPopulation(preloadedPopulation)
                .WithReinsertion(reinsertion)
                .WithSelection(selection)
                .WithFitnessFunction(fitness)
                .WithMutation(mutation, mutationProbability)
                .WithCrossover(crossover, crossoverProbability)
                .WithOperatorsStrategy(operatorStrategy)
                .WithTermination(termination)
                .Build();

            var gameConfiguration = new GamePartsConfigurator(
                blocks,
                boardDefinition,
                solver,
                angles);

            var isConfigurationValid = gameConfiguration.Validate();

            if (!isConfigurationValid)
                throw new Exception("The summarized block definitions area cannot be bigger than the board area.");

            return gameConfiguration;
        }
    }
}