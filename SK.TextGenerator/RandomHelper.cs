using System;
using System.Threading;

namespace SK.TextGenerator
{
    //https://github.com/dochoffiday/Lorem.NET/blob/master/Lorem.NET/Lorem.cs
    public static class RandomHelper
    {
        private static int _seedCounter = new Random().Next();

        [ThreadStatic]
        private static Random _rng;

        public static Random Instance
        {
            get
            {
                if (_rng == null)
                {
                    int seed = Interlocked.Increment(ref _seedCounter);
                    _rng = new Random(seed);
                }
                return _rng;
            }
        }
    }
}