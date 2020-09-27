using System;
using TravellingSalesman;

namespace DynamicProgramming.TravellingSalesman
{
    public class Main : IDynamicProgramming
    {
        public void Start()
        {
            var map = new int[,] {
                { 0,2,2,1,4 },
                { 2,0,3,2,3 },
                { 2,3,0,2,2 },
                { 1,2,2,0,4 },
                { 4,3,2,4,0 }
            };

            ISolveableTravellingSalesman travellingSalesmanMatrix = new TravellingSalesmanMatrix();
            var result = travellingSalesmanMatrix.Solve(map);
            showResult(result);


            var map2 = new int[,] { 
                {0, 20, 42, 25},
                {20, 0, 30, 34},
                {42, 30, 0,  10},
                {25, 34, 10, 0}
            };
            var result2 = travellingSalesmanMatrix.Solve(map2);
            showResult(result2);
        }

        void showResult(Result result)
        {
            Console.WriteLine($"Minimum distance: {result.MinimumDistance}");
            Console.WriteLine($"Path: {string.Join(" -> ", result.Path)}");
        }
    }
}
