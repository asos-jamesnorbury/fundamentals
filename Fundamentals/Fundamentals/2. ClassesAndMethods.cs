using System;

namespace Fundamentals;

// Functionality about a single thing can be grouped in a class.
// This allows us to organise our code more easily.
public class ClassesAndMethods
{
    // A field stores data that is associated to the instance of that class.
    // Setting it as "readonly" means it can only be set in the
    // constructor. After that it can only be read.
    private readonly DateTime _classInstantiatedAt;

    // A class has a special method called a "constructor". This just 
    // has the accessibility modifier and return type. The 
    // return type is always the name of the class.
    public ClassesAndMethods()
    {
        _classInstantiatedAt = DateTime.Now;
    }

    public void CallAMethodInAnotherClass()
    {
        // When calling a method that is defined in a different class you need to "dot" into it.
        // If the method is static, you use the class name "dot" method name.
        // Else, use an instance of the class variable "dot" method name.

        // Static method called on the Environment class.
        // Environment "dot" GetEnvironmentVariables
        var environmentVariables = Environment.GetEnvironmentVariables();

        // Non-static method called on the Random class. An instance
        // of the random class is created first. A reference to
        // this instance is stored in the "rng" variable.
        var rng = new Random();

        // Variable holding an instance of the class "dot" method name.
        var randomNumber = rng.Next();
    }

    public void CreatingInstancesOfClasses()
    {
        // Non-static classes require an "instance". An instance of a class is
        // can hold state and get passed around like a variable. We use the
        // "new" keyword to create an instance of a class. Here we say
        // that a reference to an instance of the class is stored
        // in the variable.
        var rng = new Random();

        // If there are properties in the class, we can set them when we create the class.
        var person = new Person
        {
            FirstName = "Maggie",
            Surname = "Simpson"
        };

        // This is equivalent to the above. Because we are creating instances of the
        // Person class, we're able to create multiple of them to hold
        // different information. 
        var otherPerson = new Person();
        person.FirstName = "Homer";
        person.Surname = "Simpson";
    }

    public void UsingStaticClasses()
    {
        // Static classes or methods do not required an "instance" to be called.
        // They are accessible just by using the class and method name.
        var isLeapYear = DateTime.IsLeapYear(1994);

        // When a static class is called, an instance is created by .NET under-the-hood.
        // This instance lasts the entire lifetime of the program, and the same
        // instance is used every time that static class is called.
        Singleton.GetProcessId();
    }
}

public static class Singleton
{
    // If a class is static, every field, property, or method inside must also be static.
    public static int GetProcessId() => System.Diagnostics.Process.GetCurrentProcess().Id;
}

public class Person
{
    public string FirstName { get; set; }
    public string Surname { get; set; }
}
