using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure.Tracing;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class EventIdsMetaTests
    {
        private static IEnumerable<Tuple<string, object>> InformationalPreliminary
        {
            get
            {
                var fieldInfos = typeof(EventIds.Informational_Preliminary_1XXX).GetFields();
                return fieldInfos.Where(x => x.IsLiteral)
                                 .Select(fieldInfo => Tuple.Create(fieldInfo.Name, fieldInfo.GetRawConstantValue()));
            }
        }

        private static IEnumerable<Tuple<string, object>> InformationalCompletion
        {
            get
            {
                var fieldInfos = typeof(EventIds.Informational_Completion_2XXX).GetFields();
                return fieldInfos.Where(x => x.IsLiteral)
                                 .Select(fieldInfo => Tuple.Create(fieldInfo.Name, fieldInfo.GetRawConstantValue()));
            }
        }

        private static IEnumerable<Tuple<string, object>> InformationalApplicationLog
        {
            get
            {
                var fieldInfos = typeof(EventIds.Informational_ApplicationLog_3XXX).GetFields();
                return fieldInfos.Where(x => x.IsLiteral)
                                 .Select(fieldInfo => Tuple.Create(fieldInfo.Name, fieldInfo.GetRawConstantValue()));
            }
        }

        private static IEnumerable<Tuple<string, object>> WarningTransient
        {
            get
            {
                var fieldInfos = typeof(EventIds.Warning_Transient_4XXX).GetFields();
                return fieldInfos.Where(x => x.IsLiteral)
                                 .Select(fieldInfo => Tuple.Create(fieldInfo.Name, fieldInfo.GetRawConstantValue()));
            }
        }

        private static IEnumerable<Tuple<string, object>> ErrorPermanent
        {
            get
            {
                var fieldInfos = typeof(EventIds.Error_Permanent_5XXX).GetFields();
                return fieldInfos.Where(x => x.IsLiteral)
                                 .Select(fieldInfo => Tuple.Create(fieldInfo.Name, fieldInfo.GetRawConstantValue()));
            }
        }

        private static IEnumerable<Tuple<string, object>> InformationFinalization
        {
            get
            {
                var fieldInfos = typeof(EventIds.Information_Finalization_8XXX).GetFields();
                return fieldInfos.Where(x => x.IsLiteral)
                                 .Select(fieldInfo => Tuple.Create(fieldInfo.Name, fieldInfo.GetRawConstantValue()));
            }
        }

        private static IEnumerable<Tuple<string, object>> CriticalUnknown
        {
            get
            {
                var fieldInfos = typeof(EventIds.Critical_Unknown_9XXX).GetFields();
                return fieldInfos.Where(x => x.IsLiteral)
                                 .Select(fieldInfo => Tuple.Create(fieldInfo.Name, fieldInfo.GetRawConstantValue()));
            }
        }

        [TestCaseSource("InformationalPreliminary")]
        public void Informational_Preliminary(Tuple<string, object> keyValuePair)
        {
            VerifyConstant(keyValuePair);
        }

        [TestCaseSource("InformationalCompletion")]
        public void Informational_Completion(Tuple<string, object> keyValuePair)
        {
            VerifyConstant(keyValuePair);
        }

        [TestCaseSource("InformationalApplicationLog")]
        public void Informational_ApplicationLog(Tuple<string, object> keyValuePair)
        {
            VerifyConstant(keyValuePair);
        }

        [TestCaseSource("WarningTransient")]
        public void Warning_Transient(Tuple<string, object> keyValuePair)
        {
            VerifyConstant(keyValuePair);
        }

        [TestCaseSource("ErrorPermanent")]
        public void Error_Permanent(Tuple<string, object> keyValuePair)
        {
            VerifyConstant(keyValuePair);
        }

        [TestCaseSource("InformationFinalization")]
        public void Information_Finalization(Tuple<string, object> keyValuePair)
        {
            VerifyConstant(keyValuePair);
        }

        [TestCaseSource("CriticalUnknown")]
        public void Critical_Unknown(Tuple<string, object> keyValuePair)
        {
            VerifyConstant(keyValuePair);
        }

        [Test]
        public void Informational_Preliminary_Unique()
        {
            var values = InformationalPreliminary.Select(x => x.Item2);
            var duplicates = values.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            Assert.That(
                duplicates,
                Has.Length.EqualTo(0),
                "InformationalPreliminary has duplicate values " + string.Join(", ", duplicates));
        }

        [Test]
        public void Informational_Completion_Unique()
        {
            var values = InformationalCompletion.Select(x => x.Item2);
            var duplicates = values.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            Assert.That(
                duplicates,
                Has.Length.EqualTo(0),
                "InformationalCompletion has duplicate values " + string.Join(", ", duplicates));
        }

        [Test]
        public void Informational_ApplicationLog_Unique()
        {
            var values = InformationalApplicationLog.Select(x => x.Item2);
            var duplicates = values.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            Assert.That(
                duplicates,
                Has.Length.EqualTo(0),
                "InformationalApplicationLog has duplicate values " + string.Join(", ", duplicates));
        }

        [Test]
        public void Warning_Transient_Unique()
        {
            var values = WarningTransient.Select(x => x.Item2);
            var duplicates = values.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            Assert.That(
                duplicates,
                Has.Length.EqualTo(0),
                "WarningTransient has duplicate values " + string.Join(", ", duplicates));
        }

        [Test]
        public void Error_Permanent_Unique()
        {
            var values = ErrorPermanent.Select(x => x.Item2);
            var duplicates = values.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            Assert.That(
                duplicates,
                Has.Length.EqualTo(0),
                "ErrorPermanent has duplicate values " + string.Join(", ", duplicates));
        }

        [Test]
        public void Information_Finalization_Unique()
        {
            var values = InformationFinalization.Select(x => x.Item2);
            var duplicates = values.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            Assert.That(
                duplicates,
                Has.Length.EqualTo(0),
                "InformationFinalization has duplicate values " + string.Join(", ", duplicates));
        }

        [Test]
        public void Critical_Unknown_Unique()
        {
            var values = CriticalUnknown.Select(x => x.Item2);
            var duplicates = values.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
            Assert.That(
                duplicates,
                Has.Length.EqualTo(0),
                "CriticalUnknown has duplicate values " + string.Join(", ", duplicates));
        }

        private static void VerifyConstant(Tuple<string, object> keyValuePair)
        {
            // type
            var id = keyValuePair.Item1;
            var value = keyValuePair.Item2;
            Assert.That(value, Is.InstanceOf<int>());

            // prefix
            Assert.That(
                id,
                Is.StringStarting("Web").Or.StringStarting("NotificationService"));

            // postfix
            var indexOf = id.LastIndexOf("_", StringComparison.Ordinal);
            Assert.That(
                indexOf,
                Is.AtLeast(1),
                string.Format("Expected _ as separator, like so: Subsystem_EventType_1024 ({0} has no _)", id));

            // value
            int number;
            Assert.That(int.TryParse(id.Substring(indexOf + 1), out number), Is.True, "Expected id to end with number, like so: _1024");
            Assert.That((int)value, Is.EqualTo(number), "Value must be same as postfix: " + id);
        }
    }
}