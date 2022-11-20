using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Generator
{
    public class GeneralValues
    {
        private ulong _g = 0;
        public ulong P { get; } = 2147483647; // TODO: find 32 bit prime number

        public ulong GetG()
        {
            if (_g == 0)
            {
                _g = findPrimitive(P);
            }

            return _g;
        }

        static ulong power(ulong x, ulong y, ulong p)
        {
            ulong res = 1;

            x = x % p; 

            while (y > 0)
            {
                if (y % 2 == 1)
                {
                    res = (res * x) % p;
                }

                y = y >> 1;
                x = (x * x) % p;
            }
            return res;
        }

        static void findPrimefactors(HashSet<ulong> s, ulong n)
        {
            while (n % 2 == 0)
            {
                s.Add(2);
                n = n / 2;
            }

            for (ulong i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                while (n % i == 0)
                {
                    s.Add(i);
                    n = n / i;
                }
            }

            if (n > 2)
            {
                s.Add(n);
            }
        }

        static ulong findPrimitive(ulong n)
        {
            HashSet<ulong> s = new HashSet<ulong>();

            ulong phi = n - 1;

            findPrimefactors(s, phi);

            for (ulong r = 2; r <= phi; r++)
            {
                bool flag = false;
                foreach (ulong a in s)
                {

                    if (power(r, phi / (a), n) == 1)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == false)
                {
                    return r;
                }
            }

            return 0;
        }
    }
}
