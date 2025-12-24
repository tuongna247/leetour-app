using System.Text.RegularExpressions;

namespace Vinaday.Web.Framework
{
    public class CreditCardUtility
    {
        private const string CardRegex = "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

        public static bool IsValidNumber(string cardNum)
        {
            //var cardTest = new Regex(CardRegex);

            //Determine the card type based on the number
            var cardType = GetCardTypeFromNumber(cardNum);

            //Call the base version of IsValidNumber and pass the 
            //number and card type
            return IsValidNumber(cardNum, cardType);
        }

        public static bool IsValidNumber(string cardNum, CreditCardTypeType? cardType)
        {
            //Create new instance of Regex comparer with our 
            //credit card regex pattern
            var cardTest = new Regex(CardRegex);

            //Make sure the supplied number matches the supplied
            //card type
            return cardTest.Match(cardNum).Groups[cardType.ToString()].Success && PassesLuhnTest(cardNum);
        }

        public static CreditCardTypeType? GetCardTypeFromNumber(string cardNum)
        {
            //Create new instance of Regex comparer with our
            //credit card regex pattern
            var cardTest = new Regex(CardRegex);

            //Compare the supplied card number with the regex
            //pattern and get reference regex named groups
            var gc = cardTest.Match(cardNum).Groups;

            //Compare each card type to the named groups to 
            //determine which card type the number matches
            if (gc[CreditCardTypeType.Amex.ToString()].Success)
            {
                return CreditCardTypeType.Amex;
            }
            else if (gc[CreditCardTypeType.MasterCard.ToString()].Success)
            {
                return CreditCardTypeType.MasterCard;
            }
            else if (gc[CreditCardTypeType.Visa.ToString()].Success)
            {
                return CreditCardTypeType.Visa;
            }
            else if (gc[CreditCardTypeType.Discover.ToString()].Success)
            {
                return CreditCardTypeType.Discover;
            }
            else
            {
                //Card type is not supported by our system, return null
                //(You can modify this code to support more (or less)
                // card types as it pertains to your application)
                return null;
            }
        }

        public static string GetCardTestNumber(CreditCardTypeType cardType)
        {
            //According to PayPal, the valid test numbers that should be used
            //for testing card transactions are:
            //Credit Card Type              Credit Card Number
            //American Express              378282246310005
            //American Express              371449635398431
            //American Express Corporate    378734493671000
            //Diners Club                   30569309025904
            //Diners Club                   38520000023237
            //Discover                      6011111111111117
            //Discover                      6011000990139424
            //MasterCard                    5555555555554444
            //MasterCard                    5105105105105100
            //Visa                          4111111111111111
            //Visa                          4012888888881881
            //Src: https://www.paypal.com/en_US/vhelp/paypalmanager_help/credit_card_numbers.htm
            //Credit: Scott Dorman, http://www.geekswithblogs.net/sdorman

            //Return bogus CC number that passes Luhn and format tests
            switch (cardType)
            {
                case CreditCardTypeType.Amex:
                    return "3782 822463 10005";
                case CreditCardTypeType.Discover:
                    return "6011 1111 1111 1117";
                case CreditCardTypeType.MasterCard:
                    return "5105 1051 0510 5100";
                case CreditCardTypeType.Visa:
                    return "4111 1111 1111 1111";
                default:
                    return null;
            }
        }

        public static bool PassesLuhnTest(string cardNumber)
        {
            //Clean the card number- remove dashes and spaces
            cardNumber = cardNumber.Replace("-", "").Replace(" ", "");

            //Convert card number into digits array
            var digits = new int[cardNumber.Length];
            for (var len = 0; len < cardNumber.Length; len++)
            {
                digits[len] = int.Parse(cardNumber.Substring(len, 1));
            }

            //Luhn Algorithm
            //Adapted from code availabe on Wikipedia at
            //http://en.wikipedia.org/wiki/Luhn_algorithm
            var sum = 0;
            var alt = false;
            for (var i = digits.Length - 1; i >= 0; i--)
            {
                var curDigit = digits[i];
                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                    {
                        curDigit -= 9;
                    }
                }
                sum += curDigit;
                alt = !alt;
            }

            //If Mod 10 equals 0, the number is good and this will return true
            return sum % 10 == 0;
        }
    }

    /// <summary>
    /// CreditCardTypeType copied for PayPal WebPayment Pro API
    /// (If you use the PayPal API, you do not need this definition)
    /// </summary>
    public enum CreditCardTypeType
    {
        Visa,
        MasterCard,
        Discover,
        Amex,
        Switch,
        Solo
    }
}
