using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar1
{
    public class Person
    {
        public enum Gender
        {
            Male,
            Female
        }

        public string Name { get; private set; }
        public Gender Sex { get; private set; }
        public DateTime Birthday { get; private set; }
        public Person Mother { get; private set; }
        public Person Father { get; private set; }
        public Person Spouse { get; private set; }
        public List<Person> Children { get; private set; }
        public List<Person> Siblings { get; private set; }

        public Person(string name, DateTime birthday, Gender sex)
        {
            this.Name = name;
            this.Birthday = birthday;
            this.Sex = sex;
            Siblings = new List<Person>();
            Children = new List<Person>();
        }

        public string GetBirthday()
        {
            return Birthday.ToString("dd.MM.yyyy");
        }

        public void SetBirthday(string birth)
        {
            if (DateTime.TryParseExact(birth, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedBirth))
            {
                Birthday = parsedBirth;
            }
            else
            {
                throw new ArgumentException("Введите дату рождения по формату 'дд.мм.гггг'.");
            }
        }

        public void AddChild(Person child)
        {
            Children.Add(child);
            child.Mother = this;
            if (Sex == Gender.Male)
                child.Father = this;
        }

        public void AddSibling(Person sibling)
        {
            Siblings.Add(sibling);
            sibling.Siblings.Add(this);
        }

        public void PrintFamilyTree(int level, string relationship = null)
        {

            string prefix = new string('|', level) + new string('-', 3);
            string personInfo = $"Имя: {Name}, Дата рождения: {Birthday}";
            if (relationship != null)
            {
                personInfo += $" {relationship}";
            }
            Console.WriteLine($" {prefix} {personInfo}");

            foreach (var child in Children)
            {
                string childRelationship = child.Sex == Gender.Female ? " Дочь" : " Сын";
                child.PrintFamilyTree(level + 1, childRelationship);
            }

            foreach (var sibling in Siblings)
            {
                string siblingRelationship = sibling.Sex == Gender.Female ? " Сестра" : " Брат";
                sibling.PrintFamilyTree(level, siblingRelationship);
            }
        }


    }
}
