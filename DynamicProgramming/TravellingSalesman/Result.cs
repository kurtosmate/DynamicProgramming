using System;
using System.Collections.Generic;
using System.Text;

namespace TravellingSalesman
{
    public class Result
    {
        public int MinimumDistance { get;  }
        public List<int> Path { get; }

        public Result(int minimumDistance, List<int> path)
        {
            MinimumDistance = minimumDistance;
            Path = path;
        }
    }
}
