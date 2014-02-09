using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class PracticeController : ControllerBase
    {
        public ActionResult Index(int wordListId)
        {
            var word = ExecuteQuery(new GetPracticeWordQuery(wordListId, CurrentUser));
            return View(new ShowWordViewModel(wordListId, word));
        }

        public ActionResult Show(int wordListId, string wordId)
        {
            var word = ExecuteQuery(new GetWordQuery(wordId));
            return View(new AnswerWordViewModel(wordListId, word));
        }

        public ActionResult Answer(string wordListId, string wordId, int confidence)
        {
            ExecuteCommand(new AnswerWordCommand(CurrentUser, wordId, wordListId, confidence));
            return RedirectToAction("Index", new { wordListId });
        }

        public class ShowWordViewModel
        {
            public ShowWordViewModel(int wordListId, GetPracticeWordQuery.Result word)
            {
                WordListId = wordListId;
                Word = word;
            }

            public int WordListId { get; set; }

            public GetPracticeWordQuery.Result Word { get; set; }
        }

        public class AnswerWordViewModel
        {
            public AnswerWordViewModel(int wordListId, GetWordQuery.Result word)
            {
                WordListId = wordListId;
                Word = word;
            }

            public int WordListId { get; set; }

            public GetWordQuery.Result Word { get; set; }
        }
    }
}