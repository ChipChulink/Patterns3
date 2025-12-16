using System;
using System.Collections.Generic;

public class ShoppingCartMemento
{
    public List<string> Items { get; }

    public ShoppingCartMemento(List<string> items)
    {
        Items = new List<string>(items);
    }
}

public class ShoppingCart
{
    private List<string> _items = new List<string>();

    public void AddItem(string item)
    {
        _items.Add(item);
        Console.WriteLine($"Добавлен товар: {item}");
        DisplayCart();
    }

    public void RemoveItem(string item)
    {
        if (_items.Remove(item))
        {
            Console.WriteLine($"Удален товар: {item}");
        }
        else
        {
            Console.WriteLine($"Товар {item} не найден в корзине");
        }
        DisplayCart();
    }

    public ShoppingCartMemento Save()
    {
        Console.WriteLine("Состояние корзины сохранено\n");
        return new ShoppingCartMemento(_items);
    }

    public void Restore(ShoppingCartMemento memento)
    {
        _items = new List<string>(memento.Items);
        Console.WriteLine("Состояние корзины восстановлено");
        DisplayCart();
    }

    private void DisplayCart()
    {
        Console.WriteLine($"Корзина: {string.Join(", ", _items)}");
    }
}

public class CartCaretaker
{
    private Stack<ShoppingCartMemento> _history = new Stack<ShoppingCartMemento>();

    public void SaveState(ShoppingCart cart)
    {
        _history.Push(cart.Save());
    }

    public void Undo(ShoppingCart cart)
    {
        if (_history.Count > 0)
        {
            _history.Pop();
            if (_history.Count > 0)
            {
                cart.Restore(_history.Peek());
            }
            else
            {
                Console.WriteLine("Нет предыдущих состояний для восстановления");
            }
        }
        else
        {
            Console.WriteLine("История пуста");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        ShoppingCart cart = new ShoppingCart();
        CartCaretaker caretaker = new CartCaretaker();

        caretaker.SaveState(cart);


        cart.AddItem("Яблоко");
        caretaker.SaveState(cart);

        cart.AddItem("Апельсин");
        caretaker.SaveState(cart);

        cart.AddItem("Банан");
        caretaker.SaveState(cart);


        cart.RemoveItem("Яблоко");
        caretaker.SaveState(cart);


        Console.WriteLine("\n--- Отмена 1 ---");
        caretaker.Undo(cart);

        Console.WriteLine("\n--- Отмена 2 ---");
        caretaker.Undo(cart);

        Console.WriteLine("\n--- Отмена 3 ---");
        caretaker.Undo(cart);

        // Как вы будете хранить несколько точек сохранения(например, для отмены нескольких действий)?
        // Какие ограничения могут возникнуть при использовании этого паттерна?

        // Для хранения можно использовать коллекцию: список или стек (как я и сделал). 
        // Может быть проблема с памятью: если корзина имеет сложную структуру или большое количество данных, то процесс копирования может стать ресурсороемким,
        // а снимки занимать много места в памяти.
    }
}