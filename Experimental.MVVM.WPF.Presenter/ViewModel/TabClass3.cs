﻿using Algorithm.Tangram.Common.Extensions;
using Solver.Tangram.Game.Logic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Tangram.GameParts.Logic.GameParts.Block;
using System.Linq;
using System.Windows.Documents;
using Tangram.GameParts.Logic.GameParts.Board;
using NetTopologySuite.Utilities;
using Tangram.GameParts.Logic.Utilities;

namespace Demo.ViewModel
{
    public class TabClass3 : TabBase
    {
        private Game _gameInstance;

        public TabClass3(ref Game gameInstance)
        {
            this._gameInstance = gameInstance;
            MyCanvasContent = MapBoardToDetails();
        }

        public UIElement MyCanvasContent { get; set; }

        private UIElement MapBoardToDetails()
        {
            var board = this._gameInstance.GameSet.Board;

            var grid = GetGridDefinition();

            // board definition label
            var boardDefinitionLabel = new TextBlock();
            boardDefinitionLabel.Text = "Board definition:";
            boardDefinitionLabel.Margin = new Thickness(0, 0, 0, 8);

            Grid.SetRow(boardDefinitionLabel, 0);
            Grid.SetColumn(boardDefinitionLabel, 0);
            grid.Children.Add(boardDefinitionLabel);

            // board definition image
            UIElement boardDefinitionContent = new DisplayBlockHelper()
                .MapBoardToCanvas(
                    _gameInstance.GameSet.Board,
                    160);

            Grid.SetRow(boardDefinitionContent, 1);
            Grid.SetColumn(boardDefinitionContent, 0);
            Grid.SetColumnSpan(boardDefinitionContent, 2);

            grid.Children.Add(boardDefinitionContent);

            var hasMeshLabel = new TextBlock();
            var hasMeshValue = board.IsExtraRistricted;
            hasMeshLabel.Text = $"Has mesh: {hasMeshValue}";
            hasMeshLabel.Margin = new Thickness(0, 0, 0, 8);

            Grid.SetRow(hasMeshLabel, 3);
            Grid.SetColumn(hasMeshLabel, 0);
            grid.Children.Add(hasMeshLabel);

            if (hasMeshValue)
            {
                var meshContent = new TextBlock();
                meshContent.Height = 160;
                meshContent.Text = ExtraRestrictionMarkupsHelper.MeshSideToString<int>(
                    board.WithExtraRestrictedMarkups,
                    board.SkippedMarkup); 

                Grid.SetRow(meshContent, 4);
                Grid.SetColumn(meshContent, 0);
                Grid.SetColumnSpan(meshContent, 1);

                grid.Children.Add(meshContent);

                var allowedMatchesLabel = new TextBlock();
                allowedMatchesLabel.Text = $"Allowed matches:";
                allowedMatchesLabel.Margin = new Thickness(0, 0, 0, 8);

                Grid.SetRow(allowedMatchesLabel, 3);
                Grid.SetColumn(allowedMatchesLabel, 1);
                Grid.SetColumnSpan(allowedMatchesLabel, 1);

                grid.Children.Add(allowedMatchesLabel);

                var allowedMatches = new TextBlock();
                var allowedMatchesValue = board
                    .AllowedMatches
                    .Select(p => $"{p.Item1} - {p.Item2}")
                    .ToArray();

                allowedMatches.Text = $"{string.Join(", ", allowedMatchesValue)}";
                allowedMatches.Margin = new Thickness(0, 0, 0, 8);

                Grid.SetRow(allowedMatches, 4);
                Grid.SetColumn(allowedMatches, 1);
                Grid.SetColumnSpan(allowedMatches, 1);

                grid.Children.Add(allowedMatches);
            }

            return grid;
        }

        private Grid GetGridDefinition()
        {
            // Create the Grid
            Grid myGrid = new Grid();

            //myGrid.Width = 250;
            //myGrid.Height = 100;
            myGrid.HorizontalAlignment = HorizontalAlignment.Left;
            myGrid.VerticalAlignment = VerticalAlignment.Top;
            myGrid.ShowGridLines = false;

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition()
            {
                Width = new GridLength(360)
            };
            ColumnDefinition colDef2 = new ColumnDefinition()
            {
                Width = new GridLength(360)
            };
            myGrid.ColumnDefinitions.Add(colDef1);
            myGrid.ColumnDefinitions.Add(colDef2);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition()
            {
                Height = GridLength.Auto
            };
            RowDefinition rowDef1b = new RowDefinition()
            {
                Height = new GridLength(160)
            }; 
            RowDefinition rowDef2 = new RowDefinition()
            {
                Height = GridLength.Auto
            };
            RowDefinition rowDef3 = new RowDefinition()
            {
                Height = GridLength.Auto
            };
            RowDefinition rowDef4 = new RowDefinition()
            {
                Height = GridLength.Auto
            };

            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef1b);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);
            myGrid.RowDefinitions.Add(rowDef4);

            return myGrid;
        }
    }
}
