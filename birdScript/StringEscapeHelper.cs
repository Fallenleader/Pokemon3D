using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript
{
    /// <summary>
    /// A string escaper searching an expression from left to right.
    /// </summary>
    internal class LeftToRightStringEscapeHelper : StringEscapeHelper
    {
        bool _isEscaped;

        /// <summary>
        /// Creates a new instance of the <see cref="LeftToRightStringEscapeHelper"/> class, setting it to an index within the expression and ignoring every char in front of that index.
        /// </summary>
        internal LeftToRightStringEscapeHelper(string expression, int startIndex, bool ignoreStart) : base(expression)
        {
            if (ignoreStart)
            {
                _index = startIndex;
            }
            else
            {
                if (_hasStrings)
                {
                    while (startIndex > _index)
                    {
                        CheckNext();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LeftToRightStringEscapeHelper"/> class, setting searching up to a specific index in the expression.
        /// </summary>
        internal LeftToRightStringEscapeHelper(string expression, int startIndex) : this(expression, startIndex, false) { }

        internal override void CheckStartAt(int startIndex)
        {
            if (_hasStrings)
            {
                if (startIndex < _index)
                {
                    _index = 0;
                    CheckStartAt(startIndex);
                }
                else
                {
                    while (startIndex > _index)
                    {
                        CheckNext();
                    }
                    CheckNext();
                }
            }
        }

        protected override void CheckNext()
        {
            char t = _expression[_index];

            if (_endOfString)
            {
                _endOfString = false;
                _isString = false;
            }

            if (t == STRING_DELIMITER_SINGLE || t == STRING_DELIMITER_DOUBLE)
            {
                if (!_isString)
                {
                    _isString = true;
                    _startChar = t;
                }
                else
                {
                    if (!_isEscaped && t == _startChar)
                    {
                        _endOfString = true;
                    }
                    else
                    {
                        _isEscaped = false;
                    }
                }
            }
            else if (t == '\\')
            {
                _isEscaped = !_isEscaped;
            }
            else
            {
                if (_isEscaped)
                    _isEscaped = false;
            }

            _index++;
        }
    }

    /// <summary>
    /// A string escaper searching an expression from right to left.
    /// </summary>
    internal class RightToLeftStringEscapeHelper : StringEscapeHelper
    {

        /// <summary>
        /// Creates a new instance of the <see cref="RightToLeftStringEscapeHelper"/> class, setting it to an index within the expression and ignoring every char before that index.
        /// </summary>
        internal RightToLeftStringEscapeHelper(string expression, int startIndex, bool ignoreStart) : base(expression)
        {
            if (ignoreStart)
            {
                _index = startIndex;
            }
            else
            {
                if (_hasStrings)
                {
                    while (startIndex < _index)
                    {
                        CheckNext();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RightToLeftStringEscapeHelper"/> class, setting searching up to a specific index in the expression.
        /// </summary>
        internal RightToLeftStringEscapeHelper(string expression, int startIndex) : this(expression, startIndex, false) { }

        internal override void CheckStartAt(int startIndex)
        {
            if (_hasStrings)
            {
                if (startIndex > _index)
                {
                    _index = _expression.Length - 1;
                    CheckStartAt(startIndex);
                }
                else
                {
                    while (startIndex < _index)
                    {
                        CheckNext();
                    }
                    CheckNext();
                }
            }
        }

        protected override void CheckNext()
        {
            char t = _expression[_index];

            if (_endOfString)
            {
                _endOfString = false;
                _isString = false;
            }

            if (t == STRING_DELIMITER_SINGLE || t == STRING_DELIMITER_DOUBLE)
            {
                if (_isString && t == _startChar)
                {
                    int cIndex = _index - 1;
                    bool isEscaped = false;

                    while (cIndex >= 0 && _expression[cIndex] == '\\')
                    {
                        isEscaped = !isEscaped;
                        cIndex--;
                    }

                    if (!isEscaped)
                    {
                        _endOfString = true;
                    }
                }
                else if (!_isString)
                {
                    _isString = true;
                    _startChar = t;
                }
            }

            _index--;
        }
    }

    /// <summary>
    /// A base class for classes to implement searches for string delimiters and wrappers.
    /// </summary>
    internal abstract class StringEscapeHelper
    {
        protected internal const char STRING_DELIMITER_SINGLE = '\'';
        protected internal const char STRING_DELIMITER_DOUBLE = '\"';

        protected bool _isString = false;
        protected string _expression;
        protected int _index;
        protected char _startChar;
        protected bool _endOfString;
        protected bool _hasStrings;

        protected internal StringEscapeHelper(string expression)
        {
            _expression = expression;
            _hasStrings = HasStrings(expression);
        }

        /// <summary>
        /// Returns if the <see cref="StringEscapeHelper"/> is currently positioned within a string.
        /// </summary>
        internal bool IsString
        {
            get { return _isString; }
        }

        /// <summary>
        /// Jumps to the given index, ignoring all chars in between the current index and the new one.
        /// </summary>
        internal void JumpTo(int index)
        {
            _index = index;
        }

        /// <summary>
        /// Checks for string delimiters at the given index.
        /// </summary>
        internal abstract void CheckStartAt(int startIndex);
        protected abstract void CheckNext();

        protected static bool HasStrings(string expression)
        {
            return expression.Contains(STRING_DELIMITER_SINGLE) || expression.Contains(STRING_DELIMITER_DOUBLE);
        }

        /// <summary>
        /// Returns if an expression contains a value, disregarding content within string literals.
        /// </summary>
        internal static bool ContainsWithoutStrings(string expression, string value)
        {
            if (!expression.Contains(value))
            {
                return false;
            }
            else
            {
                if (!HasStrings(expression))
                {
                    return true;
                }
                else
                {
                    int index = 0;
                    bool escaped = false;
                    bool isString = false;
                    char startChar = STRING_DELIMITER_SINGLE;

                    StringBuilder sb = new StringBuilder();

                    while (index < expression.Length)
                    {
                        char t = expression[index];

                        if (t == STRING_DELIMITER_SINGLE || t == STRING_DELIMITER_DOUBLE)
                        {
                            if (!isString)
                            {
                                isString = true;
                                startChar = t;

                                // Append the starting delimiter:
                                sb.Append(t);
                            }
                            else
                            {
                                if (!escaped && t == startChar)
                                {
                                    isString = false;
                                }
                                else
                                {
                                    escaped = false;
                                }
                            }
                        }
                        else if (t == '\\')
                        {
                            escaped = !escaped;
                        }
                        else
                        {
                            if (escaped)
                                escaped = false;
                        }

                        if (!isString)
                            sb.Append(t);

                        index++;
                    }

                    return sb.ToString().Contains(value);
                }
            }
        }
    }
}
