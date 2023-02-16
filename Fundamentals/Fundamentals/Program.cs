using System;
using System.Runtime.CompilerServices;

namespace Fundamentals;

internal class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1. Value vs reference type");
            Console.WriteLine("2. Loops");
            Console.WriteLine("3. Conditions");
            Console.WriteLine("4. Control flow");
            Console.Write("Select demo: ");
            var demo = Console.ReadLine();
            switch (demo)
            {
                case "1":
                    ValueVsReference();
                    break;
                case "2":
                    Loops();
                    break;
                case "3":
                    Conditions();
                    break;
                case "4":
                    ControlFlow();
                    break;
                default:
                    return;
            }

            Console.WriteLine();
        }
    }

    #region Value vs reference

    private static void ValueVsReference()
    {
        Console.WriteLine("Value types");
        int i = 10;
        Log($"i = {i}");
        Modify(i);
        Log($"i = {i}");

        Console.WriteLine();

        Console.WriteLine("Reference types");
        Person person = new Person { Name = "James" };
        Log($"Name = {person.Name}");
        Modify(person);
        Log($"Name = {person.Name}");

        Console.WriteLine("What happens if you change the Person class into a struct?");
    }

    private static void Modify(int i)
    {
        i = 5;
        Log($"i = {i}");
    }

    private static void Modify(Person person)
    {
        var newPerson = person.Copy();
        newPerson.Name = "Someone";
        Log($"Name = {person.Name}");
    }

    // What happens if you change the Person class to a struct?
    private class Person
    {
        public string Name { get; set; }
        public Person Copy() => new Person { Name = Name };
    }

    #endregion

    #region Loops
    private static void Loops()
    {
        For();
        Console.WriteLine();
        ForEach();
        Console.WriteLine();
        While();
        Console.WriteLine();
        DoWhile();
        Console.WriteLine();
    }

    private static void For()
    {
        string[] names = new string[] { "Alice", "Bob", "Sam" };
        for (var i = 0; i < names.Length; i++)
        {
            if (i == names.Length - 1)
            {
                break;
            }

            Log(names[i]);
        }
    }

    private static void ForEach()
    {
        string[] names = new string[] { "Alice", "Bob", "Sam" };
        foreach (var name in names)
        {
            Log(name);
        }
    }

    private static void While()
    {
        int counter = 0;
        while (!IsFinished())
        {
            Log(counter.ToString());
            counter++;
        }
    }

    private static void DoWhile()
    {
        int counter = 0;
        do
        {
            Log(counter.ToString());
            counter++;
        } while (!IsFinished());
    }

    private static bool IsFinished()
    {
        var rand = new Random();
        return rand.Next(0, 5) == 0;
    }

    #endregion

    #region Conditions

    private static void Conditions()
    {
        If();
        Switch();
    }

    private static void If()
    {
        var num = GetRandomNumber();
        if (num == 1)
        {
            Log($"num = {num}");
        }
        else if (num == 2)
        {
            Log($"num = {num}");
        }
        else
        {
            Log($"num = {num}");
        }
    }

    private static void Switch()
    {
        var num = GetRandomNumber();
        // Define expected values and a catch all case
        switch (num)
        {
            case 1:
                Log($"num = {num}");
                // Must break or return at the end of the case
                break;
            case 2:
                Log($"num = {num}");
                break;
            // Cases can be grouped if you want to do the same thing
            case 3:
            case 4:
                Log("Num is 3 or 4");
                break;
            default:
                Log($"num = {num}");
                break;
        }
    }

    private static int GetRandomNumber() =>
        new Random().Next(0, 10);

    #endregion

    #region Control flow

    private static void ControlFlow()
    {
        Continue();
        Console.WriteLine();
        Break();
        Console.WriteLine();
        Return();
    }

    private static void Continue()
    {
        for (var i = 1; i <= 5; i++)
        {
            if (i == 3)
            {
                // Continue to the next iteration of the loop
                continue;
            }

            Log(i.ToString());
        }

        Log("I'm now out the loop");
    }

    private static void Break()
    {
        for (var i = 1; i <= 5; i++)
        {
            if (i == 3)
            {
                // Break out the loop
                break;
            }

            Log(i.ToString());
        }

        Log("I'm now out the loop");
    }

    private static void Return()
    {
        for (var i = 1; i <= 5; i++)
        {
            if (i == 3)
            {
                // Return to the calling method - exit the method
                return;
            }

            Log(i.ToString());
        }

        Log("I'm now out the loop");
    }

    #endregion

    private static void Log(string message, [CallerMemberName] string caller = "") =>
        Console.WriteLine($"{caller} - {message}");
}
