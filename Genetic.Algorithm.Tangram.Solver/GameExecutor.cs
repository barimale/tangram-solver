﻿using Solver.Tangram.AlgorithmDefinitions.Generics;
using Solver.Tangram.Configuration;
using System;
using System.Collections.Generic;

namespace Algorithm.Executor.WPF
{
    public class GameExecutor
    {
        private AlgorithmDisplayHelper algorithmDisplayHelper;
        private Game konfiguracjaGry;
        private IEnumerable<AlgorithmResult> results;

        public event EventHandler GenerationRan;

        public IEnumerable<AlgorithmResult> Results => results;

        public string AlgorithmState => "TO BE DONE";
            //konfiguracjaGry?
            //.Algorithm?
            //.State
            //.ToString();

        public GameExecutor(
            AlgorithmDisplayHelper algorithmDisplayHelper,
            Game dataInput)
        {
            this.algorithmDisplayHelper = algorithmDisplayHelper;
            konfiguracjaGry = dataInput;

            if (konfiguracjaGry.Algorithm == null)
                throw new Exception();

            // TODO WIP 
            //konfiguracjaGry
            //    .Algorithm
            //    .TerminationReached += this.algorithmDisplayHelper.Algorithm_TerminationReached;

            //konfiguracjaGry
            //    .Algorithm
            //    .GenerationRan += this.algorithmDisplayHelper.Algorithm_Ran;

            //konfiguracjaGry
            //    .Algorithm
            //    .GenerationRan += this.Algorithm_Ran;

            results = new List<AlgorithmResult>();
        }

        public void Execute()
        {
            //konfiguracjaGry?.Algorithm?.Start();
            Algorithm_Ran(konfiguracjaGry?.Algorithm, null);
        }

        private void Algorithm_Ran(object? sender, EventArgs e)
        {
            GenerationRan.Invoke(sender, e);
        }

        public void Pause()
        {
            //if (konfiguracjaGry.Algorithm != null && konfiguracjaGry.Algorithm.IsRunning)
            //    konfiguracjaGry.Algorithm.Stop();
        }

        public void Resume()
        {
            //if (konfiguracjaGry.Algorithm != null && konfiguracjaGry.Algorithm.IsRunning)
            //    konfiguracjaGry.Algorithm.Resume();
        }
    }
}
