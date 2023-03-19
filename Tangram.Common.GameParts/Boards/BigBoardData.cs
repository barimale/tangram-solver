﻿using Genetic.Algorithm.Tangram.GameParts.Blocks;
using Genetic.Algorithm.Tangram.Solver.Domain.Generators;
using Genetic.Algorithm.Tangram.Solver.Domain.Block;
using Genetic.Algorithm.Tangram.Solver.Domain.Board;

namespace Genetic.Algorithm.Tangram.GameParts.Boards.BigBoard
{
    internal class PolishBigBoardData : IGameParts
    {
        private int ScaleFactor = 1;

        private IList<BlockBase> Blocks = new List<BlockBase>()
        {
            DarkBlue.Create(withFieldRestrictions: true),
            Red.Create(withFieldRestrictions: true),
            LightBlue.Create(withFieldRestrictions: true),
            Purple.Create(withFieldRestrictions: true),
            Blue.Create(withFieldRestrictions: true),
            Pink.Create(withFieldRestrictions: true),
            Green.Create(withFieldRestrictions: true),
            LightGreen.Create(withFieldRestrictions: true),
            Orange.Create(withFieldRestrictions: true),
            Yellow.Create(withFieldRestrictions: true)
        };

        private int[] Angles = new int[]
        {
            //-270,
            //-180,
            //-90,
            0,
            90,
            180,
            270
        };

        public GamePartsConfigurator CreateNew(bool withAllowedLocations = false)
        {
            if(withAllowedLocations)
            {
                var modificator = new AllowedLocationsGenerator();
                var preconfiguredBlocks = modificator.Preconfigure(
                    Blocks.ToList(),
                    Board(),
                    Angles);

                return new GamePartsConfigurator(
                    preconfiguredBlocks,
                    Board(),
                    Angles);
            }

            return new GamePartsConfigurator(
                Blocks,
                Board(),
                Angles);
        }

        private BoardShapeBase Board()
        {
            var fieldHeight = 1d;
            var fieldWidth = 1d;
            var boardColumnsCount = 10;
            var boardRowsCount = 5;
            var fields = GamePartsFactory
                .GeneratorFactory
                .RectangularBoardFieldsGenerator
                .GenerateFields(
                    ScaleFactor,
                    fieldHeight,
                    fieldWidth,
                    boardColumnsCount,
                    boardRowsCount,
                    new object[,] {
                        { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                        { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
                        { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                        { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
                        { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 }
                    }
                );

            var boardDefinition = new BoardShapeBase(
                fields,
                boardColumnsCount,
                boardRowsCount,
                ScaleFactor);

            return boardDefinition;
        }
    }
}
