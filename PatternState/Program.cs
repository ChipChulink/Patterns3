using System;
using System.Collections.Generic;

public abstract class OrderState
{
    public abstract void ProcessOrder(Order order);
    public abstract string GetStatus();
}

public class NewState : OrderState
{
    public override void ProcessOrder(Order order)
    {
        Console.WriteLine("Обработка нового заказа...");
        order.SetState(new ProcessingState());
    }

    public override string GetStatus()
    {
        return "Новый";
    }
}

public class ProcessingState : OrderState
{
    public override void ProcessOrder(Order order)
    {
        Console.WriteLine("Заказ проверяется и готовится к отправке...");
        order.SetState(new ShippedState());
    }

    public override string GetStatus()
    {
        return "В обработке";
    }
}

public class ShippedState : OrderState
{
    public override void ProcessOrder(Order order)
    {
        Console.WriteLine("Заказ отправлен покупателю...");
        order.SetState(new DeliveredState());
    }

    public override string GetStatus()
    {
        return "Отправлен";
    }
}

public class DeliveredState : OrderState
{
    public override void ProcessOrder(Order order)
    {
        Console.WriteLine("Заказ уже доставлен. Дальнейшая обработка невозможна.");
    }

    public override string GetStatus()
    {
        return "Доставлен";
    }
}

public class CancelledState : OrderState
{
    public override void ProcessOrder(Order order)
    {
        Console.WriteLine("Заказ отменен. Дальнейшая обработка невозможна.");
    }

    public override string GetStatus()
    {
        return "Отменен";
    }
}

public class Order
{
    private OrderState _currentState;
    public string OrderId { get; }

    public Order(string orderId)
    {
        OrderId = orderId;
        _currentState = new NewState();
    }

    public void SetState(OrderState state)
    {
        _currentState = state;
    }

    public void Process()
    {
        Console.WriteLine($"Заказ #{OrderId}");
        _currentState.ProcessOrder(this);
        Console.WriteLine($"Новый статус: {_currentState.GetStatus()}");
        Console.WriteLine();
    }

    public void Cancel()
    {
        if (_currentState.GetStatus() != "Доставлен" && _currentState.GetStatus() != "Отменен")
        {
            _currentState = new CancelledState();
            Console.WriteLine($"Заказ #{OrderId} отменен.");
        }
        else
        {
            Console.WriteLine($"Невозможно отменить заказ со статусом: {_currentState.GetStatus()}");
        }
        Console.WriteLine();
    }

    public string GetStatus()
    {
        return _currentState.GetStatus();
    }
}

class Program
{
    static void Main(string[] args)
    {

        Order order = new Order("12345");

        order.Process(); 
        order.Process();
        order.Process();
        order.Process();

        Console.WriteLine("--- Создаем новый заказ ---");

        Order order2 = new Order("67890");
        order2.Process();

        order2.Cancel();
        order2.Process();


        // Как вы обеспечите корректность переходов между состояниями(например, нельзя вернуться из "Доставлен" в "В обработке")?
        // Какие методы добавите для управления состояниями?

        // Для корректности переходов нужно добавить метод, который проверяет возможность перехода между состояниями,
        // например, используя словарь валидных переходов:
        /*
        Dictionary < string, string[]> allowedTransitions = new Dictionary<string, string[]>
        {
            ["Новый"] = new[] { "В обработке", "Отменен" },
            ["В обработке"] = new[] { "Отправлен", "Отменен" },
            ["Отправлен"] = new[] { "Доставлен", "Отменен" }
        };
        */ 

    }
}