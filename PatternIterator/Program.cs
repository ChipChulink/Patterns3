public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public int Popularity { get; set; }

    public override string ToString()
    {
        return $"[ID: {Id}] {Name} ({Category}) | Цена: {Price} | Популярность: {Popularity}";
    }
}


public interface ICatalogIterator
{
    bool HasNext();
    Item Next();
    List<Item> Next(int count);
    void Reset();
}


public class Catalog
{
    private readonly List<Item> _items;

    public Catalog(List<Item> items)
    {
        _items = items;
    }

    public List<Item> GetItems()
    {
        return _items;
    }

    public ICatalogIterator CreateIterator(string criteria)
    {
        switch (criteria.ToLower())
        {
            case "category":
                return new CategoryIterator(this);
            case "price":
                return new PriceIterator(this);
            case "popularity":
                return new PopularityIterator(this);
            default:
                throw new ArgumentException($"Критерий обхода '{criteria}' не поддерживается.");
        }
    }
}


public class CategoryIterator : ICatalogIterator
{
    private readonly List<Item> _items;
    private int _position = 0;

    public CategoryIterator(Catalog catalog)
    {
        _items = catalog.GetItems().OrderBy(i => i.Category).ToList();
    }

    public bool HasNext()
    {
        return _position < _items.Count;
    }

    public Item Next()
    {
        if (HasNext())
        {
            return _items[_position++];
        }
        return null;
    }

    public List<Item> Next(int count)
    {
        var result = _items.Skip(_position).Take(count).ToList();
        _position += result.Count;
        return result;
    }

    public void Reset()
    {
        _position = 0;
    }
}

public class PriceIterator : ICatalogIterator
{
    private readonly List<Item> _items;
    private int _position = 0;

    public PriceIterator(Catalog catalog)
    {
        _items = catalog.GetItems().OrderBy(i => i.Price).ToList();
    }

    public bool HasNext() => _position < _items.Count;

    public Item Next()
    {
        if (HasNext()) return _items[_position++];
        return null;
    }

    public List<Item> Next(int count)
    {
        var result = _items.Skip(_position).Take(count).ToList();
        _position += result.Count;
        return result;
    }

    public void Reset() => _position = 0;
}

public class PopularityIterator : ICatalogIterator
{
    private readonly List<Item> _items;
    private int _position = 0;

    public PopularityIterator(Catalog catalog)
    {
        // Сортируем по популярности (от высокой к низкой)
        _items = catalog.GetItems().OrderByDescending(i => i.Popularity).ToList();
    }

    public bool HasNext() => _position < _items.Count;

    public Item Next()
    {
        if (HasNext()) return _items[_position++];
        return null;
    }

    public List<Item> Next(int count)
    {
        List<Item> result = _items.Skip(_position).Take(count).ToList();
        _position += result.Count;
        return result;
    }

    public void Reset() => _position = 0;
}

public class Program
{
    public static void Main(string[] args)
    {
        List<Item> items = new List<Item>
        {
            new Item { Id = 1, Name = "Книга 'Паттерны'", Category = "Книги", Price = 499.99m, Popularity = 150 },
            new Item { Id = 2, Name = "Ноутбук", Category = "Электроника", Price = 79999.00m, Popularity = 90 },
            new Item { Id = 3, Name = "Мышь беспроводная", Category = "Электроника", Price = 1299.50m, Popularity = 250 },
            new Item { Id = 4, Name = "Футболка", Category = "Одежда", Price = 2500.00m, Popularity = 100 },
            new Item { Id = 5, Name = "Книга 'Паттерны 2'", Category = "Книги", Price = 350.00m, Popularity = 50 }
        };

        Catalog catalog = new Catalog(items);

        Console.WriteLine("--- Обход по Популярности (Топ-3) ---");
        ICatalogIterator popularityIterator = catalog.CreateIterator("popularity");

        foreach (var item in popularityIterator.Next(3))
        {
            Console.WriteLine($"{item}");
        }

        Console.WriteLine("\n--- Обход по Цене ---");
        ICatalogIterator priceIterator = catalog.CreateIterator("price");

        foreach (var item in priceIterator.Next(catalog.GetItems().Count))
        {
            Console.WriteLine($"{item}");
        }


        // Вопрос: Как вы будете обрабатывать ситуацию, когда в каталоге нет товаров, соответствующих определенному критерию?
        // Какие изменения внесете в систему?

        // Обработка пустого результата: После фильтрации, если список товаров пуст, метод HasNext() итератора вернет false.
        // Клиентский код просто не увидит ни одного элемента. Можно сделать чтобы клиентский код проверял HasNext() и выводил сообщение "Товаров не найдено".
    }
}