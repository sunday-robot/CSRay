namespace CsRay
{
    public sealed class MyRand
    {
        const int _size = 10000;

        readonly double[] _doubleValues;
        readonly int[] _intValues;
        readonly object _lockObject = new();
        int _index = -1;

        public MyRand() => Initialize(new Random(), out _doubleValues, out _intValues);

        public double NextDouble() => _doubleValues[Index()];

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

        static void Initialize(Random rand, out double[] doubleValues, out int[] intValues)
        {
            doubleValues = new double[_size];
            intValues = new int[_size];
            for (var i = 0; i < _size; i++)
            {
                doubleValues[i] = rand.NextDouble();
                intValues[i] = rand.Next();
            }
        }
    }
}
