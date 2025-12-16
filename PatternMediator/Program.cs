public abstract class Mediator
{
    public abstract void Notify(object sender, string eventName);
}

public abstract class Component
{
    protected Mediator _mediator;

    public Component(Mediator mediator)
    {
        _mediator = mediator;
    }

    public abstract void Send(string message);
    public abstract void Receive(string message);
}

public class OrderMediator : Mediator
{
    private Client _client;
    private Manager _manager;
    private Warehouse _warehouse;

    public void SetComponents(Client client, Manager manager, Warehouse warehouse)
    {
        _client = client;
        _manager = manager;
        _warehouse = warehouse;
    }

    public override void Notify(object sender, string eventName)
    {
        switch (eventName)
        {
            case "PlaceOrder":
                Console.WriteLine("Посредник: Заказ получен, уведомляю менеджера...");
                _manager.Receive("Новый заказ от клиента");
                break;
            case "ApproveOrder":
                Console.WriteLine("Посредник: Заказ подтвержден, уведомляю склад...");
                _warehouse.Receive("Подготовить товар к отправке");
                break;
            case "PrepareOrder":
                Console.WriteLine("Посредник: Заказ готов, уведомляю клиента...");
                _client.Receive("Ваш заказ готов к отправке");
                break;
            case "CancelOrder":
                Console.WriteLine("Посредник: Заказ отменен, уведомляю все компоненты...");
                _manager.Receive("Заказ отменен");
                _warehouse.Receive("Отмена подготовки заказа");
                _client.Receive("Ваш заказ отменен");
                break;
        }
    }
}

public class Client : Component
{
    public Client(Mediator mediator) : base(mediator) { }

    public override void Send(string message)
    {
        Console.WriteLine($"Клиент: {message}");
        _mediator.Notify(this, "PlaceOrder");
    }

    public override void Receive(string message)
    {
        Console.WriteLine($"Клиент получил: {message}");
    }
}

public class Manager : Component
{
    public Manager(Mediator mediator) : base(mediator) { }

    public override void Send(string message)
    {
        Console.WriteLine($"Менеджер: {message}");
        _mediator.Notify(this, "ApproveOrder");
    }

    public override void Receive(string message)
    {
        Console.WriteLine($"Менеджер получил: {message}");

        if (message == "Новый заказ от клиента")
        {
            Console.WriteLine("Менеджер: Проверяю заказ...");
            Send("Заказ подтвержден");
        }
    }
}

public class Warehouse : Component
{
    public Warehouse(Mediator mediator) : base(mediator) { }

    public override void Send(string message)
    {
        Console.WriteLine($"Склад: {message}");
        _mediator.Notify(this, "PrepareOrder");
    }

    public override void Receive(string message)
    {
        Console.WriteLine($"Склад получил: {message}");

        if (message == "Подготовить товар к отправке")
        {
            Console.WriteLine("Склад: Готовлю товар...");
            Send("Заказ упакован и готов к отправке");
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
        OrderMediator mediator = new OrderMediator();

        Client client = new Client(mediator);
        Manager manager = new Manager(mediator);
        Warehouse warehouse = new Warehouse(mediator);

        mediator.SetComponents(client, manager, warehouse);

        Console.WriteLine("----Клиент размещает заказ:");
        client.Send("Хочу купить арбуз");

        Console.WriteLine("\n----Отмена заказа:");
        mediator.Notify(client, "CancelOrder");


        // Вопрос: Как вы обеспечите безопасность при обработке сообщений между компонентами? Какие дополнительные проверки добавите?

        // 1. Валидация отправителя: проверка, что сообщение пришло от авторизованного компонента
        // 2. Проверка допустимых событий: валидация eventName в Notify() через enum или список разрешенных событий

    }
}