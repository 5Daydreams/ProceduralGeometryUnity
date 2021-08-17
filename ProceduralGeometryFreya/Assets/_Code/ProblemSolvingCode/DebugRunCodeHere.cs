using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace _Code.ProblemSolvingCode
{
    [CreateAssetMenu(fileName = "DebugProblemSolving", menuName = "Solving")]
    public class DebugRunCodeHere : ScriptableObject
    {
        public string _numberLuhn;
        public string _encoding;
        public int _height = 4;
        public int _base10Number = 4;

        private int _state = 0;

        [SerializeField] [TextArea(20, 1)] private string _output;

        [ContextMenu("Play Chap2 String-Geometry")]
        public void ExercisesChap2_NumberBases()
        {
            int sum = _base10Number / 2;

            while (_base10Number / 2 > 0)
            {
                sum *= 10;
                sum += _base10Number / 2;
            }
        }


        [ContextMenu("Play Chap2 String-Geometry")]
        public void TestChap2_StringGeometry()
        {
            string result = "";

            char hash = '*';
            char nl = '\n';

            for (int j = 1; j < _height; j++)
            {
                int width = _height;
                for (int i = 1; i < width; i++)
                {
                    // Generate a rhombus for "even" sizes (height = "N" yields an "N-1"-units-wide rhombus)
                    if (
                        i > j - _height/2 
                        && i > _height/2 - j 
                        && i < _height + _height/2 - j
                        && i < j + _height/2
                    )
                    {
                        result += hash;
                        result += ' ';
                    }
                    else
                    {
                        result += '-';
                        result += ' ';
                    }

                    // // Generate an hourglass
                    // if ((i >= j && i <= _height - j ) || ( i <= j && i >= _height-j))

                }

                result += nl;
            }

            _output = result;
        }


        [ContextMenu("Play Chap2 Decoding")]
        public void TestChap2_Decoding()
        {
            int[] stateModulos = new[] {27, 27, 9};
            int currentIndex = 0;
            string decodedMessage = "";
            _state = 0;

            _encoding += '\0';

            bool isLastCharacter = _encoding[currentIndex].Equals('\0');

            int sum = 0;

            while (!isLastCharacter)
            {
                bool invalidChar =
                    _encoding[currentIndex].Equals(',') || _encoding[currentIndex].Equals('\0');

                sum = 0;

                while (!invalidChar)
                {
                    sum *= 10;
                    int currentValue = Convert.ToInt32(_encoding[currentIndex]) - 48;
                    sum += currentValue;

                    currentIndex++;

                    invalidChar =
                        _encoding[currentIndex].Equals(',') || _encoding[currentIndex].Equals('\0');
                }

                int numberAfterModulo = sum % stateModulos[_state];

                if (numberAfterModulo < 0)
                {
                    throw new Exception("Modulo lower than zero");
                }

                if (numberAfterModulo == 0)
                {
                    _state = (_state + 1) % 3;
                }
                else
                {
                    decodedMessage += DecodeInteger(numberAfterModulo);
                }

                isLastCharacter = _encoding[currentIndex].Equals('\0');

                currentIndex++;
            }

            _encoding.Remove(_encoding.Length - 1);

            Debug.Log(decodedMessage);
            _output = decodedMessage;
        }

        private char DecodeInteger(int value)
        {
            char decodedChar = Convert.ToChar(value - 1);

            if (_state == 0)
                decodedChar += 'A';
            else if (_state == 1)
                decodedChar += 'a';
            else if (_state == 2)
            {
                switch (value)
                {
                    case 1:
                        decodedChar = '!';
                        break;
                    case 2:
                        decodedChar = '?';
                        break;
                    case 3:
                        decodedChar = ',';
                        break;
                    case 4:
                        decodedChar = '.';
                        break;
                    case 5:
                        decodedChar = ' ';
                        break;
                    case 6:
                        decodedChar = ';';
                        break;
                    case 7:
                        decodedChar = '"';
                        break;
                    case 8:
                        decodedChar = '\'';
                        break;
                    default:
                        break;
                }
            }

            return decodedChar;
        }


        [ContextMenu("Play Chap2 Luhn")]
        public void TestChap2_Luhn()
        {
            List<int> newNumber = new List<int>();

            // for (int i = 0; i < _numberLuhn.Length; i++)
            // {
            //     int value = Convert.ToInt32(_numberLuhn[i]) - 48;
            //
            //     value = GetDoubledSum(value);
            //
            //     newNumber.Add(value);
            // }


            int digitsType = _numberLuhn.Length % 2;

            for (int i = 0; i < _numberLuhn.Length; i++)
            {
                int value = Convert.ToInt32(_numberLuhn[i]) - 48;

                if (i % 2 != digitsType)
                    value = GetDoubledSum(value);

                newNumber.Add(value);
            }

            int sum = SumOfList(newNumber);

            int extraDigit = 0;

            while (sum % 10 != 0)
            {
                sum++;
                extraDigit++;
            }

            var result = _numberLuhn + extraDigit;

            Debug.Log(_numberLuhn + " --> " + result + " --> " + CheckLuhn(result));
        }

        private bool CheckLuhn(string luhnNumber)
        {
            int digitsType = (luhnNumber.Length - 1) % 2;

            int sum = 0;

            for (int i = 0; i < luhnNumber.Length - 1; i++)
            {
                int value = Convert.ToInt32(_numberLuhn[i]) - 48;

                if (i % 2 != digitsType)
                    value = GetDoubledSum(value);

                sum += value;
            }

            int lastDigitValue = Convert.ToInt32(luhnNumber[luhnNumber.Length - 1]) - 48;

            sum += lastDigitValue;

            if (sum % 10 == 0)
                return true;

            return false;
        }

        private int SumOfList(List<int> numbers)
        {
            int sum = 0;
            foreach (var value in numbers)
            {
                sum += value;
            }

            return sum;
        }

        private int GetDoubledSum(int value)
        {
            value *= 2;
            if (value / 10 > 0)
            {
                value = 1 + value % 10;
            }

            return value;
        }
    }
}