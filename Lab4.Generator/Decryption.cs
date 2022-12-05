using System.Text;

namespace Lab4.Generator
{
    public class Decryption
    {
        public static string DecryptMessage(string message, ulong key)
        {
            var stringBuilder = new StringBuilder();
            var keyString = key.ToString();

            int symbolIndex = 0;
            int i = 0;
            while (symbolIndex < message.Length / 4)
            {
                var symbolNumberString = GetSymbolNumberString(message, symbolIndex);
                var symbolStringBuilder = new StringBuilder();
                foreach (var symbol in symbolNumberString)
                {
                    var symbolNum = Convert.ToInt16(symbol.ToString());
                    var keyNum = Convert.ToInt16(keyString[i % keyString.Length].ToString());
                    symbolStringBuilder.Append(((symbolNum + 10) - keyNum) % 10);

                    i++;
                }

                var decryptedSymbol = (char)Convert.ToUInt64(RemoveZeros(symbolStringBuilder.ToString()));
                stringBuilder.Append(decryptedSymbol);
                symbolIndex++;
            }

            return stringBuilder.ToString();
        }

        private static string GetSymbolNumberString(string message, int index)
        {
            return message.Substring(4 * index, 4);
        }

        private static string RemoveZeros(string symbolNumber)
        {
            while (symbolNumber[0].Equals("0"))
            {
                symbolNumber = symbolNumber.Substring(1);
            }

            return symbolNumber;
        }
    }
}
