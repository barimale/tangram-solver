﻿using NetTopologySuite.Geometries;
using System.Drawing;
using Tangram.GameParts.Elements.Elements.Blocks.CommonSettings;
using Tangram.GameParts.Logic.GameParts.Block;

namespace Tangram.GameParts.Elements.Elements.Blocks.XOXO
{
    public sealed class Yellow : PolishGameBaseBlock
    {
        public Yellow()
        {
            fieldRestriction1side = new object[,] {
                { "O", "X", "O" },
                { NA, "O", NA },
                { NA, "X", NA }
            };

            fieldRestriction2side = new object[,] {
                { "X", "O", "X" },
                { NA, "X", NA },
                { NA, "O", NA }
            };

            color = Color.Yellow;

            polygon = new GeometryFactory()
                    .CreatePolygon(new Coordinate[] {
                        new Coordinate(0,0),// first the same as last
                        new Coordinate(0,1),
                        new Coordinate(1,1),
                        new Coordinate(1,3),
                        new Coordinate(2,3),
                        new Coordinate(2,1),
                        new Coordinate(3,1),
                        new Coordinate(3,0),
                        new Coordinate(0,0)// last the same as first
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
