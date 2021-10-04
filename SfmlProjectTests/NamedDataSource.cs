using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SfmlProjectTests {
    abstract class NamedDataSource : Attribute, ITestDataSource {
        public abstract IEnumerable<object[]> GetData(MethodInfo methodInfo);

        public string GetDisplayName(MethodInfo methodInfo, object[] data) {
            return string.Format(CultureInfo.CurrentCulture, "{0} : {1}", methodInfo.Name, data[^1]);
        }
    }
}
