using Genetic.Algorithm.Tangram.Solver.Logic.UT.BaseUT;
using Solver.Tangram.AlgorithmDefinitions.Generics.SingleAlgorithm;
using Solver.Tangram.Game.Logic;
using System.Reflection;
using Tangram.GameParts.Logic.GameParts;
using Xunit.Abstractions;

namespace Genetic.Algorithm.Tangram.Solver.Logic.UT.UTs.Activator
{
    public class Activator_Needs_To_Be_Executed_Instead_Of_SwitchPattern : PrintToConsoleUTBase
    {
        public Activator_Needs_To_Be_Executed_Instead_Of_SwitchPattern(ITestOutputHelper output)
            : base(output)
        {
            // intentionally left blank
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public GameSet? Example_CreateBigBoard(bool withAllowedLocations)
        {
            // given
            var controlClassName = "Tangram.GameParts.Elements.GameSetFactory";
            var methodName = "CreateBigBoard";

            // when
            var filtered = FilterAssembliesBy(controlClassName);
            if (filtered == null)
            {
                return null;
            }

            var assembly = filtered.FirstOrDefault();
            if(assembly == null)
            {
                return null;
            }

            Type t = assembly.GetType(controlClassName);
            var inst = System.Activator.CreateInstance(t);

            var property = t.GetMethod(methodName);
            if (property == null)
            {
                return null;
            }

            GameSet gameParts = (GameSet)property.Invoke(inst, new object[] { withAllowedLocations });

            // then
            Assert.NotNull(gameParts);

            return gameParts;
        }

        [Fact]
        public IExecutableAlgorithm? Example_binDepthTS()
        {
            // given
            var controlClassName = "TreeSearch.Algorithm.Tangram.Solver.Templates.TSTemplatesFactory";
            var methodName = "CreateDepthFirstTreeSearchAlgorithm";
            int maxDegreeOfParallelism = 2048 * 6; // -1
            var withAllowedLocations = true;
            var gameParts = Example_CreateBigBoard(withAllowedLocations);

            // when
            var filtered = FilterAssembliesBy(controlClassName);
            if (filtered == null)
            {
                return null;
            }

            var assembly = filtered.FirstOrDefault();
            if (assembly == null)
            {
                return null;
            }

            Type t = assembly.GetType(controlClassName);
            var inst = System.Activator.CreateInstance(t);

            var method = t.GetMethod(methodName);
            if (method == null)
            {
                return null;
            }

            IExecutableAlgorithm binDepthTS = (IExecutableAlgorithm)method.Invoke(inst, new object[] {
                gameParts.Board,
                gameParts.Blocks,
                maxDegreeOfParallelism
                });

            // then
            Assert.NotNull(binDepthTS);

            return binDepthTS;
        }

        // Summary: do mappings between input data and more meaningful overal 
        // description of the board and algorithms

        [Fact]
        public void Example_konfiguracjaGry()
        {
            // given
                // 1. required input data
                //var controlClassName = "Tangram.GameParts.Elements.GameSetFactory";
                //var methodName = "CreateBigBoard";
                //var withAllowedLocations = true;
                var gameParts = Example_CreateBigBoard(true);

                // 2. required input data
                //var controlClassName = "TreeSearch.Algorithm.Tangram.Solver.Templates.TSTemplatesFactory";
                //var methodName = "CreateDepthFirstTreeSearchAlgorithm";
                //int maxDegreeOfParallelism = 2048 * 6; // -1
                var binDepthTS = Example_binDepthTS();

            // when 
            Assert.NotNull(gameParts);
            Assert.NotNull(binDepthTS);

            var konfiguracjaGry = new GameBuilder()
                .WithGamePartsConfigurator(gameParts)
                .WithAlgorithm(binDepthTS)
                .Build();

            // then
            Assert.NotNull(konfiguracjaGry);
        }

        // comment Skip to do the test
        [Fact
            (Skip = "As amount of RAM is device-specific, the test is skipped.")
        ]
        public void Check_amount_of_RAM()
        {
            // given
            var gcMemoryInfo = GC.GetGCMemoryInfo();

            // when
            long installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes;
            var physicalMemoryInGigaBytes = (double)installedMemory / 1048576.0 / 1024.0;
            var physicalMemoryInGigaBytesAsInt = Convert.ToInt32(physicalMemoryInGigaBytes);

            // then
            Assert.Equal(16, physicalMemoryInGigaBytesAsInt);
        }

        private static List<Assembly?>? FilterAssembliesBy(string controlClassName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var filtered = assemblies
                .Select(p =>
                {
                    try
                    {
                        Type t = p.GetType(controlClassName);
                        if (t != null) return p;

                        return null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }  
                })
                .Where(p => p!= null)
                .ToList();

            return filtered;
        }
    }
}