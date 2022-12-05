namespace Lab4.Generator
{
    public class RandomKey
    {
        private readonly Random _rand;
        private readonly ulong _p;
        private ulong _key = 0;

        public RandomKey(ulong p)
        {
            _rand = new Random();
            _p = p;
        }

        public ulong GetPrivateKey()
        {
            if (_key == 0)
            {
                _key = (ulong)_rand.NextInt64(2, (long)_p); ;
            }

            return _key;
        }
    }
}
