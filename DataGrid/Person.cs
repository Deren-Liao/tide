using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LogView
{ 

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Skill
    {
        public int Years { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Years} years of {Name}";
        }
        public List<string> Extensible { get; set; }
    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<Skill> SkillList { get; set; }
        public string Skills
        {
            get { return string.Join(";", SkillList); }
        }
        public DateTime Birthday { get; set; }
    }

    public class Persons : ObservableCollection<Person>
    {
        public Persons()
        {
        }


        private Random gen = new Random();
        DateTime RandomDay(int range = 3)
        {
            DateTime start = new DateTime(1995, 1, 1);
            return start.AddDays(gen.Next(range));
        }

        private string[] skillSet = { "C#", "Python", "COM", "DotNet", "Java", "Big Data" };

        private Skill RandomSkill()
        {
            return new Skill()
            {
                Years = gen.Next(10),
                Name = skillSet[gen.Next(skillSet.Length)],
                Extensible = new List<string>(skillSet)
            };
        }

        private List<Person> AddSome()
        {
            List<Person> list = new List<Person>();
            list.Add(new Person() { Id = 1, Name = "Person1", Position = "Manager", SkillList = new List<Skill>() });
            list.Add(new Person() { Id = 2, Name = "Programmer2", Position = "Programmer", SkillList = new List<Skill>() });
            list.Add(new Person() { Id = 3, Name = "Person3", Position = "Programmer", SkillList = new List<Skill>() });
            list.Add(new Person() { Id = 4, Name = "Person4", Position = "Admin", SkillList = new List<Skill>() });
            list.Add(new Person() { Id = 5, Name = "Person5", Position = "Tester", SkillList = new List<Skill>() });
            list.Add(new Person() { Id = 6, Name = "Person6", Position = "Developer", SkillList = new List<Skill>() });
            list.Add(new Person() { Id = 7, Name = "Person7", Position = "Developer", SkillList = new List<Skill>() });
            return list;
        }

        private int pageNumber = 0;
        public void AddPage()
        {
            Random rand = new Random();
            var pList = AddSome();
            foreach (Person person in pList)
            {
                person.Id += pageNumber * 10;
                person.Birthday = RandomDay();
                for (int i = 0; i < 10; ++i)
                {
                    person.SkillList.Add(RandomSkill());
                }

                this.Add(person);
            }

            ++pageNumber;
        }
    }
}
