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
        public ulong P { get; } = 0; // TODO: find 32 bit prime number

        public ulong GetG()
        {
            if (_g == 0)
            {
                // TODO: generate 'g'
            }

            return _g;
        }
    }
}
