namespace Fundamentals;

// Interfaces describe what a class can do. It is a contract that must be adhered to (implemented).
// The naming convention for an interface is that it starts with I then describes what it does.
// Everything inside an interface is public, as anyone using this contract must have
// access to the properties and methods inside it.
public interface IPrinter
{
    // To define a method, you just need the return type, method name, and parameters.
    // You do not need public or private before the method, and you do not need any
    // body of the method (no {}). This is just the contract, the class
    // that implements the interface has to worry about that!
    void Print(string printer, string document);
}

// To implement an interface we use this syntax: class name : interface name
// The code will not compile unless we "implement" each method in
// the interface. We don't truly need to implement it, just
// as long as the method exists with the correct signature
// the compiler will be satisfied.
public class PrinterManager : IPrinter
{
    // Here we "implement" the Print method in the IPrinter interface
    public void Print(string printer, string document)
    {
        throw new System.NotImplementedException();
    }

    // This method is not part of the interface,
    // just part of the PrinterManager class.
    public string[] GetAllPrinters()
    {
        return ["Cannon", "Epson", "HP"];
    }
}

public class Application
{
    public void Run()
    {
        // If we create an instance of the PrinterManager class, and assign it to a variable
        // of the type PrinterManager, we have access to all the public methods.
        PrinterManager printerManager = new PrinterManager();
        string[] printers = printerManager.GetAllPrinters();
        printerManager.Print(printers[0], "Hello, world");

        // If we assign the instance of the class to an IPrinter variable,
        // we only have methods that are in the "contract" available.
        IPrinter printer = new PrinterManager();
        printer.Print("Epson", "I am a printer");
        // Even though by looking at the code we know the printer variable is set to
        // an instance of PrinterManager, we only have access to the methods
        // from IPrinter because that is the type of the variable.
        // This would error if uncommented.
        //printer.GetAllPrinters();

        string[] documents = ["The Tempest", "Othello", "Much Ado About Nothing"];
        // Because PrinterManager meets the contract of IPrinter, it can be used
        // wherever IPrinter is required.
        PrintMultiple(printerManager, documents);
        PrintMultiple(printer, documents);
    }

    // Here it is more obvious that by only knowing about an IPrinter, we don't know 
    // what the underlying class really is. We just know it by its capabilities,
    // as defined in the interface.
    private void PrintMultiple(IPrinter printer, string[] documents)
    {
        foreach (var document in documents)
        {
            printer.Print("Cannon", document);
        }
    }
}

