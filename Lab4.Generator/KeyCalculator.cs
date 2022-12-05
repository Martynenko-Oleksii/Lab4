namespace Lab4.Generator
{
    public class KeyCalculator
    {
        public static ulong CalculateKey(ulong g, ulong p, ulong x)
        {
            ulong i = 0;
            ulong c = 1;
            while (i < x)
            {
                i++;
                c = (g * c) % p; 
            }

            return c;
        }
    }
}
