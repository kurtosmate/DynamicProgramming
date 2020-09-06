using Knapsack;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicProgramming.Knapsack
{
    public class Main : IDynamicProgramming
    {
        public void Start()
        {
            int maxWeight = 20; //Max capacity
            var items = new List<IItem>
            {
                //Id, Value, Weight
                new Item(10, 5, 11),
                new Item(7, 3, 10),
                new Item(9, 15, 12),
                new Item(11, 2, 3),
                new Item(12, 4, 8),
            };

            var matrixBasedSolver = new KnapsackMatrix();
            var btreeBasedSolver = new KnapsackBtree();

            var matrixResult = matrixBasedSolver.Solve(maxWeight, items);
            Console.WriteLine("Matrix based solution max profit: " + matrixResult.MaxProfit);
            Console.WriteLine("-Items included: " + string.Join(", ", matrixResult.IncludedIds));

            
            var btreeResult = btreeBasedSolver.Solve(maxWeight, items);
            Console.WriteLine("B-tree based solution max profit: " + btreeResult.MaxProfit);
            Console.WriteLine("-Items included: " + string.Join(", ", btreeResult.IncludedIds));

            // Use B-tree based solution for large weights
            int bigMaxWeight = 20_000_000; 
            var bigItems = new List<IItem>
            {
                //Id, Value, Weight
                new Item(10, 5_000_000, 11_000_000),
                new Item(7, 3_000_000, 10_000_000),
                new Item(9, 15_000_000, 12_000_000),
                new Item(11, 2_000_000, 3_000_000),
                new Item(12, 4_000_000, 8_000_000),
            };

            Console.WriteLine("Large weights:");

            var bigBtreeResult = btreeBasedSolver.Solve(bigMaxWeight, bigItems);
            Console.WriteLine("B-tree based solution max profit: " + bigBtreeResult.MaxProfit);
            Console.Write("-Items included: " + string.Join(", ", bigBtreeResult.IncludedIds));
        }

        public class Item : IItem
        {
            public long Id { get; }

            /// <summary>
            /// Profit
            /// </summary>
            public decimal Value { get; }

            public int Weight { get; }

            public Item(long id, decimal value, int weight)
            {
                Id = id;
                Value = value;
                Weight = weight;
            }

            public override string ToString()
            {
                return $"Id: {Id}, V: {Value}, W: {Weight}";
            }
        }
    }

}
