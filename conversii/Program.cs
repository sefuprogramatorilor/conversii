using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;

Console.WriteLine(converter.convert("20110.1011", 4, 16));

class converter
{
    public static decimal toDecimal(string number, int base_)
    {
        number = Regex.Replace(number, @"\s+", "");
        number = number.ToUpper();
        string wholePart = number.Split('.')[0];
        int wholeLen = wholePart.Length;
        int power = 1;
        int wholeResult = 0;
        decimal fractResult = 0;
        if (number.Contains("."))
        {
            string fractPart = number.Split('.')[1];
            int fractLen = fractPart.Length;
            int fractPower = -1;
            for (int i = 0; i < fractLen; i++)
            {
                if (value(fractPart[i]) >= base_)
                {
                    Console.WriteLine("Invalid Number");
                    return -1;
                }

                fractResult += value(fractPart[i]) * (decimal)Math.Pow(base_, fractPower);
                fractPower--;
            }
        }

        for (int i = wholeLen - 1; i >= 0; i--)
        {

            if (value(wholePart[i]) >= base_)
            {
                Console.WriteLine("Invalid Number");
                return -1;
            }

            wholeResult += value(wholePart[i]) * power;
            power *= base_;
        }

        return wholeResult + fractResult;
    }

    public static string fromDecimal(decimal input, int toBase)
    {
        int decimalWhole = (int)input;
        decimal decimalFract = input - decimalWhole;


        string s = "";
        do
        {
            s += reValue(decimalWhole % toBase);
            decimalWhole /= toBase;
        } while (decimalWhole > 0);
        char[] wholeRes = s.ToCharArray();

        Array.Reverse(wholeRes);
        string wholeResult = new(wholeRes);

        string fractResult = "";
        bool periodic = false;
        decimal initialDecimalFract = decimalFract;
        while (decimalFract != 0)
        {
            fractResult += reValue((int)(decimalFract * toBase));
            decimalFract = (decimalFract * toBase) - (int)(decimalFract * toBase);
            if (decimalFract == initialDecimalFract)
            {
                periodic = true;
                break;
            }
        }


        return $"{wholeResult}{(fractResult != "" ? "." : "")}{(periodic ? $"({fractResult})" : fractResult)}";
    }

    public static string convert(string input, int fromBase, int toBase)
    {
        return fromDecimal(toDecimal(input, fromBase), toBase);

    }

    private static int value(char c)
    {
        if (c >= '0' && c <= '9')
            return c - '0';
        else
            return c - 'A' + 10;
    }

    private static char reValue(int num)
    {
        if (num >= 0 && num <= 9)
            return (char)(num + 48);
        else
            return (char)(num - 10 + 65);
    }
}
