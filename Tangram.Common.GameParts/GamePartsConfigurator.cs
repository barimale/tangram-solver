﻿using Genetic.Algorithm.Tangram.Solver.Domain.Block;
using Genetic.Algorithm.Tangram.Solver.Domain.Board;
using GeneticSharp;
using NetTopologySuite.Geometries;

namespace Genetic.Algorithm.Tangram.GameParts
{
    public class GamePartsConfigurator
    {
        public BoardShapeBase Board { private set; get; }
        public IList<BlockBase> Blocks { private set; get; }
        public int[] AllowedAngles { get; private set; }
        public GeneticAlgorithm? Algorithm { private set; get; }

        public GamePartsConfigurator(
            IList<BlockBase> blocks,
            BoardShapeBase boardShape,
            int[] allowedAngles)
        {
            Board = boardShape;
            Blocks = blocks;
            AllowedAngles = allowedAngles;
        }

        public GamePartsConfigurator(
            IList<BlockBase> blocks,
            BoardShapeBase boardShape,
            GeneticAlgorithm algorithm,
            int[] allowedAngles)
            : this(blocks, boardShape, allowedAngles)
        {
            Algorithm = algorithm;
        }

        public static string[] LocationAsStringArray(Geometry location)
        {
            var toString = location
                .Coordinates
                .Select(p =>
                    "(" +
                    Math.Round(p.X, 2)
                        .ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    "," +
                    Math.Round(p.Y, 2)
                        .ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    ")").ToArray();
            var toStringAsArray = string.Join(',', toString);

            return new string[1] { toStringAsArray };
        }

        public string[] BoardAsStringArray()
        {
            return Board.BoardPolygonAsStringArray();
        }

        public bool Validate()
        {
            var digits = 3;

            var summarizeBlocksArea = Math.Round(
                    Blocks.ToList().Sum(p => p.Area),
                    digits,
                    MidpointRounding.ToEven);

            var boardArea = Math.Round(
                    Board.Area,
                    digits,
                    MidpointRounding.ToEven);

            return boardArea >= summarizeBlocksArea;
        }
    }
}