using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Commands;
using Raven.Client;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Commands.Handlers
{
    public class ProgressCommandHandler :
        ICommandHandler<UpdateProgressAfterAddWordCommand, object>,
        ICommandHandler<UpdateWordScoreCommand, object>,
        ICommandHandler<CreateProgressCommand, CreateProgressCommand.Result>,
        ICommandHandler<WordScoreRegisteredCommand, object>,
        ICommandHandler<WordScoreChangedDifficultyCommand, object>,
        ICommandHandler<WordIsUpToDateCommand, object>,
        ICommandHandler<WordExpiredCommand, object>
    {
        public IDocumentSession Session { get; set; }

        public object Handle(UpdateProgressAfterAddWordCommand command)
        {
            var progress = Session.Load<Progress>(command.ProgressId);
            progress.WordAdded(command.NumberOfWords);
            return new object();
        }

        public bool CanExecute(UpdateProgressAfterAddWordCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(UpdateWordScoreCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var id = Progress.GetIdFromWordListForUser(word.WordListId, command.OwnerId);
            var progress = Session.Load<Progress>(id);
            progress.UpdateDifficultyCounts(command.WordScoreDifficultyLifecycle);
            return new object();
        }

        public bool CanExecute(UpdateWordScoreCommand command, User currentUser)
        {
            return true;
        }

        public CreateProgressCommand.Result Handle(CreateProgressCommand command)
        {
            var progress = new Progress(
                command.OwnerId,
                command.WordListId);
            Session.Store(progress);
            return new CreateProgressCommand.Result(progress);
        }

        public bool CanExecute(CreateProgressCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(WordScoreRegisteredCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var id = Progress.GetIdFromWordListForUser(wordList.Id, command.OwnerId);
            var progress = Session.Load<Progress>(id);
            progress.NewWordHasBeenPracticed(wordList.NumberOfWords);
            return new object();
        }

        public bool CanExecute(WordScoreRegisteredCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(WordScoreChangedDifficultyCommand command)
        {
            // load progress model, update it
            var progressId = Progress.GetIdFromWordListForUser(command.WordListId, command.OwnerId);
            var progress = Session.Load<Progress>(progressId);
            progress.UpdateDifficultyCounts(command.WordScoreDifficultyLifecycle);
            return new object();
        }

        public bool CanExecute(WordScoreChangedDifficultyCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(WordIsUpToDateCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var id = Progress.GetIdFromWordListForUser(wordList.Id, command.OwnerId);
            var progress = Session.Load<Progress>(id);
            progress.WordIsUpToDate(wordList.NumberOfWords);
            return new object();
        }

        public bool CanExecute(WordIsUpToDateCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(WordExpiredCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var progressId = Progress.GetIdFromWordListForUser(word.WordListId, command.OwnerId);
            var progress = Session.Load<Progress>(progressId);
            progress.WordHasExpired(wordList.NumberOfWords);

            return new object();
        }

        public bool CanExecute(WordExpiredCommand command, User currentUser)
        {
            return true;
        }
    }
}