using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// Implements methods to use operators on <see cref="SObject"/> instances.
    /// </summary>
    class ObjectOperators
    {
        private static Tuple<double, double> GetNumericOperatorParameters(ScriptProcessor processor, SObject left, SObject right)
        {
            double numLeft, numRight;

            if (left is SNumber)
                numLeft = ((SNumber)left).Value;
            else
                numLeft = left.ToNumber(processor).Value;

            if (right is SNumber)
                numRight = ((SNumber)right).Value;
            else
                numRight = right.ToNumber(processor).Value;

            return new Tuple<double, double>(numLeft, numRight);
        }

        private static Tuple<bool, bool> GetBooleanicOperatorParameters(ScriptProcessor processor, SObject left, SObject right)
        {
            bool boolLeft, boolRight;

            if (left is SBool)
                boolLeft = ((SBool)left).Value;
            else
                boolLeft = left.ToBool(processor).Value;

            if (right is SBool)
                boolRight = ((SBool)right).Value;
            else
                boolRight = right.ToBool(processor).Value;

            return new Tuple<bool, bool>(boolLeft, boolRight);
        }

        internal static string AddOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            if (left is SString || right is SString)
            {
                string strLeft, strRight;

                if (left is SString)
                    strLeft = ((SString)left).Value;
                else
                    strLeft = left.ToString(processor).Value;

                if (right is SString)
                    strRight = ((SString)right).Value;
                else
                    strRight = right.ToString(processor).Value;

                return strLeft + strRight;
            }
            else
            {
                var numbers = GetNumericOperatorParameters(processor, left, right);

                return SNumber.ConvertToScriptString(numbers.Item1 + numbers.Item2);
            }
        }

        internal static string SubtractOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            return SNumber.ConvertToScriptString(numbers.Item1 - numbers.Item2);
        }

        internal static string MultiplyOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            return SNumber.ConvertToScriptString(numbers.Item1 * numbers.Item2);
        }

        internal static string DivideOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            if (numbers.Item2 == 0D) // Catch division by 0 by returning infinity (wtf -> what a terrible feature).
                return SObject.LITERAL_INFINITY;

            return SNumber.ConvertToScriptString(numbers.Item1 / numbers.Item2);
        }

        internal static string ModulusOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            if (numbers.Item2 == 0D) // Because, when we divide by 0, we get Infinity, but when we do (x % 0), we get NaN. Great.
                return SObject.LITERAL_NAN;

            return SNumber.ConvertToScriptString(numbers.Item1 % numbers.Item2);
        }

        internal static string ExponentOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            return SNumber.ConvertToScriptString(Math.Pow(numbers.Item1, numbers.Item2));
        }

        internal static string NotOperator(ScriptProcessor processor, SObject obj)
        {
            return SBool.ConvertToScriptString(!obj.ToBool(processor).Value);
        }

        internal static string IncrementOperator(ScriptProcessor processor, SObject obj)
        {
            // Only variables can be incremented:

            if (obj is SVariable)
            {
                var svar = (SVariable)obj;
                svar.Data = processor.CreateNumber(svar.Data.ToNumber(processor).Value + 1D);
                return svar.Identifier;
            }
            else
            {
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_INVALID_INCREMENT);
                return "";
            }
        }

        internal static string DecrementOperator(ScriptProcessor processor, SObject obj)
        {
            // Only variables can be decremented:

            if (obj is SVariable)
            {
                var svar = (SVariable)obj;
                svar.Data = processor.CreateNumber(svar.Data.ToNumber(processor).Value - 1D);
                return svar.Identifier;
            }
            else
            {
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_INVALID_DECREMENT);
                return "";
            }
        }

        internal static string EqualsOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            return SBool.ConvertToScriptString(ObjectComparer.LooseEquals(processor, left, right));
        }

        internal static string NotEqualsOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            return SBool.ConvertToScriptString(!ObjectComparer.LooseEquals(processor, left, right));
        }

        internal static string TypeEqualsOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            return SBool.ConvertToScriptString(ObjectComparer.StrictEquals(processor, left, right));
        }

        internal static string TypeNotEqualsOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            return SBool.ConvertToScriptString(!ObjectComparer.StrictEquals(processor, left, right));
        }

        internal static string SmallerOrEqualsOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            return SBool.ConvertToScriptString(numbers.Item1 <= numbers.Item2);
        }

        internal static string LargerOrEqualsOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            return SBool.ConvertToScriptString(numbers.Item1 >= numbers.Item2);
        }

        internal static string SmallerOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            return SBool.ConvertToScriptString(numbers.Item1 < numbers.Item2);
        }

        internal static string LargerOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var numbers = GetNumericOperatorParameters(processor, left, right);

            return SBool.ConvertToScriptString(numbers.Item1 > numbers.Item2);
        }

        internal static string OrOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var bools = GetBooleanicOperatorParameters(processor, left, right);

            return SBool.ConvertToScriptString(bools.Item1 || bools.Item2);
        }

        internal static string AndOperator(ScriptProcessor processor, SObject left, SObject right)
        {
            var bools = GetBooleanicOperatorParameters(processor, left, right);

            return SBool.ConvertToScriptString(bools.Item1 && bools.Item2);
        }
    }
}
