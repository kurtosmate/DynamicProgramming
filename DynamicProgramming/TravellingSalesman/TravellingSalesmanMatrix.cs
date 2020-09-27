using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravellingSalesman
{
    /// <summary>
    /// Solves Travelling salesman problem with matrix based solution.
    /// Returns the shortest possible route that visits each vertices exactly once and returns to the origin vertices in a graph
    /// </summary>
    public class TravellingSalesmanMatrix : ISolveableTravellingSalesman
    {
        private const int marker = int.MaxValue;
        private int[,] pathHelper;
        public Result Solve(int[,] map)
        {
            //init
            var state = new int[map.GetLength(0), 1 << map.GetLength(0)]; // length * 2^length size matrix
            pathHelper = new int[state.GetLength(0), state.GetLength(1)];

            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    state[i, j] = marker;
                    pathHelper[i, j] = -1;
                }
            }

            // Result
            var minDistance = tsp(map, 0, 1, state);
            var fullPath = getPath(pathHelper, map.GetLength(0));

            return new Result(minDistance, fullPath);
        }

        private int tsp(int[,] cities, int pos, int visited, int[,] state)
        {
            if (visited == (state.GetLength(1) - 1))
                return (cities[pos, 0]); // return to starting city

            if (state[pos, visited] != marker)
                return (state[pos, visited]);
            int k = 0;
            for (int i = 0; i < cities.GetLength(0); i++)
            {
                // can't visit ourselves unless we're ending and skip if already visited
                if ((visited & (1 << i)) == 0)
                {

                    var distance = cities[pos, i] + tsp(cities, i, (visited + (1 << i)), state);

                    if (distance < state[pos, visited])
                    {
                        k = i;
                        state[pos, visited] = distance;
                    }
                }
            }

            pathHelper[pos, visited] = k;
            var result = state[pos, visited];

            return (result);
        }

        private List<int> getPath(int[,] path, int citiesLength)
        {
            var route = new List<int>() { 1 }; //Start from first position
            int idx = 0;
            int count = 0;
            int[] tracker = new int[citiesLength];
            for (int i = 0; i < citiesLength; i++)
            {
                tracker[i] = -1;
            }


            while (count < citiesLength - 1)
            {
                for (int i = 0; i < path.GetLength(1); i++)
                {
                    if (path[idx, i] != -1 && tracker[path[idx, i]] == -1)
                    {
                        tracker[path[idx, i]]++;
                        idx = path[idx, i];

                        route.Add(idx + 1);

                        count++;
                        break;
                    }
                }
            }
            route.Add(1); //Back to starting point
            return route;
        }
    }
}
