using System.Text;

namespace Lab4.Generator
{
    public class Encryption
    {
        public static string EncryptMessage(string message, ulong key)
        {
            var stringBuilder = new StringBuilder();
            var keyString = key.ToString();

            int i = 0;
            foreach (var symbol in message)
            {
                var symbolNumberString = GetSymbolNumberString(symbol);
                foreach (var symbolNumber in symbolNumberString)
                {
                    var messageNum = Convert.ToInt16(symbolNumber.ToString());
                    var keyNum = Convert.ToInt16(keyString[i % keyString.Length].ToString());
                    stringBuilder.Append((messageNum + keyNum) % 10);

                    i++;
                }
            }

            return stringBuilder.ToString();
        }

        private static string GetSymbolNumberString(char symbol)
        {
            var symbolNumberString = ((int)symbol).ToString();
            if (symbolNumberString.Length < 4)
            {
                while (symbolNumberString.Length != 4)
                {
                    symbolNumberString = "0" + symbolNumberString;
                }
            }

            return symbolNumberString;
        }
    }
}
