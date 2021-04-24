using amount_in_words.Abstractions;
using amount_in_words.Enums;
using amount_in_words.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amount_in_words.Implementation
{
    public class RublesToWordConverter : IAmountToWordConverter
    {
        readonly string zero = "ноль";
        readonly string firstMale = "один";
        readonly string firstFemale = "одна";
        readonly string firstFemaleAccusative = "одну";
        readonly string firstMaleGenetive = "одно";
        readonly string secondMale = "два";
        readonly string secondFemale = "две";
        readonly string secondMaleGenetive = "двух";
        readonly string secondFemaleGenetive = "двух";

        readonly string[] from3till19 = { "", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
        readonly string[] from3till19Genetive = { "", "трех", "четырех", "пяти", "шести", "семи", "восеми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
        readonly string[] tens = { "", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
        readonly string[] tensGenetive = { "", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "восьмидесяти", "девяноста" };
        readonly string[] hundreds = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
        readonly string[] hundredsGenetive = { "", "ста", "двухсот", "трехсот", "четырехсот", "пятисот", "шестисот", "семисот", "восемисот", "девятисот" };
        readonly string[] thousands = { "", "тысяча", "тысячи", "тысяч" };
        readonly string[] thousandsAccusative = { "", "тысячу", "тысячи", "тысяч" };
        readonly string[] millions = { "", "миллион", "миллиона", "миллионов" };
        readonly string[] billions = { "", "миллиард", "миллиарда", "миллиардов" };
        readonly string[] trillions = { "", "трилион", "трилиона", "триллионов" };
        readonly string[] rubles = { "", "рубль", "рубля", "рублей" };
        readonly string[] copecks = { "", "копейка", "копейки", "копеек" };

        bool IsPluralGenitive(int _digits)
        {
            return _digits >= 5 || _digits == 0;
        }

        bool IsSingularGenitive(int _digits)
        {
            return _digits >= 2 && _digits <= 4;
        }

        int LastDigit(long _amount)
        {
            long amount = _amount;

            if (amount >= 100)
                amount %= 100;

            if (amount >= 20)
                amount %= 10;

            return (int)amount;
        }

        /// <summary>
        /// Convert number to word
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="convertCents"></param>
        /// <returns></returns>
        public string CurrencyToWord(decimal amount, bool convertCents = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ConvertRubles(amount));
            sb.AppendLine(ConvertCopecks(amount, convertCents));
            return StringHelper.ToString(sb);
        }

        string ConvertRubles(decimal amount)
        {
            StringBuilder sb = new StringBuilder();

            long rublesAmount = (long)Math.Floor(amount);
            int lastRublesDigit = LastDigit(rublesAmount);

            sb.AddLine(NumeralsToTxt(rublesAmount, TextCase.Nominative));

            if (IsPluralGenitive(lastRublesDigit))
            {
                sb.AddLine(rubles[3]);
            }
            else if (IsSingularGenitive(lastRublesDigit))
            {
                sb.AddLine(rubles[2]);
            }
            else
            {
                sb.AddLine(rubles[1]);
            }

            return StringHelper.ToString(sb);
        }

        string ConvertCopecks(decimal amount, bool copecksInWord)
        {
            StringBuilder sb = new StringBuilder();
            long copecksAmount = ((long)Math.Round(amount * 100)) % 100;
            int lastCopecksDigit = LastDigit(copecksAmount);

            sb.AddLine(copecksInWord ? NumeralsToTxt(copecksAmount, TextCase.Nominative, false, false) : string.Format("{0:00}", copecksAmount));

            if (IsPluralGenitive(lastCopecksDigit))
            {
                sb.AddLine(copecks[3]);
            }
            else if (IsSingularGenitive(lastCopecksDigit))
            {
                sb.AddLine(copecks[2]);
            }
            else
            {
                sb.AddLine(copecks[1]);
            }
            return StringHelper.ToString(sb);

        }

        string MakeText(int _digits, string[] _hundreds, string[] _tens, string[] _from3till19, string _second, string _first, string[] _power = null)
        {
            StringBuilder sb = new StringBuilder();

            int digits = _digits;

            if (digits >= 100)
            {
                sb.AddLine(_hundreds[digits / 100]);
                digits %= 100;
            }
            if (digits >= 20)
            {
                sb.AddLine(_tens[digits / 10 - 1]);
                digits %= 10;
            }

            if (digits >= 3)
            {
                sb.AddLine(_from3till19[digits - 2]);
            }
            else if (digits == 2)
            {
                sb.AddLine(_second);
            }
            else if (digits == 1)
            {
                sb.AddLine(_first);
            }

            if (_digits != 0 && _power?.Length > 0)
            {
                digits = LastDigit(_digits);

                if (IsPluralGenitive(digits))
                {
                    sb.AddLine(_power[3]);
                }
                else if (IsSingularGenitive(digits))
                {
                    sb.AddLine(_power[2]);
                }
                else
                {
                    sb.AddLine(_power[1]);
                }
            }

            return StringHelper.ToString(sb);
        }

        /// <summary>
        /// реализовано для падежей: именительный (nominative), родительный (Genitive),  винительный (accusative)
        /// </summary>
        /// <param name="_sourceNumber"></param>
        /// <param name="_case"></param>
        /// <param name="_isMale"></param>
        /// <param name="_firstCapital"></param>
        /// <returns></returns>
        string NumeralsToTxt(long _sourceNumber, TextCase _case, bool _isMale = true, bool _firstCapital = true)
        {
            long number = _sourceNumber;
            int remainder;
            int power = 0;

            if ((number >= (long)Math.Pow(10, 15)) || number < 0)
            {
                throw new Exception("Данное число не поддерживается");
            }

            StringBuilder sb = new StringBuilder();

            while (number > 0)
            {
                remainder = (int)(number % 1000);
                number /= 1000;

                switch (power)
                {
                    case 12:
                        sb.AddLine(MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, trillions));
                        break;
                    case 9:
                        sb.AddLine(MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, billions));
                        break;
                    case 6:
                        sb.AddLine(MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, millions));
                        break;
                    case 3:
                        sb.AddLine(_case == TextCase.Accusative
                            ? MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemaleAccusative, thousandsAccusative)
                            : MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemale, thousands));
                        break;
                    default:
                        switch (_case)
                        {
                            case TextCase.Genitive:
                                sb.AddLine(MakeText(remainder, hundredsGenetive, tensGenetive, from3till19Genetive, _isMale ? secondMaleGenetive : secondFemaleGenetive, _isMale ? firstMaleGenetive : firstFemale));
                                break;
                            case TextCase.Accusative:
                                sb.AddLine(MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemaleAccusative));
                                break;
                            default:
                                sb.AddLine(MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemale));
                                break;
                        }
                        break;
                }

                power += 3;
            }

            if (_sourceNumber == 0)
            {
                sb.AddLine(zero);
            }

            var s = sb.ToString(reverseOrder: true);

            if (!string.IsNullOrWhiteSpace(s) && _firstCapital)
                s = s.Substring(0, 1).ToUpper() + s[1..];

            return s;
        }


    }
    
}
