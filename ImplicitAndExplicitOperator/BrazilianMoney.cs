using System.Globalization;

namespace ConsoleAppExplicitOperator
{
    public class BrazilianMoney
    {
        public BrazilianMoney(decimal value)
        {
            CultureInfo = new CultureInfo("pt-BR");
            Value = value;
        }
        private CultureInfo CultureInfo { get; set; }
        private decimal Value { get; set; }

        public override string ToString()
        {
            return Math.Round(Value, 2).ToString(CultureInfo);
        }

        public static explicit operator decimal(BrazilianMoney money)
            => money.Value;

        public static implicit operator BrazilianMoney(decimal value)
            => new BrazilianMoney(value);

        /// <summary>
        /// Converts string in a BrazilianMoney object
        /// </summary>
        /// <param name="value">Examples: 10.25 / 50,00 / 100,999.99 / 100.000,99 </param>
        public static implicit operator BrazilianMoney(string value)
        {
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, "^[0-9]*$"))
                    return new BrazilianMoney(decimal.Parse(value));

                var pointIndex = value.IndexOf('.');
                var CommaIndex = value.IndexOf(',');

                var decimalSeparatorIndex = pointIndex > CommaIndex ? pointIndex : CommaIndex;

                var integerPart = string.Empty;
                var decimalPart = value.Substring(decimalSeparatorIndex + 1);

                for (int i = 0; i < value.Substring(0, decimalSeparatorIndex).Length; i++)
                {
                    if (char.IsDigit(value[i]))
                        integerPart += value[i];
                }

                var numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                return new BrazilianMoney(decimal.Parse($"{integerPart}{numberDecimalSeparator}{decimalPart}"));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Incompatible input '{value}'.", ex);
            } 
        }
    }
}
