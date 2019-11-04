using System;
using System.Collections.Generic;

namespace Gradebook
{
    class Program
    {
        static void Main(string[] args)
        {
            IBook book = new DiskBook("Buzz's Grade Book");
            book.GradeAdded += OnGradeAdded;
            EnterGrades(book);

            var stats = book.GetStats();

            Console.WriteLine($"The average grade is {stats.Average:N1}");
            Console.WriteLine($"The lowest grade is {stats.Low:N1}");
            Console.WriteLine($"The highest grade is {stats.High:N1}");
            Console.WriteLine($"The letter grade is {stats.Letter}");
        }

        private static void EnterGrades(IBook book)
        {
            while (true)
            {
                Console.WriteLine("Enter Grades. Press 'q' to stop and calculate statistics");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }

                //exception handling
                try
                {
                    book.AddGrade(double.Parse(input));
                }
                //catch AddGrade Exception
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //catch double.Parse() Exception
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //event
        static void OnGradeAdded(Object sender, EventArgs e)
        {
            Console.WriteLine("A grade was added");
        }
    }
}
