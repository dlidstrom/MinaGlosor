using System;
using System.Collections.Generic;
using MinaGlosor.Web.Models;
using NUnit.Framework;

namespace MinaGlosor.Test.Api.Domain
{
    [TestFixture]
    public class PracticeSessionStatisticsTest
    {
        private static IEnumerable<Tuple<int, int, int, int, int, int>> Numbers
        {
            get
            {
                yield return Tuple.Create(1, 0, 0, 10, 0, 0);
                yield return Tuple.Create(1, 1, 0, 10, 10, 0);
            }
        }

        private static IEnumerable<Tuple<int, int, int, int, int, int>> NumbersAll
        {
            get
            {
                yield return Tuple.Create(3, 4, 0, 30, 40, 0);
                yield return Tuple.Create(3, 0, 4, 30, 0, 40);
            }
        }

        [TestCaseSource("Numbers")]
        public void VerifyGreenUnfinished(Tuple<int, int, int, int, int, int> tuple)
        {
            var statistics = new PracticeSessionStatistics(false, 10, tuple.Item1, tuple.Item2, tuple.Item3);

            Assert.That(statistics.Green, Is.EqualTo(tuple.Item4));
        }

        [TestCaseSource("Numbers")]
        public void VerifyBlueUnfinished(Tuple<int, int, int, int, int, int> tuple)
        {
            var statistics = new PracticeSessionStatistics(false, 10, tuple.Item1, tuple.Item2, tuple.Item3);

            Assert.That(statistics.Blue, Is.EqualTo(tuple.Item5));
        }

        [TestCaseSource("Numbers")]
        public void VerifyYellowUnfinished(Tuple<int, int, int, int, int, int> tuple)
        {
            var statistics = new PracticeSessionStatistics(false, 10, tuple.Item1, tuple.Item2, tuple.Item3);

            Assert.That(statistics.Yellow, Is.EqualTo(tuple.Item6));
        }

        [TestCaseSource("NumbersAll")]
        public void VerifyGreenFinished(Tuple<int, int, int, int, int, int> tuple)
        {
            var statistics = new PracticeSessionStatistics(false, 10, tuple.Item1, tuple.Item2, tuple.Item3);

            Assert.That(statistics.Green, Is.EqualTo(tuple.Item4));
        }

        [TestCaseSource("NumbersAll")]
        public void VerifyBlueFinished(Tuple<int, int, int, int, int, int> tuple)
        {
            var statistics = new PracticeSessionStatistics(false, 10, tuple.Item1, tuple.Item2, tuple.Item3);

            Assert.That(statistics.Blue, Is.EqualTo(tuple.Item5));
        }

        [TestCaseSource("NumbersAll")]
        public void VerifyYellowFinished(Tuple<int, int, int, int, int, int> tuple)
        {
            var statistics = new PracticeSessionStatistics(false, 10, tuple.Item1, tuple.Item2, tuple.Item3);

            Assert.That(statistics.Yellow, Is.EqualTo(tuple.Item6));
        }
    }
}