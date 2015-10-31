using Microsoft.VisualStudio.TestTools.UnitTesting;
using birdScript.Adapters;
using birdScript.Types;
using birdScript;
using System;

namespace Test.birdScript.Adapters
{
    [TestClass]
    public class ScriptOutAdapterTests
    {
        [TestMethod]
        public void SStringTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            SString testString = processor.CreateString("this is a test string!");

            var obj = ScriptOutAdapter.Translate(testString);

            Assert.IsTrue(obj.GetType() == typeof(string));
            Assert.IsTrue((string)obj == testString.Value);
        }

        [TestMethod]
        public void SNumberTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            SNumber testNumber = processor.CreateNumber(-72.65);

            var obj = ScriptOutAdapter.Translate(testNumber);

            Assert.IsTrue(obj.GetType() == typeof(double));
            Assert.IsTrue((double)obj == testNumber.Value);
        }

        [TestMethod]
        public void SBoolTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            SBool testBool = processor.CreateBool(true);

            var obj = ScriptOutAdapter.Translate(testBool);

            Assert.IsTrue(obj.GetType() == typeof(bool));
            Assert.IsTrue((bool)obj == testBool.Value);
        }

        [TestMethod]
        public void SNullTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            SNull Null = (SNull)processor.Null;

            var obj = ScriptOutAdapter.Translate(Null);

            Assert.IsTrue(obj == null);
        }

        [TestMethod]
        public void SUndefinedTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            SUndefined Undefined = (SUndefined)processor.Undefined;

            var obj = ScriptOutAdapter.Translate(Undefined);

            Assert.IsTrue(obj is SUndefined);
        }

        [TestMethod]
        public void SArrayTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            SObject arrobj = processor.Context.CreateInstance("Array", new SObject[]
            {
                processor.CreateBool(true),
                processor.CreateString("test"),
                processor.CreateNumber(-1234.3)
            });

            Assert.IsTrue(arrobj is SArray);
            
            var obj = ScriptOutAdapter.Translate(arrobj);

            Assert.IsTrue(obj.GetType().IsArray);

            var arr = (Array)obj;

            Assert.IsTrue(arr.Length == 3);
        }
    }
}
