﻿using Genetic.Algorithm.Tangram.Solver.Logic.GameParts.Blocks;
using Genetic.Algorithm.Tangram.Solver.Logic.GameParts.Board;
using Genetic.Algorithm.Tangram.Solver.Logic.GameParts;
using GeneticSharp;
using Genetic.Algorithm.Tangram.Solver.Logic.Utilities;
using Genetic.Algorithm.Tangram.Solver.Logic.UT.Data.BigBoard.Blocks;

namespace Genetic.Algorithm.Tangram.Solver.Logic.UT.Data.BigBoard
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
            var fields = GamePartsHelper.GenerateFields(
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
                Purple.Create(),
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

            var dynamicPopulationSize = blocks
                .Select(p => p.AllowedLocations.Length)
                .ToList();

            var multipliedDynamicPopulationSize = 1;
            foreach(var item in dynamicPopulationSize)
            {
                multipliedDynamicPopulationSize = multipliedDynamicPopulationSize * item;
            }

            // solver
            var generationChromosomesNumber = Math.Max(multipliedDynamicPopulationSize / blocks.Count, 500);
            var mutationProbability = 0.2f;
            var crossoverProbability = 1.0f - mutationProbability;
            var chromosome = new TangramChromosome(
                preconfiguredBlocks,
                boardDefinition,
                angles);
            var population = new Population(
                generationChromosomesNumber,
                generationChromosomesNumber,
                chromosome);// understand the population parameters
            var selection = new EliteSelection(generationChromosomesNumber);//maybe half or 20% of the defined population, understand the parameter
            var crossover = new TangramCrossover(); //  TangramCrossover
            var mutation = new TangramMutation(); // RouletteWheelSelection
            var fitness = new TangramFitness(boardDefinition, blocks);
            var reinsertion = new ElitistReinsertion();
            var operatorStrategy = new DefaultOperatorsStrategy(); // DefaultOperatorsStrategy(); //TplOperatorsStrategy
            var termination = new FitnessThresholdTermination(-0.01f); // new FitnessStagnationTermination(100);

            var solverBuilder = Factory.Factory.CreateNew();
            var solver = solverBuilder
                .WithPopulation(population)
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

        private static int Silnia(int n)
        {
            int result = 1;
            while(n > 0)
            {
                result = result * n;
                n = n - 1;
            }

            return result;
        }
    }
}
