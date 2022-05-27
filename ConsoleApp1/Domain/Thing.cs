namespace ConsoleApp1.Domain;

class Thing
{
    public Thing(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }

    public string Name { get; private set; }
    public int Quantity { get; private set; }
}
