using System;

namespace Fundamentals;

public class Variables
{
    public static void Run()
    {
        // Variables are declared inside a method.
        // They have a type, a name, and a value.
        // The type can be set explicitly.
        string firstName = "Ron";

        // The type can be set implicitly using the var keyword.
        // It looks at the right hand of the assignment to
        // determine what the type should be.
        var surname = "Burgundy";

        // As the name suggests, they are variable, so can change.
        // When using a variable we don't need to provide the
        // type again, just the variable's name.
        firstName = "Baxter";
        surname = "The Dog";

        // When calling a method that is inside our current class we just use
        // the method name followed by brackets. The value returned by a
        // method can also be used to set variables.
        var today = GetTodaysDate();

        // A method might have parameters, which will need to be provided.
        // We can either use variables or directly provide values.
        // Again, we just use the variable name when using them.
        Greet(firstName, surname);
    }

    // When creating a method we specify it's accessibility modifier, return type, and name.
    // This method, GetTodaysDate, returns a DateTime and has no parameters.
    private static DateTime GetTodaysDate()
    {
        return DateTime.Now;
    }

    // We can require parameters to use the method. To specify
    // a parameter we provide the type and name.
    private static void Greet(string firstName, string surname)
    {
        // We use parameters inside a method like we would a variable.
        Console.WriteLine($"Hello, {firstName} {surname}!");
    }
}
