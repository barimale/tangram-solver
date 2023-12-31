﻿using GeneticSharp;
using Tangram.GameParts.Logic.GameParts.Block;
using Tangram.GameParts.Logic.GameParts.Board;

namespace Solver.Tangram.AlgorithmDefinitions
{
    public interface IAlgorithmSettings
    {
        GeneticAlgorithm CreateNew(BoardShapeBase board, IList<BlockBase> blocks, int[] allowedAngles);
    }
}