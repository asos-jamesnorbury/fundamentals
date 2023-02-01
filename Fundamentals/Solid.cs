﻿using System;
using System.Linq;

namespace Fundamentals;

#region Single responsibility

public class Person_
{
    public string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public void Save()
    {
        // Save person in database
    }

}

#region solution

public class Person
{
    public string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
}

public class PersonRepository
{
    public void Save(Person person)
    {
        // Save person in database
    }
}

#endregion

#endregion

#region Open / close principal

public class Animal_
{
    public string Name { get; set; }

    public void MakeSound()
    {
        if (Name == "lion")
        {
            Console.WriteLine("roar");
        }
        else if (Name == "mouse")
        {
            Console.WriteLine("squeek");
        }
    }
}

#region solution

public abstract class Animal
{
    public string Name { get; set; }
    public abstract void MakeSound();
}

public class Lion : Animal
{
    public override void MakeSound() => Console.WriteLine("roar");
}

public class Mouse : Animal
{
    public override void MakeSound() => Console.WriteLine("squeek");
}

public class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("woof");
}

#endregion

#endregion

#region Liskov substitution

public class Calculator_
{
    protected readonly int[] _numbers;

    public Calculator_(int[] numbers)
    {
        _numbers = numbers;
    }

    public int Sum() => _numbers.Sum();
}

public class MinusCalculator_ : Calculator_
{
    public MinusCalculator_(int[] numbers) : base(numbers)
    {
    }

    public int MinusSum() => Sum() * -1;
}

#region solution
public abstract class Calculator
{
    protected readonly int[] _numbers;

    public Calculator(int[] numbers)
    {
        _numbers = numbers;
    }

    public abstract int Calculate();
}

public class SumCalculator : Calculator
{
    public SumCalculator(int[] numbers) : base(numbers)
    {
    }

    public override int Calculate() => _numbers.Sum();
}

public class MinusSumCalculator : Calculator
{
    public MinusSumCalculator(int[] numbers) : base(numbers)
    {
    }

    public override int Calculate() => _numbers.Sum() * -1;
}

#endregion

#endregion

