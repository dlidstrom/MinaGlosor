using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class WordController : ControllerBase
    {
        public ActionResult ViewWords(string wordListId)
        {
            var words = ExecuteQuery(new GetWordsQuery(wordListId));
            return View(new WordIndexViewModel(wordListId, words));
        }

        public ActionResult Add(string wordListId)
        {
            return View(new AddWordViewModel(wordListId));
        }

        [HttpPost]
        public ActionResult Add(string wordListId, AddWordRequest request)
        {
            if (ModelState.IsValid == false)
                return View();
            ExecuteCommand(new CreateWordCommand(wordListId, request.Text, request.Definition));
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
            public WordIndexViewModel(string wordListId, GetWordsQuery.Result[] words)
            {
                WordListId = wordListId;
                Words = words;
            }

            public string WordListId { get; private set; }

            public GetWordsQuery.Result[] Words { get; private set; }
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