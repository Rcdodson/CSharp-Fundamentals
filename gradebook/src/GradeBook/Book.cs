using System;
using System.IO;
using System.Collections.Generic;

namespace Gradebook
{
    //delegate
    public delegate void GradeAddedDelegate(object sender, EventArgs args);
    

    //base class
    public class NamedObject 
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        //property
        public string Name
        {
            get;
            set;
        }
    }


    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStats();
        string Name {get; }
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        public Book(string name) : base(name)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public abstract Statistics GetStats();
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {

        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            using(var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
        }

        public override Statistics GetStats()
        {
            var result = new Statistics();
            using(var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }

            return result;
        }
    }

    public class InMemoryBook : Book
    {   
        //field definition 
        private List<double> grades;

        //Constructor
        public InMemoryBook(string name) : base(name)
        {
            Name = name;
            grades = new List<double>();
        }

        //event
        public override event GradeAddedDelegate GradeAdded;

        //class method
        public override void AddGrade(double grade)
        {
            if(grade <= 100 && grade >= 0)
            {
                grades.Add(grade);

                //event
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else 
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        //overloaded method
        public void AddGrade(char letter)
        {
            switch(letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                case 'D':
                    AddGrade(60);
                    break;
                case 'F':
                    AddGrade(50);
                    break;
                default:
                    Console.WriteLine("Invalid LetterGrade");
                    break;
            }
        }

        public List<double> GetGrades()
        {
            return grades;
        }

        //Build stats object
        public override Statistics GetStats()
        {
            var result = new Statistics();

            foreach(double grade in grades)
            {
                result.Add(grade);
            }

            return result;
        }
    }
}