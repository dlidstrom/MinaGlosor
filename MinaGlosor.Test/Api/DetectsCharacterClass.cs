using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class DetectsCharacterClass
    {
        [Test]
        public void IsArabic()
        {
            // Assert
            const string Sentence = "!چه مى گويى؟ تعريف كن";
            Assert.That(Sentence, Is.StringMatching(@"\p{IsArabic}"));
        }
    }
}