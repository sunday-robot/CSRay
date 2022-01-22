namespace CsRay
{
    public sealed class MyRand
    {
        const int _size = 10000;

        readonly double[] _values;
        readonly int[] _intValues;
        readonly object _lockObject = new();
        int _index = -1;

        public MyRand() => Initialize(new Random(), out _values, out _intValues);

        public double NextDouble() => _values[Index()];

        public int Next() => _intValues[Index()];

        int Index()
        {
            lock (_lockObject)
            {
                _index++;
                if (_index == _size)
                    _index = 0;
                return _index;
            }
        }

        static void Initialize(Random rand, out double[] values, out int[] intValues)
        {
            values = new double[_size];
            intValues = new int[_size];
            for (var i = 0; i < _size; i++)
            {
                values[i] = rand.NextDouble();
                intValues[i] = rand.Next();
            }
        }
    }
}
