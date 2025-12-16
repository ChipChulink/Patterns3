public class Order
{
    private string _status;
    public int OrderId { get; }

    public class OrderStatusChangedEventArgs : EventArgs
    {
        public int OrderId { get; }
        public string NewStatus { get; }
        public DateTime Timestamp { get; }

        public OrderStatusChangedEventArgs(int orderId, string newStatus)
        {
            OrderId = orderId;
            NewStatus = newStatus;
            Timestamp = DateTime.Now;
        }
    }

    public event EventHandler<OrderStatusChangedEventArgs> StatusChanged;

    public Order(int orderId)
    {
        OrderId = orderId;
        _status = "Оформлен";
        Console.WriteLine($"Создан Заказ #{OrderId}. Изначальный статус: {_status}");
    }

    public void ChangeStatus(string newStatus)
    {
        if (_status != newStatus)
        {
            _status = newStatus;

            Console.WriteLine($"\n*** Статус Заказа #{OrderId} изменился на '{_status}' ***");

            StatusChanged?.Invoke(this, new OrderStatusChangedEventArgs(OrderId, newStatus));
        }
    }
}


public class ClientNotification
{
    public void HandleStatusChange(object sender, Order.OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"[ClientNotification]: Уважаемый клиент, статус Заказа #{e.OrderId} изменен на '{e.NewStatus}'.");
    }
}

public class ManagerNotification
{
    public void HandleStatusChange(object sender, Order.OrderStatusChangedEventArgs e)
    {
        if (e.NewStatus == "Отправлен") Console.WriteLine($"[ManagerNotification]: Заказ #{e.OrderId} отправлен. Требуется проверка трек-номера.");
        else Console.WriteLine($"[ManagerNotification]: Статус Заказа #{e.OrderId}: {e.NewStatus}. Требуется проверка.");
    }
}

public class AnalyticsSystem
{
    public void HandleStatusChange(object sender, Order.OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"[AnalyticsSystem]: Логирование. Заказ #{e.OrderId} перешел в статус '{e.NewStatus}'. Время: {e.Timestamp:HH:mm:ss}.");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var order = new Order(2002);

        var client = new ClientNotification();
        var manager = new ManagerNotification();
        var analytics = new AnalyticsSystem();

        order.StatusChanged += client.HandleStatusChange;
        order.StatusChanged += manager.HandleStatusChange;
        order.StatusChanged += analytics.HandleStatusChange;

        order.ChangeStatus("В обработке");

        order.StatusChanged -= analytics.HandleStatusChange;

        order.ChangeStatus("Отправлен");

        // Вопрос: Есть ли необходимость добавления дополнительных классов или методов для обеспечения безопасности? Почему?

        // Да, есть необходимость в дополнительных механизмах безопасности, 
        // если речь идет о защите данных, контроле доступа или предотвращении нежелательных действий.
    }
}