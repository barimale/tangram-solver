﻿using NetTopologySuite.Geometries;
using System.Drawing;
using Tangram.GameParts.Elements.Elements.Blocks.CommonSettings;
using Tangram.GameParts.Logic.GameParts.Block;

namespace Tangram.GameParts.Elements.Elements.Blocks.XOXO
{
    public sealed class Purple : PolishGameBaseBlock
    {
        public Purple()
        {
            fieldRestriction1side = new object[,] {
                { "X", "O", "X" },
                { "O", NA, "O" }
            };

            fieldRestriction2side = new object[,] {
                { "O", "X", "O" },
                { "X", NA, "X" }
            };

            color = Color.Purple;

            polygon = new GeometryFactory()
            .CreatePolygon(new Coordinate[] {
                    new Coordinate(0,0),// first the same as last
                    new Coordinate(0,2),
                    new Coordinate(1,2),
                    new Coordinate(1,1),
                    new Coordinate(2,1),
                    new Coordinate(2,2),
                    new Coordinate(3,2),
                    new Coordinate(3,0),
                    new Coordinate(0,0)// last the same as first
            });
        }

        public static BlockBase Create(bool withFieldRestrictions = false)
        {
            var bloczekDoNarysowania = new Purple()
                .CreateNew(withFieldRestrictions).ToString();

            return new Purple()
                .CreateNew(withFieldRestrictions);
        }
    }
}
