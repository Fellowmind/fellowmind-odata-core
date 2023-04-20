using System;
using Xunit;
using Fellowmind.OData.Core.Extensions;

namespace StringExtensionTester
{
    public class PluralizeTests
    {

        [Theory(DisplayName = "Check Automatic Pluralization")]
        [InlineData("Dog", "Dogs")]
        [InlineData("Opportunity", "Opportunities")]
        [InlineData("Fish", "Fishes")]
        [InlineData("Potato", "Potatoes")]
        [InlineData("Criterion", "Criteria")]
        [InlineData("Buzz", "Buzzes")]
        [InlineData("Fungus", "Fungi")]
        [InlineData("Axis", "Axes")]
        public void TestPluralize(string name, string expectedResult)
        {
            Assert.Equal(expectedResult, name.Pluralize());
        }

        [Theory(DisplayName = "Check Pluralization with explicit operator")]
        [InlineData("Opportunity", PluralOperation.YtoIES, "Opportunities")]
        [InlineData("Fish", PluralOperation.DoNothing, "Fish")]
        [InlineData("Potato", PluralOperation.OtoOES, "Potatoes")]
        [InlineData("Criterion", PluralOperation.ONtoA, "Criteria")]
        [InlineData("Buzz", PluralOperation.XtoES, "Buzzes")]
        [InlineData("Fungus", PluralOperation.UStoI, "Fungi")]
        [InlineData("Axis", PluralOperation.IStoES, "Axes")]
        [InlineData("Dog", PluralOperation.Auto, "Dogs")]
        [InlineData("People", PluralOperation.DoNothing, "People")]
        [InlineData("Bird", PluralOperation.NotDefined, "Birds")]
        [InlineData("Car", PluralOperation.Normal, "Cars")]
        public void TestPluralizeExplicit(string name, PluralOperation pluralOperation, string expectedResult)
        {
            Assert.Equal(expectedResult, name.Pluralize(pluralOperation));
        }

        [Theory(DisplayName = "Check Pluralization with explicit operator and designated result")]
        [InlineData("Child", PluralOperation.Irregular, "Children", "Children")]
        public void TestPluralizeIrregular(string name, PluralOperation pluralOperation, string irregular, string expectedResult)
        {
            Assert.Equal(expectedResult, name.Pluralize(pluralOperation, irregular));
        }

    }
}
