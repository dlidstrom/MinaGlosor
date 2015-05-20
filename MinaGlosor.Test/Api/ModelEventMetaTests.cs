using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MinaGlosor.Web.Models.DomainEvents;
using NUnit.Framework;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class ModelEventMetaTests
    {
        private static IEnumerable<Type> ModelEvents
        {
            get
            {
                foreach (var type in typeof(ModelEvent).Assembly.GetTypes().Where(x => x.IsAbstract == false))
                {
                    if (typeof(ModelEvent).IsAssignableFrom(type)) yield return type;
                }
            }
        }

        [TestCaseSource("ModelEvents")]
        public void MustHaveJsonConstructor(Type eventType)
        {
            var ctor =
                eventType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    Type.EmptyTypes,
                    null);
            Assert.That(ctor, Is.Not.Null, eventType.Name + " must have default constructor");
            Assert.That(
                ctor.GetCustomAttribute<JsonConstructorAttribute>(),
                Is.Not.Null,
                eventType.Name + " constructor must be decorated with JsonConstructor attribute");
        }
    }
}