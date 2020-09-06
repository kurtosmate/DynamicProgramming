using System;
using System.Collections.Generic;
using System.Linq;

namespace Knapsack
{
    /// <summary>
    /// Solves 0/1 Knapsack problem using with B-tree - branch and bound solution
    /// Should use with bigger capacity (c >= 1_000_000)
    /// It is used when we have a limited capacity of item weight and many items with value
    /// We'd like to choose the highest values of items with the total weight we can carry by the limited capacity
    /// </summary>
    public class KnapsackBtree : ISolveableKnapsack
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
            int size = items.Count;
            // sorting Item on basis of value per unit 
            // weight. 
            items.Sort(new RatioComparer());

            // Create map for Ids to handle high Id values without creating a big "path" arrays
            Dictionary<long, long> mappedIds = new Dictionary<long, long>();
            for (int i = 0; i < size; i++)
            {
                mappedIds.Add(i, items[i].Id);
            }

            Node current = new Node();
            Node left = new Node();
            Node right = new Node();

            // min_lb -> Minimum lower bound 
            // of all the nodes explored 

            // final_lb -> Minimum lower bound 
            // of all the paths that reached 
            // the final level 
            decimal minLB = 0, finalLB = Int32.MaxValue;

            // make a queue for traversing the node 
            SortedSet<Node> Q = new SortedSet<Node>();
            // dummy node at starting 
            Q.Add(current);

            bool[] currPath = new bool[size];
            bool[] finalPath = new bool[size];
            while (Q.Count != 0)
            {
                // Dequeue a node 
                current = Q.Min();
                Q.Remove(current);

                if (current.UpperBoud > minLB
                || current.UpperBoud >= finalLB)
                {
                    // if the current node's best case 
                    // value is not optimal than minLB, 
                    // then there is no reason to 
                    // explore that node. Including 
                    // finalLB eliminates all those 
                    // paths whose best values is equal 
                    // to the finalLB 
                    continue;
                }

                if (current.Level != 0)
                {
                    currPath[current.Level - 1] = current.IsSelected;
                }

                if (current.Level == size)
                {
                    if (current.LowerBound < finalLB)
                    {
                        // Reached last level 
                        for (int i = 0; i < size; i++)
                        {
                            finalPath[i] = currPath[i];
                        }
                        finalLB = current.LowerBound;
                    }
                    continue;
                }

                int level = current.Level;

                // right node -> Exludes current item 
                // Hence, cp, cw will obtain the value 
                // of that of parent 
                AssignNode(right,
                        UpperBound(current.Profit, current.Weight, level + 1, items, size, maxWeight),
                       LowerBound(current.Profit, current.Weight, level + 1, items, size, maxWeight),
                       level + 1, false,
                       current.Profit, current.Weight);

                if (current.Weight + items[current.Level].Weight <= maxWeight)
                {

                    // left node -> includes current item 
                    // c and lb should be calculated 
                    // including the current item. 
                    left.UpperBoud = UpperBound(
                        current.Profit - items[level].Value,
                        current.Weight + items[level].Weight,
                        level + 1, items, size, maxWeight);

                    left.LowerBound = LowerBound(
                        current.Profit - items[level].Value,
                        current.Weight + items[level].Weight,
                        level + 1, items, size, maxWeight);

                    AssignNode(left, left.UpperBoud, left.LowerBound,
                           level + 1, true,
                           current.Profit - items[level].Value,
                           current.Weight + items[level].Weight);
                }
                // If the left node cannot 
                // be inserted 
                else
                {

                    // Stop the left node from 
                    // getting added to the 
                    // priority queue 
                    left.UpperBoud = left.LowerBound = 1;
                }
                // Update minLB 
                minLB = Math.Min(minLB, left.LowerBound);
                minLB = Math.Min(minLB, right.LowerBound);

                if (minLB >= left.UpperBoud)
                    Q.Add(new Node(left));
                if (minLB >= right.UpperBoud)
                    Q.Add(new Node(right));
            }

            // Console.WriteLine ("Items taken into the knapsack are");
            for (int i = 0; i < size; i++)
            {
                if (finalPath[i])
                {
                    //Console.Write("1 ");
                    itemsIncluded.Add(mappedIds[i]);
                }
                else
                {
                    //Console.Write("0 ");
                }
            }
            //Console.WriteLine("Profit:" + (-finalLB));

            return new Result(-finalLB, itemsIncluded);
        }

        class Node : IComparable<Node>
        {
            // level: Level of node in decision tree (or index 
            //             in arr[] 
            // profit: Profit of nodes on path from root to this 
            //            node (including this node) 
            // bound: Upper bound of maximum profit in subtree 
            //            of this node/ 
            public int Level;
            public decimal Weight, Profit, LowerBound, UpperBoud;
            public bool IsSelected;
            public Node() { }
            public Node(Node n)
            {
                Level = n.Level;
                Weight = n.Weight;
                Profit = n.Profit;
                LowerBound = n.LowerBound;
                UpperBoud = n.UpperBoud;
                IsSelected = n.IsSelected;
            }
            public int CompareTo(Node other)
            {
                if (this.Weight == other.Weight && this.Profit == other.Profit)
                {
                    return 0;
                }
                else
                {
                    return this.LowerBound > other.LowerBound ? 1 : -1;
                }
            }

        };

        void AssignNode(Node a, decimal upperBound, decimal lowerBound,
                      int level, bool isSelected,
                      decimal profit, decimal weight)
        {
            a.UpperBoud = upperBound;
            a.LowerBound = lowerBound;
            a.Level = level;
            a.IsSelected = isSelected;
            a.Profit = profit;
            a.Weight = weight;
        }

        class RatioComparer : IComparer<IItem>
        {
            int IComparer<IItem>.Compare(IItem a, IItem b)
            {
                decimal r1 = a.Value / a.Weight;
                decimal r2 = b.Value / b.Weight;
                return r2 > r1 ? 1 : -1;
            }
        }

        decimal UpperBound(decimal tv, decimal tw,
                            int idx, List<IItem> items, int size, int capacity)
        {
            decimal value = tv;
            decimal weight = tw;
            for (int i = idx; i < size; i++)
            {
                if (weight + items[i].Weight
                    <= capacity)
                {
                    weight += items[i].Weight;
                    value -= items[i].Value;
                }
                else
                {
                    value -= (capacity - weight)
                             / items[i].Weight
                             * items[i].Value;
                    break;
                }
            }
            return value;
        }
        decimal LowerBound(decimal tv, decimal tw,
                            int idx, List<IItem> items, int size, int capacity)
        {
            decimal value = tv;
            decimal weight = tw;
            for (int i = idx; i < size; i++)
            {
                if (weight + items[i].Weight
                    <= capacity)
                {
                    weight += items[i].Weight;
                    value -= items[i].Value;
                }
                else
                {
                    break;
                }
            }
            return value;
        }

    }
}
