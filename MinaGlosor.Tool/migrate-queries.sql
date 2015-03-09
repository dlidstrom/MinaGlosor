select top 10 * from Users
select top 10 * from Words
select top 10 WordLists.Id, WordLists.Name, Users.Email
from WordLists
join Users on WordLists.OwnerId = Users.Id
where WordLists.Migrated = 0

select Words.Id, Words.CreatedDate, Words.Text, Words.Definition, WordLists.Name, Users.Email
from Words
join WordLists on Words.WordListId = WordLists.Id
join Users on WordLists.OwnerId = Users.Id
where Words.Migrated = 0

update users set migrated = 0
update wordlists set migrated = 0
update words set migrated = 0
select top 10 * from Users
select top 10 * from wordlists
select top 10 * from words
