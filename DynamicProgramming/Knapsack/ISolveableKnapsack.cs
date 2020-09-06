using System.Collections.Generic;

namespace Knapsack
{
    public interface ISolveableKnapsack
    {
        Result Solve(int maxWeight, List<IItem> items);
    }
}