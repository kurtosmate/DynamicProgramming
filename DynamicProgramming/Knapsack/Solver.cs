using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
    /// <summary>
    /// Pick the algorihtm based on the capacity and number of items
    /// Matrix based algorithm could use lots of memory on large number of weights
    /// </summary>
    public class Solver
    {
        public Result Solve(int maxWeight, List<IItem> items)
        {
            ISolveableKnapsack knapsack;
            if (maxWeight * items.Count  < 5_000_000 )
            {
                knapsack = new Knapsack.KnapsackMatrix();
            }
            else
            {
                knapsack = new Knapsack.KnapsackBtree();
            }

            return knapsack.Solve(maxWeight, items);
        }
    }
}
