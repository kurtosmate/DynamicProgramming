using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
    public class Result
    {
        public decimal MaxProfit { get; }
        public List<long> IncludedIds { get; }

        public Result(decimal maxProfit, List<long> includedIds)
        {
            MaxProfit = maxProfit;
            IncludedIds = includedIds;
        }

    }
}
