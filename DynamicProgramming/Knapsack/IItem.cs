namespace Knapsack
{
    public interface IItem
    {
        long Id { get; }

        /// <summary>
        /// Profit
        /// </summary>
        decimal Value { get; }
        int Weight { get; }
    }

}