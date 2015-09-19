using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            Console.Title = "birdScript";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("birdScript V. 0.1");

            bool isRunning = true;
            var ev = new ScriptExpressionEvaluator();

            while (isRunning)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                for (int i = 0; i < Console.BufferWidth; i++)
                {
                    Console.Write("─");
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ◄ ");
                string input = Console.ReadLine();

                try
                {
                    SObject result = ev.eval(input);

                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (result is SVariable)
                    {
                        Console.WriteLine(" ► " + ((SVariable)result).Data.toScriptString());
                    }
                    else if (result is SAccessor)
                    {
                        Console.WriteLine(" ► " + ((SAccessor)result).getAccess().toScriptString());
                    }
                    else
                    {
                        Console.WriteLine(" ► " + result.toScriptString());
                    }
                }
                catch (ScriptExpressionEvaluator.ScriptException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" ▲ " + ex.Message);
                }
            }
        }
    }

    #region DataTypes

    public abstract class SObject
    {
        public virtual string toScriptString()
        {
            return "undefined";
        }

        public virtual SString toString()
        {
            return new SString("undefined");
        }

        public virtual SNumber toNumber()
        {
            return new SNumber(double.NaN);
        }

        public virtual SBool toBool()
        {
            return new SBool(true);
        }

        public virtual SString typeOf()
        {
            return new SString("object");
        }

        public virtual SNumber sizeOf()
        {
            return new SNumber(0D);
        }

        public virtual SObject getMember(SNumber accessor)
        {
            return new SUndefined();
        }

        public virtual void setMember(SNumber accessor, SObject obj)
        {
            //Empty.
        }

        public static SObject operator +(SObject left, SObject right)
        {
            if (left is SString || right is SString)
            {
                return new SString(left.toString().Value + right.toString().Value);
            }
            else
            {
                return new SNumber(left.toNumber().Value + right.toNumber().Value);
            }
        }

        public static SNumber operator -(SObject left, SObject right)
        {
            return new SNumber(left.toNumber().Value - right.toNumber().Value);
        }

        public static SNumber operator /(SObject left, SObject right)
        {
            double numLeft = left.toNumber().Value;
            double numRight = right.toNumber().Value;

            //Catch division by 0 by returning +Infinity.
            if (numRight == 0D)
                return new SNumber(double.PositiveInfinity);

            return new SNumber(left.toNumber().Value / right.toNumber().Value);
        }

        public static SNumber operator *(SObject left, SObject right)
        {
            return new SNumber(left.toNumber().Value * right.toNumber().Value);
        }

        public static SNumber operator %(SObject left, SObject right)
        {
            return new SNumber(left.toNumber().Value % right.toNumber().Value);
        }

        public static SBool operator !(SObject obj)
        {
            return new SBool(!(obj.toBool().Value));
        }

        public static SObject operator ++(SObject obj)
        {
            if (obj is SVariable)
            {
                ((SVariable)obj).Data = ((SVariable)obj).Data.toNumber() + new SNumber(1D);
                return obj;
            }
            else if (obj is SAccessor)
            {
                ((SAccessor)obj).setAccess(((SAccessor)obj).getAccess() + new SNumber(1D));
                return obj;
            }
            else
            {
                throw new ScriptExpressionEvaluator.SyntaxErrorException("invalid increment operand");
            }
        }

        public static SObject operator --(SObject obj)
        {
            if (obj is SVariable)
            {
                ((SVariable)obj).Data = ((SVariable)obj).Data.toNumber() - new SNumber(1D);
                return obj;
            }
            else if (obj is SAccessor)
            {
                ((SAccessor)obj).setAccess(((SAccessor)obj).getAccess() - new SNumber(1D));
                return obj;
            }
            else
            {
                throw new ScriptExpressionEvaluator.SyntaxErrorException("invalid decrement operand");
            }
        }

        public static SBool operator ==(SObject left, SObject right)
        {
            return new SBool(compare(left, right));
        }

        public static SBool operator !=(SObject left, SObject right)
        {
            return !(left == right);
        }

        public static SBool operator <=(SObject left, SObject right)
        {
            double numLeft = ((SNumber)left).Value;
            double numRight = ((SNumber)right).Value;

            return new SBool(numLeft <= numRight);
        }

        public static SBool operator >=(SObject left, SObject right)
        {
            double numLeft = ((SNumber)left).Value;
            double numRight = ((SNumber)right).Value;

            return new SBool(numLeft >= numRight);
        }

        public static SBool operator <(SObject left, SObject right)
        {
            double numLeft = ((SNumber)left).Value;
            double numRight = ((SNumber)right).Value;

            return new SBool(numLeft < numRight);
        }

        public static SBool operator >(SObject left, SObject right)
        {
            double numLeft = ((SNumber)left).Value;
            double numRight = ((SNumber)right).Value;

            return new SBool(numLeft > numRight);
        }

        public static SBool operator |(SObject left, SObject right)
        {
            bool boolLeft = left.toBool().Value;
            bool boolRight = right.toBool().Value;

            return new SBool(boolLeft | boolRight);
        }

        public static SBool operator &(SObject left, SObject right)
        {
            bool boolLeft = left.toBool().Value;
            bool boolRight = right.toBool().Value;

            return new SBool(boolLeft & boolRight);
        }

        private static bool compare(SObject left, SObject right)
        {
            if (left is SNull && !(right is SNull) || !(left is SNull) && right is SNull)
            {
                return false;
            }
            else if (left is SNull && right is SNull)
            {
                return true;
            }
            else if (left is SUndefined && !(right is SUndefined) || !(left is SUndefined) && right is SUndefined)
            {
                return false;
            }
            else if (left is SUndefined && right is SUndefined)
            {
                return true;
            }
            else if (left is SArray && right is SArray)
            {
                return SArray.compareArrays((SArray)left, (SArray)right);
            }
            else if (left is SString || right is SString)
            {
                string strLeft = left.toString().Value;
                string strRight = right.toString().Value;

                return strLeft == strRight;
            }
            else if (left is SNumber && right is SNumber)
            {
                double numLeft = ((SNumber)left).Value;
                double numRight = ((SNumber)right).Value;

                return numLeft == numRight;
            }
            else if (left is SBool && right is SBool)
            {
                return ((SBool)left).Value == ((SBool)right).Value;
            }
            else
            {
                bool boolLeft = left.toBool().Value;
                bool boolRight = right.toBool().Value;

                return boolLeft == boolRight;
            }
        }

        public static SBool typeEquals(SObject left, SObject right)
        {
            bool compareResult = compare(left, right);
            if (left.GetType() != right.GetType())
            {
                return SBool.FALSE;
            }
            return new SBool(compareResult);
        }

        public static SBool typeNotEquals(SObject left, SObject right)
        {
            return !typeEquals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is SObject)
                return compare(this, (SObject)obj);
            else
                return false;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }

    public abstract class SObjectT<T> : SObject
    {
        protected T _val;

        public SObjectT(T val)
        {
            _val = val;
        }

        public T Value
        {
            get { return _val; }
            set { _val = value; }
        }
    }

    public class SNull : SObject
    {
        public override string toScriptString()
        {
            return "null";
        }

        public override SString toString()
        {
            return new SString("null");
        }

        public override SNumber toNumber()
        {
            return new SNumber(0D);
        }

        public override SBool toBool()
        {
            return SBool.FALSE;
        }
    }

    public class SUndefined : SObject
    {
        public override SBool toBool()
        {
            return new SBool(false);
        }

        public override SString typeOf()
        {
            return new SString("undefined");
        }
    }

    public class SEmptyArrayElement : SObject
    {
        //Represents an empty array element.
        //When accessing it, it will get converted into an SUndefined instance.

        public override string toScriptString()
        {
            return "";
        }
    }

    public class SVariable : SObject
    {
        public string Identifier;
        public SObject Data;

        public SVariable(string identifier)
        {
            Identifier = identifier;
            Data = new SUndefined();
        }

        public SVariable(string identifier, SObject data)
        {
            Identifier = identifier;
            Data = data;
        }

        public override string toScriptString()
        {
            return Identifier;
        }

        public override SString toString()
        {
            return Data.toString();
        }

        public override SNumber toNumber()
        {
            return Data.toNumber();
        }

        public override SBool toBool()
        {
            return Data.toBool();
        }

        public override SString typeOf()
        {
            return Data.typeOf();
        }

        public override SNumber sizeOf()
        {
            return Data.sizeOf();
        }

        public override SObject getMember(SNumber accessor)
        {
            return Data.getMember(accessor);
        }

        public override void setMember(SNumber accessor, SObject obj)
        {
            Data.setMember(accessor, obj);
        }
    }

    public class SAccessor : SObject
    {
        public SObject Target;
        public SNumber Accessor;

        public SAccessor(SObject target, SNumber accessor)
        {
            Target = target;
            Accessor = accessor;
        }

        public SObject getAccess()
        {
            return Target.getMember(Accessor);
        }

        public void setAccess(SObject obj)
        {
            Target.setMember(Accessor, obj);
        }

        public override SString typeOf()
        {
            return getAccess().typeOf();
        }

        public override SNumber sizeOf()
        {
            return getAccess().sizeOf();
        }

        public override SBool toBool()
        {
            return getAccess().toBool();
        }

        public override SString toString()
        {
            return getAccess().toString();
        }

        public override SNumber toNumber()
        {
            return getAccess().toNumber();
        }

        public override SObject getMember(SNumber accessor)
        {
            return getAccess().getMember(accessor);
        }

        public override void setMember(SNumber accessor, SObject obj)
        {
            getAccess().setMember(accessor, obj);
        }

        public override string toScriptString()
        {
            return Target.toScriptString() + "[" + Accessor.toScriptString() + "]";
        }
    }

    public class SString : SObjectT<string>
    {
        public SString(string val)
            : base(val)
        { }

        public override string toScriptString()
        {
            return "\"" + _val + "\"";
        }

        public override SString toString()
        {
            return new SString(_val);
        }

        public override SNumber toNumber()
        {
            if (_val == "")
                return new SNumber(0D);

            double dblResult = 0D;
            if (double.TryParse(_val, out dblResult))
                return new SNumber(dblResult);
            else
                return new SNumber(double.NaN);
        }

        public override SBool toBool()
        {
            if (_val == "")
            {
                return SBool.FALSE;
            }
            else
            {
                return SBool.TRUE;
            }
        }

        public override SString typeOf()
        {
            return new SString("string");
        }

        public override SNumber sizeOf()
        {
            return new SNumber(_val.Length);
        }

        public override SObject getMember(SNumber accessor)
        {
            if (accessor.Value == double.NaN)
            {
                return new SUndefined();
            }

            int index = (int)accessor.Value;

            if (index < _val.Length && index >= 0)
            {
                return new SString(_val[index].ToString());
            }
            else
            {
                return new SUndefined();
            }
        }
    }

    public class SNumber : SObjectT<double>
    {
        public SNumber(double val)
            : base(val)
        { }

        public override string toScriptString()
        {
            return _val.ToString();
        }

        public override SString toString()
        {
            return new SString(toScriptString());
        }

        public override SNumber toNumber()
        {
            return new SNumber(_val);
        }

        public override SBool toBool()
        {
            if (_val == 0D)
            {
                return SBool.FALSE;
            }
            else
            {
                return SBool.TRUE;
            }
        }

        public override SString typeOf()
        {
            return new SString("number");
        }

        public override SNumber sizeOf()
        {
            return new SNumber(_val);
        }
    }

    public class SBool : SObjectT<bool>
    {
        public SBool(bool val)
            : base(val)
        { }

        public static SBool FALSE
        {
            get { return new SBool(false); }
        }

        public static SBool TRUE
        {
            get { return new SBool(true); }
        }

        public void Reverse()
        {
            _val = !_val;
        }

        public override string toScriptString()
        {
            if (_val)
                return "true";
            else
                return "false";
        }

        public override SString toString()
        {
            return new SString(toScriptString());
        }

        public override SNumber toNumber()
        {
            if (_val)
                return new SNumber(1D);
            else
                return new SNumber(0D);
        }

        public override SBool toBool()
        {
            return new SBool(_val);
        }

        public override SString typeOf()
        {
            return new SString("bool");
        }

        public override SNumber sizeOf()
        {
            return new SNumber(1D);
        }
    }

    public class SArray : SObjectT<SObject[]>
    {
        public SArray(SObject[] val)
         : base(val)
        { }

        public static SArray parse(string exp, ScriptExpressionEvaluator ev)
        {
            //This parses a string literal containing an array to SArray.
            //exp has the format "{ <data> }".

            List<SObject> elements = new List<SObject>();

            //Remove { and }:
            exp = exp.Remove(exp.Length - 1, 1).Remove(0, 1);

            string currentElement = string.Empty;
            bool isString = false;
            int arrayDepth = 0;

            for (int i = 0; i < exp.Length; i++)
            {
                var t = exp[i];

                if (t == '\"')
                {
                    isString = !isString;
                }
                if (!isString)
                {
                    if (t == '{')
                    {
                        arrayDepth++;
                    }
                    else if (t == '}')
                    {
                        arrayDepth--;
                    }
                }

                if (!isString && arrayDepth == 0)
                {
                    if (t == ',')
                    {
                        elements.Add(ev.eval(currentElement));
                        currentElement = "";
                    }
                    else
                    {
                        currentElement += t.ToString();
                    }
                }
                else
                {
                    currentElement += t.ToString();
                }
            }

            if (currentElement.Length > 0)
                elements.Add(ev.eval(currentElement));

            return new SArray(elements.ToArray());
        }

        public override string toScriptString()
        {
            string str = "";

            for (int i = 0; i < _val.Length; i++)
            {
                if (i > 0)
                    str += ",";

                str += _val[i].toScriptString();
            }

            return "{" + str + "}";
        }

        public override SString toString()
        {
            //This converts an array into its string representation.

            string str = "";

            for (int i = 0; i < _val.Length; i++)
            {
                if (i > 0)
                    str += ",";

                //We add the string representation of this object to the result.
                if (_val[i] is SEmptyArrayElement)
                {
                    str += "";
                }
                else
                {
                    str += _val[i].toString().Value;
                }
            }

            return new SString(str);
        }

        public override SNumber toNumber()
        {
            if (_val.Length == 0)
            {
                return new SNumber(0D);
            }
            else if (_val.Length == 1)
            {
                return _val[0].toNumber();
            }
            else
            {
                return new SNumber(double.NaN);
            }
        }

        public override SBool toBool()
        {
            return new SBool(true);
        }

        public override SString typeOf()
        {
            return new SString("array");
        }

        public override SNumber sizeOf()
        {
            return new SNumber(_val.Length);
        }

        public override SObject getMember(SNumber accessor)
        {
            if (accessor.Value == double.NaN)
            {
                return new SUndefined();
            }

            int index = (int)accessor.Value;

            if (index < _val.Length && index >= 0)
            {
                if (_val[index] is SEmptyArrayElement)
                {
                    return new SUndefined();
                }
                else
                {
                    return _val[index];
                }
            }
            else
            {
                return new SUndefined();
            }
        }

        public override void setMember(SNumber accessor, SObject obj)
        {
            if (accessor.Value != double.NaN)
            {
                int index = (int)accessor.Value;

                if (index < _val.Length && index >= 0)
                {
                    _val[index] = obj;
                }
                else if (index > 0)
                {
                    int firstIndex = _val.Length;

                    Array.Resize(ref _val, index + 1);

                    for (int i = firstIndex; i < _val.Length - 1; i++)
                        _val[i] = new SEmptyArrayElement();

                    _val[index] = obj;
                }
            }
        }

        public static bool compareArrays(SArray left, SArray right)
        {
            if (left.Value.Length != right.Value.Length)
            {
                return false;
            }
            else if (left.Value.Length == 1)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < left.Value.Length; i++)
                {
                    if (!(left.Value[i] == right.Value[i]).Value)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }

    public class SFunction : SObject
    {
        public string body;
        public string[] parameters;

        private ScriptExpressionEvaluator _ev;

        public SFunction(string body, string[] parameters, ScriptExpressionEvaluator ev)
        {
            this.body = body.Remove(body.Length - 1, 1).Remove(0, 1); //Removing { and }.
            this.parameters = parameters;

            _ev = ev;
        }

        public static SFunction parse(string exp, ScriptExpressionEvaluator ev)
        {
            exp = exp.Remove(0, "function~".Length);
            string[] parameters = exp.Remove(exp.IndexOf("{")).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string body = exp.Remove(0, exp.IndexOf("{"));

            return new SFunction(body, parameters, ev);
        }

        public override string toScriptString()
        {
            return "function~" + string.Join(",", parameters) + "{" + body + "}";
        }

        public override SString typeOf()
        {
            return new SString("function");
        }

        public override SNumber sizeOf()
        {
            return new SNumber(1D);
        }

        public override SBool toBool()
        {
            return new SBool(true);
        }

        public override SNumber toNumber()
        {
            return new SNumber(double.NaN);
        }

        public override SString toString()
        {
            return new SString(toScriptString());
        }

        public SObject call(SArray args)
        {
            List<Tuple<string, SObject>> largs = new List<Tuple<string, SObject>>();
            for (int i = 0; i < parameters.Length; i++)
            {
                largs.Add(new Tuple<string, SObject>(parameters[i], args.getMember(new SNumber(i))));
            }

            return _ev.evalWithParams(body, largs.ToArray());
        }
    }

    public class SIfStruct : SObject
    {
        public static SIfStruct parse(string exp, ScriptExpressionEvaluator ev)
        {
            //Input's like this:
            //if~{condition}{ifstruct}

            return null;
        }
    }

    #endregion

    public class ScriptExpressionEvaluator
    {
        public Dictionary<string, SVariable> Variables = new Dictionary<string, SVariable>();

        /// <summary>
        /// The chars where the identifier search stops at.
        /// </summary>
        private static char[] identifierSeperators = "-+*/=!%&|<>".ToCharArray();

        public SObject evalWithParams(string exp, Tuple<string, SObject>[] args)
        {
            //this method adds temporary variables and removes them right after.
            for (int i = 0; i < args.Length; i++)
            {
                Variables.Add(args[i].Item1, new SVariable(args[i].Item1, args[i].Item2));
            }

            SObject evalResult = eval(exp);

            //At the end of the execution, remove all variables again:
            for (int i = 0; i < args.Length; i++)
            {
                Variables.Remove(args[i].Item1);
            }

            return evalResult;
        }

        public SObject eval(string exp)
        {
            exp = removeWhitespace(exp);

            string[] expressions = splitExpressions(exp);

            for (int i = 0; i < expressions.Length - 1; i++)
                eval(expressions[i]);

            exp = expressions.Last();

            exp = evalBrackets(exp, '[', ']');
            exp = evalBrackets(exp, '(', ')');

            // ++
            exp = operatorLeftToRight(exp, "++");
            // --
            exp = operatorLeftToRight(exp, "--");
            // !
            exp = operatorRightToLeft(exp, "!");
            // *
            exp = operatorLeftToRight(exp, "*");
            // /
            exp = operatorLeftToRight(exp, "/");
            // %
            exp = operatorLeftToRight(exp, "%");
            // +
            exp = operatorLeftToRight(exp, "+");
            // -
            exp = operatorLeftToRight(exp, "-");
            // <=
            exp = operatorLeftToRight(exp, "<=");
            // >=
            exp = operatorLeftToRight(exp, ">=");
            // <
            exp = operatorLeftToRight(exp, "<");
            // >
            exp = operatorLeftToRight(exp, ">");
            // ===
            exp = operatorLeftToRight(exp, "===");
            // !==
            exp = operatorLeftToRight(exp, "!==");
            // ==
            exp = operatorLeftToRight(exp, "==");
            // !=
            exp = operatorLeftToRight(exp, "!=");
            // &&
            exp = operatorLeftToRight(exp, "&&");
            // ||
            exp = operatorLeftToRight(exp, "||");
            // =
            exp = operatorRightToLeft(exp, "=");

            var sReturn = toScriptObject(exp);

            if (sReturn is SIfStruct)
            {

            }

            return sReturn;
        }

        private string evalBrackets(string exp, char cOpen, char cClose)
        {
            //This evaluates the brackets and returns the script string afterwards.

            int openCount = 0;
            int startIndex = -1;
            int closeIndex = -1;

            bool isString = false;
            int openedBrackets = 0;

            //This means that when ( and ) is used, they might be function parameter delimiter.
            bool doFunction = cOpen == '(' &&
                              cClose == ')';

            bool keepBracketChars = cOpen != '(' ||
                                    cClose != ')';

            for (int i = 0; i < exp.Length; i++)
            {
                char t = exp[i];

                if (t == '\"')
                {
                    isString = !isString;
                }
                else if (t == '}')
                {
                    openedBrackets++;
                }
                else if (t == '{')
                {
                    openedBrackets--;
                }
                else if (t == cOpen && !isString && openedBrackets == 0)
                {
                    if (openCount == 0)
                    {
                        startIndex = i;
                    }
                    openCount++;
                }
                else if (t == cClose && !isString && openedBrackets == 0)
                {
                    if (openCount == 1 && startIndex > -1)
                    {
                        closeIndex = i;

                        string caller = getElementLeft(exp, startIndex - 1);

                        //When we can do a function, check if we have a valid caller.
                        if (doFunction && caller.Length > 0)
                        {
                            SObject functionEval = evalFunction(caller, exp.Substring(startIndex + 1, closeIndex - startIndex - 1));
                            string evalScriptString = functionEval.toScriptString();

                            startIndex = startIndex - caller.Length;

                            exp = exp.Remove(startIndex, closeIndex - startIndex + 1);
                            exp = exp.Insert(startIndex, evalScriptString);

                            i = startIndex + evalScriptString.Length - 1;
                        }
                        //Otherwise, just replace the expression with the evaluated result.
                        else
                        {
                            SObject bracketEval = eval(exp.Substring(startIndex + 1, closeIndex - startIndex - 1));
                            string evalScriptString = bracketEval.toScriptString();

                            if (keepBracketChars)
                            {
                                exp = exp.Remove(startIndex + 1, closeIndex - startIndex - 1);
                                exp = exp.Insert(startIndex + 1, evalScriptString);

                                i = startIndex + evalScriptString.Length + 1;
                            }
                            else
                            {
                                exp = exp.Remove(startIndex, closeIndex - startIndex + 1);
                                exp = exp.Insert(startIndex, evalScriptString);

                                i = startIndex + evalScriptString.Length - 1;
                            }
                        }

                        startIndex = -1;
                        closeIndex = -1;
                    }

                    openCount--;
                }
            }

            return exp;
        }

        #region Operators

        private string operatorRightToLeft(string exp, string op)
        {
            int[] ops = getOperatorPositionsRightToLeft(exp, op);

            for (int i = 0; i < ops.Length; i++)
            {
                var cOp = ops[i];

                SObject result = null;

                if (op == "!")
                {
                    string elementRight = getElementRight(exp, cOp + op.Length);

                    if (elementRight.Length > 0)
                    {
                        SObject objRight = toScriptObject(elementRight);
                        result = !objRight;

                        exp = exp.Remove(cOp, elementRight.Length + op.Length);
                        exp = exp.Insert(cOp, result.toScriptString());
                    }
                }
                else if (op == "=")
                {
                    string elementRight = getElementRight(exp, cOp + op.Length);
                    SObject objRight = toScriptObject(elementRight);
                    string elementLeft = getElementLeft(exp, cOp - 1);
                    SObject objLeft = toScriptObject(elementLeft);

                    SObject target = objLeft;
                    SObject data = objRight;

                    if (data is SAccessor)
                    {
                        data = ((SAccessor)data).getAccess();
                    }

                    if (target is SVariable)
                    {
                        ((SVariable)target).Data = data;
                    }
                    else if (target is SAccessor)
                    {
                        ((SAccessor)target).setAccess(data);
                    }
                    else
                    {
                        throw new ReferenceErrorException("invalid assignment left-hand side");
                    }

                    result = target;

                    exp = exp.Remove(cOp - elementLeft.Length, elementLeft.Length + elementRight.Length + op.Length);
                    exp = exp.Insert(cOp - elementLeft.Length, result.toScriptString());
                }
            }

            return exp;
        }

        private string operatorLeftToRight(string exp, string op)
        {
            int[] ops = getOperatorPositionsLeftToRight(exp, op);

            for (int i = 0; i < ops.Length; i++)
            {
                var cOp = ops[i];

                bool needRight = true;

                string elementLeft = "", elementRight = "";
                SObject objLeft = null, objRight = null, result = null;

                if (op == "++" || op == "--")
                    needRight = false;

                elementLeft = getElementLeft(exp, cOp - 1);

                if (!(op == "-" && elementLeft.Length == 0)) //Prevents negative number notation from causing bugs.
                {
                    objLeft = toScriptObject(elementLeft);

                    if (needRight)
                    {
                        elementRight = getElementRight(exp, cOp + op.Length);
                        objRight = toScriptObject(elementRight);
                    }

                    switch (op)
                    {
                        case "+":
                            result = objLeft + objRight;
                            break;
                        case "-":
                            result = objLeft - objRight;
                            break;
                        case "*":
                            result = objLeft * objRight;
                            break;
                        case "/":
                            result = objLeft / objRight;
                            break;
                        case "%":
                            result = objLeft % objRight;
                            break;
                        case "==":
                            result = objLeft == objRight;
                            break;
                        case "===":
                            result = SObject.typeEquals(objLeft, objRight);
                            break;
                        case "!==":
                            result = SObject.typeNotEquals(objLeft, objRight);
                            break;
                        case "!=":
                            result = objLeft != objRight;
                            break;
                        case "<=":
                            result = objLeft <= objRight;
                            break;
                        case ">=":
                            result = objLeft >= objRight;
                            break;
                        case "<":
                            result = objLeft < objRight;
                            break;
                        case ">":
                            result = objLeft > objRight;
                            break;
                        case "&&":
                            result = objLeft & objRight;
                            break;
                        case "||":
                            result = objLeft | objRight;
                            break;
                        case "++":
                            result = objLeft++;
                            break;
                        case "--":
                            result = objLeft--;
                            break;
                    }

                    string resultScript = result.toScriptString();

                    exp = exp.Remove(cOp - elementLeft.Length, elementLeft.Length + elementRight.Length + op.Length);
                    exp = exp.Insert(cOp - elementLeft.Length, resultScript);

                    var offset = resultScript.Length - (elementLeft.Length + elementRight.Length + op.Length);
                    for (int j = i + 1; j < ops.Length; j++)
                    {
                        ops[j] += offset;
                    }
                }
            }

            return exp;
        }

        #endregion

        #region ScriptSearch

        private int[] getOperatorPositionsRightToLeft(string exp, string op)
        {
            //This returns operator positions right-to-left.
            //Just get them left to right and reverse the order:
            return getOperatorPositionsLeftToRight(exp, op).Reverse().ToArray();
        }

        private int[] getOperatorPositionsLeftToRight(string exp, string op)
        {
            //This returns operator positions left-to-right.
            List<int> operators = new List<int>();

            bool isString = false;
            int openedBrackets = 0;

            for (int i = 0; i < exp.Length; i++)
            {
                var t = exp[i];

                if (t == '\"')
                {
                    isString = !isString;
                }
                else
                {
                    if (!isString)
                    {
                        if (t == ')' || t == '}')
                        {
                            openedBrackets--;
                        }
                        else if (t == '(' || t == '{')
                        {
                            openedBrackets++;
                        }

                        if (t == op[0] && openedBrackets == 0)
                        {
                            if (op.Length == 2)
                            {
                                if (i + 1 < exp.Length)
                                    if (exp[i + 1] == op[1])
                                        operators.Add(i);
                            }
                            else if (op.Length == 3)
                            {
                                if (i + 1 < exp.Length)
                                    if (exp[i + 1] == op[1])
                                        if (i + 2 < exp.Length)
                                            if (exp[i + 2] == op[2])
                                                operators.Add(i);
                            }
                            else
                            {
                                operators.Add(i);
                            }
                        }
                    }
                }
            }

            return operators.ToArray();
        }

        private string getElementLeft(string exp, int position)
        {
            string identifier = "";

            bool foundSepChar = false;
            bool isString = false;
            int openedBrackets = 0;

            while (position >= 0 && !foundSepChar)
            {
                char t = exp[position];

                if (t == '\"')
                {
                    isString = !isString;
                }

                if (!isString)
                {
                    if (t == ')' || t == '}')
                    {
                        openedBrackets++;
                    }
                    else if (t == '(' || t == '{')
                    {
                        openedBrackets--;
                    }
                }

                if (identifierSeperators.Contains(t) && openedBrackets == 0 && !isString || t == ',' && openedBrackets == 0 && !isString)
                {
                    foundSepChar = true;
                }
                else
                {
                    identifier += t.ToString();
                }

                position--;
            }

            if (position >= -1 && exp[position + 1] == '-' && identifier.Length > 0)
            {
                identifier += "-";
            }

            return new string(identifier.Reverse().ToArray());
        }

        private string getElementRight(string exp, int position)
        {
            string identifier = "";

            bool foundSepChar = false;
            bool isString = false;
            int openedBrackets = 0;

            while (position < exp.Length && !foundSepChar)
            {
                char t = exp[position];

                if (t == '\"')
                {
                    isString = !isString;
                }

                if (!isString)
                {
                    if (t == ')' || t == '}')
                    {
                        openedBrackets--;
                    }
                    else if (t == '(' || t == '{')
                    {
                        openedBrackets++;
                    }
                }

                if (t == '-' && identifier.Length == 0)
                {
                    identifier += "-";
                }
                else
                {
                    if (identifierSeperators.Contains(t) && openedBrackets == 0 && !isString || t == ',' && openedBrackets == 0 && !isString)
                    {
                        foundSepChar = true;
                    }
                    else
                    {
                        identifier += t.ToString();
                    }
                }

                position++;
            }

            return identifier;
        }

        #endregion

        #region Helper

        public SObject toScriptObject(string exp)
        {
            //This converts a script identifier or value into its object representation.
            double dblResult = 0D;

            if (exp == "null")
            {
                return new SNull();
            }
            else if (exp == "undefined")
            {
                return new SUndefined();
            }
            else if (exp == "false")
            {
                return new SBool(false);
            }
            else if (exp == "true")
            {
                return new SBool(true);
            }
            else if (double.TryParse(exp, out dblResult))
            {
                return new SNumber(dblResult);
            }
            else if (Variables.Keys.Contains(exp))
            {
                return Variables[exp];
            }
            else if (exp.StartsWith("\"") && exp.EndsWith("\""))
            {
                return new SString(exp.Remove(exp.Length - 1).Remove(0, 1));
            }
            else if (exp.StartsWith("{") && exp.EndsWith("}"))
            {
                return SArray.parse(exp, this);
            }
            else if (exp.EndsWith("]"))
            {
                return parseAccessorExpression(exp);
            }
            else if (exp.StartsWith("var~"))
            {
                return newVariable(exp);
            }
            else if (exp.StartsWith("function~"))
            {
                return SFunction.parse(exp, this);
            }
            else if (exp.StartsWith("if~"))
            {
                return SIfStruct.parse(exp, this);
            }
            
            //expression was not defined, throw error:
            throw new ReferenceErrorException(exp + " is not defined");
        }

        private SVariable newVariable(string exp)
        {
            //Remove "var~"
            exp = exp.Remove(0, 4);

            if (Variables.Keys.Contains(exp))
            {
                throw new ReferenceErrorException(exp + " is already defined");
            }
            else
            {
                Variables.Add(exp, new SVariable(exp));
                return Variables[exp];
            }
        }

        private SObject parseAccessorExpression(string exp)
        {
            int accessorStart = -1;
            bool isString = false;
            int openedBrackets = 0;

            for (int i = exp.Length - 1; i >= 0; i--)
            {
                //We have not yet found where the accessor starts:
                if (accessorStart == -1)
                {
                    char t = exp[i];

                    if (t == '\"')
                    {
                        isString = !isString;
                    }
                    else
                    {
                        if (!isString)
                        {
                            if (t == '{' || t == '(')
                            {
                                openedBrackets--;
                            }
                            else if (t == '}' || t == ')')
                            {
                                openedBrackets++;
                            }
                            else if (t == '[' && openedBrackets == 0)
                            {
                                accessorStart = i;
                            }
                        }
                    }
                }
            }

            string accessorStr = exp.Remove(0, accessorStart + 1);
            accessorStr = accessorStr.Remove(accessorStr.Length - 1, 1);

            string objStr = exp.Remove(accessorStart);

            SObject accessorObj = toScriptObject(accessorStr);
            SObject obj = toScriptObject(objStr);

            if (accessorObj is SNumber)
            {
                return new SAccessor(obj, (SNumber)accessorObj);
                //return obj.getMember((SNumber)accessorObj);
            }
            else
            {
                return new SUndefined();
            }
        }

        private string removeWhitespace(string exp)
        {
            //Removes all whitespace outside of string literals.
            string newExp = "";
            bool isString = false;
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '\"')
                {
                    isString = !isString;
                    newExp += exp[i];
                }
                else if (exp[i] != ' ' || isString)
                {
                    newExp += exp[i];
                }
            }
            return newExp;
        }

        private string[] splitExpressions(string exp)
        {
            List<string> expressions = new List<string>();
            bool isString = false;
            int openBrackets = 0;

            int lastExpStart = 0;

            //Split the expression into single statements at the ";" character:
            if (exp.Contains(";"))
            {
                for (int i = 0; i < exp.Length; i++)
                {
                    char t = exp[i];
                    if (t == '\"')
                    {
                        isString = !isString;
                    }
                    else if (t == '{')
                    {
                        openBrackets++;
                    }
                    else if (t == '}')
                    {
                        openBrackets--;
                    }
                    else
                    {
                        if (!isString && t == ';' && openBrackets == 0)
                        {
                            expressions.Add(exp.Substring(lastExpStart, i - lastExpStart));
                            lastExpStart = i + 1;
                        }
                    }
                }

                if (exp.Last() != ';')
                {
                    expressions.Add(exp.Substring(lastExpStart, (exp.Length) - lastExpStart));
                }
            }
            else
            {
                expressions.Add(exp);
            }

            return expressions.ToArray();
        }

        #endregion

        #region Exceptions

        public abstract class ScriptException : Exception
        {
            public ScriptException(string message)
                : base(message)
            { }
        }

        public class ReferenceErrorException : ScriptException
        {
            public ReferenceErrorException(string message)
                : base("ReferenceError: " + message)
            { }
        }

        public class SyntaxErrorException : ScriptException
        {
            public SyntaxErrorException(string message)
                : base("SyntaxError: " + message)
            { }
        }

        #endregion

        #region Functions

        private SObject evalFunction(string caller, string argsStr)
        {
            SArray args = SArray.parse("{" + argsStr + "}", this);

            SObject func = toScriptObject(caller);

            if (func is SVariable)
                func = ((SVariable)func).Data;
            if (func is SAccessor)
                func = ((SAccessor)func).getAccess();

            return ((SFunction)func).call(args);
        }

        #endregion
    }
}