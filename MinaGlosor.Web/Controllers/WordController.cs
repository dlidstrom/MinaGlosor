using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class WordController : ControllerBase
    {
        public async Task<ActionResult> ViewWords(int wordListId)
        {
            var words = await ExecuteQueryAsync(new GetWordsQuery(wordListId));
            return View(new WordIndexViewModel(wordListId, words));
        }

        public ActionResult Add(string wordListId)
        {
            return View(new AddWordViewModel(wordListId));
        }

        [HttpPost]
        public async Task<ActionResult> Add(int wordListId, AddWordRequest request)
        {
            if (ModelState.IsValid == false)
                return View();
            await ExecuteCommandAsync(new CreateWordCommand(wordListId, request.Text, request.Definition));
            return RedirectToAction("Add", new { wordListId });
        }

        public class AddWordRequest
        {
            [MaxLength(1024)]
            public string Text { get; set; }

            [MaxLength(1024)]
            public string Definition { get; set; }
        }

        public class WordIndexViewModel
        {
            public WordIndexViewModel(int wordListId, IEnumerable<GetWordsQuery.Result> words)
            {
                WordListId = wordListId;
                Words = words;
            }

            public int WordListId { get; private set; }

            public IEnumerable<GetWordsQuery.Result> Words { get; private set; }
        }

        public class AddWordViewModel
        {
            public AddWordViewModel(string wordListId)
            {
                WordListId = wordListId;
            }

            public string WordListId { get; private set; }
        }
    }
}