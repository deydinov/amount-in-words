using amount_in_words.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace amount_in_words.Implementation.RU
{
    public class AmountToWordConverter : IAmountToWordConverter
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
            if (_digits >= 5 || _digits == 0)
                return true;

            return false;
        }
        bool IsSingularGenitive(int _digits)
        {
            if (_digits >= 2 && _digits <= 4)
                return true;

            return false;
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
        /// Десять тысяч рублей 67 копеек
        /// </summary>
        /// <param name="_amount"></param>
        /// <param name="_firstCapital"></param>
        /// <returns></returns>
        public string CurrencyToWord(double amount, bool convertCents = true)
        {
            long rublesAmount = (long)Math.Floor(amount);
            long copecksAmount = ((long)Math.Round(amount * 100)) % 100;
            int lastRublesDigit = LastDigit(rublesAmount);
            int lastCopecksDigit = LastDigit(copecksAmount);

            string s = NumeralsToTxt(rublesAmount, TextCase.Nominative) + " ";

            if (IsPluralGenitive(lastRublesDigit))
            {
                s += rubles[3] + " ";
            }
            else if (IsSingularGenitive(lastRublesDigit))
            {
                s += rubles[2] + " ";
            }
            else
            {
                s += rubles[1] + " ";
            }

            s += convertCents ? NumeralsToTxt(copecksAmount, TextCase.Nominative, false, false) + " " : string.Format("{0:00} ", copecksAmount);

            if (IsPluralGenitive(lastCopecksDigit))
            {
                s += copecks[3] + " ";
            }
            else if (IsSingularGenitive(lastCopecksDigit))
            {
                s += copecks[2] + " ";
            }
            else
            {
                s += copecks[1] + " ";
            }

            return s.Trim();
        }

        string MakeText(int _digits, string[] _hundreds, string[] _tens, string[] _from3till19, string _second, string _first, string[] _power)
        {
            string s = "";
            int digits = _digits;

            if (digits >= 100)
            {
                s += _hundreds[digits / 100] + " ";
                digits %= 100;
            }
            if (digits >= 20)
            {
                s += _tens[digits / 10 - 1] + " ";
                digits %= 10;
            }

            if (digits >= 3)
            {
                s += _from3till19[digits - 2] + " ";
            }
            else if (digits == 2)
            {
                s += _second + " ";
            }
            else if (digits == 1)
            {
                s += _first + " ";
            }

            if (_digits != 0 && _power.Length > 0)
            {
                digits = LastDigit(_digits);

                if (IsPluralGenitive(digits))
                {
                    s += _power[3] + " ";
                }
                else if (IsSingularGenitive(digits))
                {
                    s += _power[2] + " ";
                }
                else
                {
                    s += _power[1] + " ";
                }
            }

            return s;
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
            string s = "";
            long number = _sourceNumber;
            int remainder;
            int power = 0;

            if ((number >= (long)Math.Pow(10, 15)) || number < 0)
            {
                throw new Exception("Данное число не поддерживается");
            }

            while (number > 0)
            {
                remainder = (int)(number % 1000);
                number /= 1000;

                switch (power)
                {
                    case 12:
                        s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, trillions) + s;
                        break;
                    case 9:
                        s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, billions) + s;
                        break;
                    case 6:
                        s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, millions) + s;
                        break;
                    case 3:
                        switch (_case)
                        {
                            case TextCase.Accusative:
                                s = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemaleAccusative, thousandsAccusative) + s;
                                break;
                            default:
                                s = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemale, thousands) + s;
                                break;
                        }
                        break;
                    default:
                        string[] powerArray = { };
                        switch (_case)
                        {
                            case TextCase.Genitive:
                                s = MakeText(remainder, hundredsGenetive, tensGenetive, from3till19Genetive, _isMale ? secondMaleGenetive : secondFemaleGenetive, _isMale ? firstMaleGenetive : firstFemale, powerArray) + s;
                                break;
                            case TextCase.Accusative:
                                s = MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemaleAccusative, powerArray) + s;
                                break;
                            default:
                                s = MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemale, powerArray) + s;
                                break;
                        }
                        break;
                }

                power += 3;
            }

            if (_sourceNumber == 0)
            {
                s = zero + " ";
            }

            if (s != "" && _firstCapital)
                s = s.Substring(0, 1).ToUpper() + s.Substring(1);

            return s.Trim();
        }
    }
}
