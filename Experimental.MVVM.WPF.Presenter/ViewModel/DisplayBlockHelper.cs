﻿using Demo.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tangram.GameParts.Logic.GameParts.Block;
using Tangram.GameParts.Logic.GameParts.Board;

namespace Demo.ViewModel
{
    public class DisplayBlockHelper
    {
        public UIElement MapBlockDefinitionToCanvasWithBoard(BoardShapeBase board, BlockBase block)
        {
            // clear canvas
            var canvas = new Canvas();
            canvas.RenderTransform = new ScaleTransform(30, 30);

            // show board
            var boardComponent = new Polygon();
            boardComponent.Points = new PointCollection();
            boardComponent.Fill = Brushes.White;
            boardComponent.Stroke = Brushes.Black;
            boardComponent.StrokeThickness = 0.05d;

            board
                .Polygon
                .Coordinates
                .ToList()
                .ForEach(p => boardComponent
                    .Points
                    .Add(new Point(p.X, p.Y)));

            canvas.Children.Add(boardComponent);

            // show block definition
            var blockDefinition = new Polygon();
            blockDefinition.Points = new PointCollection(
                block
                    .Polygon
                    .Coordinates
                    .Select(pp => new Point(pp.X, pp.Y))
                    .ToList());
            blockDefinition.Fill = AlgorithmDisplayHelper.ConvertColor(block.Color);
            blockDefinition.Stroke = Brushes.Black;
            blockDefinition.StrokeThickness = 0.1d;

            canvas.Children.Add(blockDefinition);

            return canvas;
        }
    }
}
