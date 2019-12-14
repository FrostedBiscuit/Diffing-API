using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using DiffingAPI.DiffLogic;

namespace DiffingAPI.Tests {

    [TestFixture]
    class UnitTests {

        [TestCase("kjiV9l+bKmk=", "lYlHmjcsEUs=", DiffResult.DiffResultType.ContentDoNotMatch)]
        [TestCase("kjiV9l+bKmk=", "kjiV9l+bKmk=", DiffResult.DiffResultType.Equals)]
        [TestCase("kjiV9l+bKmk=", "bO+CsQ==", DiffResult.DiffResultType.SizeDoNotMatch)]
        public void Diffing_Algorithm_Performs_Diff(string left, string right, DiffResult.DiffResultType expected) {

            List<DiffEntry> testDiff = new List<DiffEntry>();
            testDiff.Add(new DiffEntry(1, left, right));

            DiffManager.test_Init(testDiff);

            DiffResult result = DiffManager.Compare(1);

            Assert.AreEqual(expected, result.ResultType);
        }
    }
}
