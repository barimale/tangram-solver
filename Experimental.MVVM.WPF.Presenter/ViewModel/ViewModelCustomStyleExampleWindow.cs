﻿using Solver.Tangram.Game.Logic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Demo.ViewModel
{
    public class ViewModelCustomStyleExampleWindow : ViewModelExampleBase, IViewModelCustomStyleExampleWindow
    {

        public ViewModelCustomStyleExampleWindow()
        {
            ItemCollection.Add(CreateSolutionCircuitTab(ref base._gameInstance));
            ItemCollection.Add(CreateGameElementsTab(ref base._gameInstance));
            ItemCollection.Add(CreateBoardDetailsTab(ref base._gameInstance));
           
            SelectedTab = ItemCollection.FirstOrDefault();
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemCollection);
            //This sort description is what keeps the source collection sorted, based on tab number. 
            //You can also use the sort description to manually sort the tabs, based on your own criterias.
            view.SortDescriptions.Add(new SortDescription("TabNumber", ListSortDirection.Ascending));
        }

        // TODO: Custom dynamic
        private Game CreateGame()
        {
            var gameParts = GameBuilder
                    .AvalaibleGameSets
                    .CreatePolishBigBoard(withAllowedLocations: true);

            // reorder gameparts
            var orderedBlocks = gameParts
                .Blocks
                .OrderByDescending(p => p.AllowedLocations.Length)
                .ToList();

            gameParts.Blocks.Clear();
            orderedBlocks.ForEach(pp => gameParts.Blocks.Add(pp));

            var algorithm = GameBuilder
                .AvalaibleTSTemplatesAlgorithms
                .CreateDepthFirstTreeSearchAlgorithm(
                    gameParts.Board,
                    gameParts.Blocks);

            var konfiguracjaGry = new GameBuilder()
                .WithGamePartsConfigurator(gameParts)
                .WithAlgorithm(algorithm)
                .Build();

            return konfiguracjaGry;
        }
    }
}
