namespace Fundamentals.Properties;
public class PropertyExample
{
    // Auto-properties, able to access and modify the value without
    // defining a backing field to hold it.
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Auto-property with only a get must be set in the constructor.
    public string Nickname { get; }

    // Property with backing field (cached). This allows saving the field to
    // prevent repetedly calculating a value that does not change.
    private string _backingField;
    public string BackingFieldProperty
    {
        get
        {
            if (_backingField == null)
            {
                _backingField = ExpensiveComputation();
            }

            return _backingField;
        }
    }

    // Property with backing field and expression bodied members
    private int _length;
    public int Length
    {
        get => _length;
        set => _length = value;
    }

    // Expression-bodied members can be used to define get only properties.
    // This is the same as:
    // public string FullName { get { return $"{FirstName} {LastName}"; } }
    public string FullName => $"{FirstName} {LastName}";

    // Get and set can have different levels of protection. Here, the get is
    // public, so anyone can access the property. However, the set is
    // private, so the value can only be assigned within the class.
    public int Age { get; private set; }

    // Constructor is used to set the value for the read-only property. A value can 
    // be set because it is an auto-property. Note, this does not work for
    // a property with a backing field; try uncommmenting below and see.
    public PropertyExample(string nickname)
    {
        Nickname = nickname;
        // BackingFieldProperty = "This is not allowed!";
    }

    private string ExpensiveComputation()
    {
        return "Time consuming computation string";
    }
}
