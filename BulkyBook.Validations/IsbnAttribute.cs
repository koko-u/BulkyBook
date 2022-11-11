using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BulkyBook.Validations;

public class IsbnAttribute : ValidationAttribute
{
    private readonly Regex _isbnPattern = new Regex(@"(\d{3})-(\d{1})-(\d{4})-(\d{4})-(\d{1})");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not string isbnValue)
        {
            return ValidationResult.Success;
        }

        var match = _isbnPattern.Match(isbnValue);
        if (match.Success)
        {
            var prefix = int.Parse(match.Groups[1].Value);
            var registrationGroup = int.Parse(match.Groups[2].Value);
            var registrant = int.Parse(match.Groups[3].Value);
            var publication = int.Parse(match.Groups[4].Value);
            var checkSum = int.Parse(match.Groups[5].Value);

            if (prefix != 978 && prefix != 979)
            {
                return new ValidationResult("ISBN first 3-digits is either 978 or 979");
            }

            if (CheckSumOf(prefix, registrationGroup, registrant, publication) != checkSum)
            {
                return new ValidationResult("Checksum is not valid value.");
            }

            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("ISBN code has the format nnn-x-aaaa-bbbb-c");
        }
    }

    private Stack<int> DigitsOf(Stack<int> acc, int n)
    {
        if (n <= 0)
        {
            return acc;
        }

        var q = n / 10;
        var r = n % 10;

        acc.Push(r);
        return DigitsOf(acc, q);
    }


    private int CheckSumOf(int prefix, int registrationGroup, int registrant, int publication)
    {
        var digits = new Stack<int>();
        digits = DigitsOf(digits, publication);
        digits = DigitsOf(digits, registrant);
        digits = DigitsOf(digits, registrationGroup);
        digits = DigitsOf(digits, prefix);

        var sum = 0;
        foreach (var (digit, isEven) in digits.Select((d, idx) => (d, idx % 2 == 0)))
        {
            if (isEven)
            {
                sum += digit;
            }
            else
            {
                sum += digit * 3;
            }
        }

        var r = sum % 10;
        if (r == 0)
        {
            return 0;
        }
        else
        {
            return 10 - r;
        }
    }
}