using System;
using System.Collections.Generic;
using MinaGlosor.Web.Models;
using NUnit.Framework;

namespace MinaGlosor.Test.Api.Domain
{
    [TestFixture]
    public class UserTest
    {
        private static IEnumerable<string> Valid
        {
            get
            {
                yield return "a-bc";
                yield return "aaaa";
                yield return "abc9";
                yield return "9abc";
            }
        }

        private static IEnumerable<string> Invalid
        {
            get
            {
                yield return "aaa-";
                yield return "-aaa";
                yield return "aaa--";
                yield return ".aaa";
                yield return "a.aa";
                yield return ".aaa";
                yield return "a.ba";
                yield return "9aa-";
                yield return "a9a.";
                yield return "9aa.";
            }
        }

        [TestCaseSource("Valid")]
        public void ValidUsername(string username)
        {
            Assert.DoesNotThrow(() => new User("e@d.com", "pwd", username));
        }

        [TestCaseSource("Invalid")]
        public void VerifiesUsername(string username)
        {
            Assert.Throws<ArgumentException>(() => new User("e@d.com", "pwd", username));
        }
    }
}