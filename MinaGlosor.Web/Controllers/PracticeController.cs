using System.Threading.Tasks;
using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class PracticeController : ControllerBase
    {
        public async Task<ActionResult> Index(string wordListId)
        {
            var word = await ExecuteQueryAsync(new GetPracticeWordQuery(wordListId, CurrentUser));
            return View(new ShowWordViewModel(wordListId, word));
        }

        public async Task<ActionResult> Show(string wordListId, string wordId)
        {
            var word = await ExecuteQueryAsync(new GetWordQuery(wordId));
            return View(new AnswerWordViewModel(wordListId, word));
        }

        public async Task<ActionResult> Answer(int wordListId, int wordId, int confidence)
        {
            await ExecuteCommandAsync(new AnswerWordCommand(CurrentUser, wordId, wordListId, confidence));
            return RedirectToAction("Index", new { wordListId });
        }

        public class ShowWordViewModel
        {
            public ShowWordViewModel(string wordListId, GetPracticeWordQuery.Result word)
            {
                WordListId = wordListId;
                Word = word;
            }

            public string WordListId { get; set; }

            public GetPracticeWordQuery.Result Word { get; set; }
        }

        public class AnswerWordViewModel
        {
            public AnswerWordViewModel(string wordListId, GetWordQuery.Result word)
            {
                WordListId = wordListId;
                Word = word;
            }

            public string WordListId { get; set; }

            public GetWordQuery.Result Word { get; set; }
        }
    }
}