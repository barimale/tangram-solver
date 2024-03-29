﻿using NetTopologySuite.Geometries;
using System.Drawing;
using Tangram.GameParts.Elements.Elements.Blocks.CommonSettings;
using Tangram.GameParts.Logic.GameParts.Block;

namespace Tangram.GameParts.Elements.Elements.Blocks.PuzzlerPro
{
    public sealed class Yellow : PuzzleProBaseBlock
    {
        public Yellow()
        {
            fieldRestriction1side = new object[,] {
                { NA,  "X"},
                { "X", "X"},
                { NA, "X" },
                {NA, "X"}
            };

            fieldRestriction2side = new object[,] {
                {  "X", NA},
                {  "X", "X"},
                { "X", NA },
                { "X", NA}
            };

            color = Color.Yellow;

            polygon = new GeometryFactory()
                    .CreatePolygon(new Coordinate[] {
                        new Coordinate(1.00,1.00),
                        new Coordinate(1.00,0.00),
                        new Coordinate(2.00,0.00),
                        new Coordinate(2.00,4.00),
                        new Coordinate(1.00,4.00),
                        new Coordinate(1.00,2.00),
                        new Coordinate(0.00,2.00),
                        new Coordinate(0.00,1.00),
                        new Coordinate(1.00,1.00)
                    });
        }

        public static BlockBase Create(bool withFieldRestrictions = false)
        {
            var bloczekDoNarysowania = new Yellow()
                .CreateNew(withFieldRestrictions).ToString();

            return new Yellow()
                .CreateNew(withFieldRestrictions);
        }
    }
}
