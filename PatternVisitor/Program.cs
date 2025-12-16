using static Program;

public interface IVisitor
{
    void VisitProduct(Product product);
    void VisitBox(Box box);
}

public interface IElement
{
    void Accept(IVisitor visitor);
}

public class DeliveryCostCalculator : IVisitor
{
    public double TotalDeliveryCost { get; private set; }

    public void VisitProduct(Product product)
    {
        TotalDeliveryCost += product.Cost * 0.05;
    }

    public void VisitBox(Box box)
    {
        TotalDeliveryCost += 10;
    }
}

public class TaxCalculator : IVisitor
{
    public double TotalTax { get; private set; }

    public void VisitProduct(Product product)
    {
        TotalTax += product.Cost * 0.20;
    }

    public void VisitBox(Box box)
    {
        // Коробки налогом не облагаются
    }
}

public class Box : IElement
{
    private int size;
    public int Size => size;

    private int boxCost;
    public int BoxCost => boxCost;

    private List<Box> nestedBoxes;
    private List<Product> products;

    public Box(int size)
    {
        nestedBoxes = new List<Box>();
        products = new List<Product>();
        this.size = size;
    }

    public void InsertBox(Box newBox)
    {
        if (newBox.Size >= size)
        {
            Console.WriteLine("Не удалось поместить коробку (размер коробки слишком мал)");
            return;
        }
        nestedBoxes.Add(newBox);
        boxCost += newBox.BoxCost;
    }
    public void AddProduct(Product product)
    {
        if (nestedBoxes.Count > 0)
        {
            Console.WriteLine("Не удалось поместить продукт (В коробке лежат другие коробки)");
        }
        products.Add(product);
        boxCost += product.Cost;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitBox(this);

        foreach (var product in products) product.Accept(visitor);
        foreach (var box in nestedBoxes) box.Accept(visitor);
    }
}

public class Product : IElement
{
    private string name;
    private int cost;
    public int Cost => cost;

    public Product(string name, int cost)
    {
        this.name = name;
        this.cost = cost;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitProduct(this);
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        Box mainBox = new Box(10);

        Box boxWithFruits = new Box(5);
        boxWithFruits.AddProduct(new Product("Яблоко", 100));
        boxWithFruits.AddProduct(new Product("Банан", 200));
        mainBox.InsertBox(boxWithFruits);

        var deliveryCalc = new DeliveryCostCalculator();
        mainBox.Accept(deliveryCalc);
        Console.WriteLine($"Стоимость доставки: {deliveryCalc.TotalDeliveryCost}");

        var taxCalc = new TaxCalculator();
        mainBox.Accept(taxCalc);
        Console.WriteLine($"Сумма налогов: {taxCalc.TotalTax}");


        // Вопрос: Как вы будете расширять систему, добавляя новые типы расчетов (например, скидки)? Какие изменения потребуются в коде?

        // Для добавления нового типа расчёта понадобится написание конкретного посетителя. Изменения в Product или в Box вносить не потребуется.

    }

}