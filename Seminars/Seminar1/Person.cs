using System;
using System.Collections.Generic;
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

        public Person(string name, string surname, DateTime birthday, Gender sex)
        {
            this.Name = name;
            this.Birthday = birthday;
            this.Sex = sex;
            Siblings = new List<Person>();
            Children = new List<Person>();
        }




    }
}
