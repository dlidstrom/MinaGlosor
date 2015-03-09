namespace MinaGlosor.Web.Models
{
    public enum ConfidenceLevel
    {
        PerfectResponse = 5,
        CorrectAfterHesitation = 4,
        RecalledWithSeriousDifficulty = 3,
        IncorrectWithEasyRecall = 2,
        IncorrectButRemembered = 1,
        CompleteBlackout = 0,
        Unknown = -1
    }
}