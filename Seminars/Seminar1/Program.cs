using Seminar1;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var dadsGrandFather = new Person("Сергей", new DateTime(1933, 4, 14), Person.Gender.Male);
var dadsGrandMother = new Person("Светлана", new DateTime(1939, 10, 2), Person.Gender.Female);

var momsGrandfather = new Person("Петр", new DateTime(1934, 12, 18), Person.Gender.Male);
var momsGrandmother = new Person("Мария", new DateTime(1938, 3, 5), Person.Gender.Female);

var father = new Person("Василий", new DateTime(1963, 6, 15), Person.Gender.Male);
var mother = new Person("Наталья", new DateTime(1964, 7, 20), Person.Gender.Female);

dadsGrandFather.AddChild(father);
dadsGrandMother.AddChild(father);

momsGrandfather.AddChild(mother);
momsGrandmother.AddChild(mother);

var son = new Person("Гиоргий", new DateTime(1982, 4, 21), Person.Gender.Male);
var daughter = new Person("Анастасия", new DateTime(1990, 9, 15), Person.Gender.Female);

father.AddChild(son);
father.AddChild(daughter);
mother.AddChild(son);
mother.AddChild(daughter);

var brother = new Person("Михаил", new DateTime(1982, 5, 10), Person.Gender.Male);
var sister = new Person("Александра", new DateTime(1986, 12, 4), Person.Gender.Female);

father.AddChild(brother);
father.AddChild(sister);
mother.AddChild(brother);
mother.AddChild(sister);

father.PrintFamilyTree(0);
