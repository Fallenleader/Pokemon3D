using Microsoft.VisualStudio.TestTools.UnitTesting;
using birdScript.Adapters;
using birdScript.Types;
using birdScript;

namespace Test.birdScript.Adapters
{
    [TestClass()]
    public class ScriptInAdapterTests
    {
        [TestMethod()]
        public void StringTranslateTest()
        {
            string testString = "this is a test string!";
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testString);

            Assert.IsTrue(obj.GetType() == typeof(SString));
            Assert.AreEqual(testString, ((SString)obj).Value);
        }

        [TestMethod()]
        public void NumericTranslateTest()
        {
            double testDouble = -72.64;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testDouble);

            Assert.IsTrue(obj.GetType() == typeof(SNumber));
            Assert.AreEqual(testDouble, ((SNumber)obj).Value);
        }

        [TestMethod()]
        public void NullTranslateTest()
        {
            object nullObj = null;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, nullObj);

            Assert.IsTrue(obj.GetType() == typeof(SNull), "The type of obj is not SNull, but instead " + obj.GetType().Name);
        }

        [TestMethod()]
        public void UndefinedTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var undefined = processor.Undefined;

            var obj = ScriptInAdapter.Translate(processor, undefined);

            Assert.IsTrue(obj.GetType() == typeof(SUndefined));
        }

        [TestMethod()]
        public void BooleanTranslateTest()
        {
            bool testBool = true;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testBool);

            Assert.IsTrue(obj.GetType() == typeof(SBool));
            Assert.AreEqual(testBool, ((SBool)obj).Value);
        }

        [TestMethod()]
        public void ArrayTranslateTest()
        {
            string[] testArray = new string[] { "item1", "item2", "item3" };
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testArray);

            Assert.IsTrue(obj.GetType() == typeof(SArray));

            var arr = (SArray)obj;

            Assert.AreEqual(testArray.Length, arr.ArrayMembers.Length);
        }

        [TestMethod()]
        public void ObjectTranslateTest()
        {
            var processor = new ScriptProcessor();
            ScriptContextManipulator.AddPrototype(processor, typeof(Pokemon));

            processor.Run("var p = new Pokemon(); p.SetName(\"Pika\");");

            object objp = ScriptContextManipulator.GetVariableTranslated(processor, "p");

            Assert.IsTrue(objp.GetType() == typeof(Pokemon));

            Pokemon p = (Pokemon)objp;

            Assert.AreEqual("Pikachu", p.OriginalName);
            Assert.AreEqual("Pika", p.Name);
        }

        // Test class to create instances of.
        private class Pokemon
        {
            [ScriptVariable]
            public string Name = "Pikachu";
            [ScriptVariable]
            public string OriginalName = "";

            [ScriptFunction]
            public string SetName = "function(name) { if (this.OriginalName == \"\") { this.OriginalName = this.Name; } this.Name = name; }";
        }
    }
}