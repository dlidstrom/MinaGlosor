using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class PracticeSessionIndex : AbstractIndexCreationTask<PracticeSession>
    {
        public PracticeSessionIndex()
        {
            Map = practiceSessions => from practiceSession in practiceSessions
                                      select new
                                          {
                                              practiceSession.WordListId,
                                              practiceSession.IsFinished,
                                              practiceSession.OwnerId
                                          };
        }
    }
}