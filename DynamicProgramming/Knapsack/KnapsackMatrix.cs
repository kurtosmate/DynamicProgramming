using System;
using System.Collections.Generic;


namespace Knapsack
{
    /// <summary>
    /// Solves 0/1 Knapsack problem with matrix
    /// Should use with small capacity (c < 1_000_000)
    /// It is used when we have a limited capacity of item weight and many items with value
    /// We'd like to choose the highest values of items with the total weight we can carry by the limited capacity
    /// </summary>
    public class KnapsackMatrix : ISolveableKnapsack
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxWeight"></param>
        /// <param name="items"></param>
        /// <returns>Ids of included items</returns>
        public Result Solve(int maxWeight, List<IItem> items)
        {
            List<long> itemsIncluded = new List<long>();
            int n = items.Count;
            int i, w;
            decimal[,] K = new decimal[n + 1, maxWeight + 1];

            // Build table K[][] in bottom up manner  
            for (i = 0; i <= n; i++)
            {
                for (w = 0; w <= maxWeight; w++)
                {
                    if (i == 0 || w == 0)
                    {
                        K[i, w] = 0;
                    }
                    else if (items[i - 1].Weight <= w)
                    {
                        K[i, w] =
                        Math.Max(items[i - 1].Value + K[i - 1, w - items[i - 1].Weight], K[i - 1, w]);
                    }
                    else
                    {
                        K[i, w] = K[i - 1, w];
                    }
                    
                }
            }

            // stores the result of Knapsack  
            decimal res = K[n, maxWeight];
            var maxProfit = res;
            w = maxWeight;
            for (i = n; i > 0 && res > 0; i--)
            {

                // either the result comes from the top  
                // (K[i-1][w]) or from (val[i-1] + K[i-1]  
                // [w-wt[i-1]]) as in Knapsack table. If  
                // it comes from the latter one/ it means  
                // the item is included.  
                if (res == K[i - 1, w])
                    continue;
                else
                {

                    // This item is included.  
                    itemsIncluded.Add(items[i - 1].Id);

                    // Since this weight is included its value is deducted
                    res = res - items[i - 1].Value;
                    w = w - items[i - 1].Weight;
                }
            }

            return new Result(maxProfit, itemsIncluded);
        }


    }

}