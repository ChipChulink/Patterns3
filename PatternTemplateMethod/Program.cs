public abstract class OrderProcessing
{
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"\n--- Начат процесс оформления заказа #{order.OrderId} ({this.GetType().Name}) ---");

        SelectItems(order);
        ChoosePaymentMethod(order);
        ChooseDeliveryMethod(order);
        FinalizeOrder(order);

        if (RequiresSpecialHandling())
        {
            ApplySpecialHandling(order);
        }

        Console.WriteLine($"--- Заказ #{order.OrderId} успешно обработан. ---");
    }

    protected abstract void ChoosePaymentMethod(Order order);
    protected abstract void ChooseDeliveryMethod(Order order);

    protected virtual void SelectItems(Order order)
    {
        Console.WriteLine("Шаг 1: Выбраны товары. Проверка наличия на складе.");
    }

    protected void FinalizeOrder(Order order)
    {
        order.Status = "Оформлен и ожидает выполнения";
        Console.WriteLine("Шаг 4: Финализация. Заказ занесен в базу данных.");
    }

    protected virtual bool RequiresSpecialHandling()
    {
        return false;
    }

    protected virtual void ApplySpecialHandling(Order order)
    {
    }
}

public class StandardOrderProcessing : OrderProcessing
{
    protected override void ChoosePaymentMethod(Order order)
    {
        order.Payment = "Наличными при получении";
        Console.WriteLine("Шаг 2: Выбран способ оплаты: Наличными при получении.");
    }

    protected override void ChooseDeliveryMethod(Order order)
    {
        order.Delivery = "Стандартная доставка (7-14 дней)";
        Console.WriteLine("Шаг 3: Выбран способ доставки: Стандартный пакет.");
    }
}

public class ExpressOrderProcessing : OrderProcessing
{
    protected override void ChoosePaymentMethod(Order order)
    {
        order.Payment = "Кредитная карта (онлайн)";
        Console.WriteLine("Шаг 2: Выбран способ оплаты: Кредитная карта (требуется немедленная оплата).");
    }

    protected override void ChooseDeliveryMethod(Order order)
    {
        order.Delivery = "Экспресс-доставка (1-2 дня) с повышенной стоимостью.";
        Console.WriteLine("Шаг 3: Выбран способ доставки: Курьерская Экспресс-служба.");
    }

    protected override bool RequiresSpecialHandling()
    {
        return true;
    }

    protected override void ApplySpecialHandling(Order order)
    {
        Console.WriteLine("Шаг 5: Дополнительная обработка: Срочный звонок в курьерскую службу.");
    }
}

public class Order
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public string Payment { get; set; }
    public string Delivery { get; set; }

    public Order(int id, decimal amount)
    {
        OrderId = id;
        TotalAmount = amount;
        Status = "Создан";
    }

    public void Process(OrderProcessing processor)
    {
        processor.ProcessOrder(this);
    }
}


public class PrepaidOrderProcessing : OrderProcessing
{
    protected override void ChoosePaymentMethod(Order order)
    {
        order.Payment = "Полная предоплата (обработана)";
        Console.WriteLine($"Шаг 2: Выбран способ оплаты: ПОЛНАЯ ПРЕДОПЛАТА. Проверка транзакции для суммы {order.TotalAmount:C}.");
    }

    protected override void ChooseDeliveryMethod(Order order)
    {
        order.Delivery = "Доставка после подтверждения оплаты (3-5 дней).";
        Console.WriteLine("Шаг 3: Выбран способ доставки: Доставка со склада после обработки платежа.");
    }

    protected override void SelectItems(Order order)
    {
        base.SelectItems(order);
        Console.WriteLine("Шаг 1.1: Дополнительная проверка кредитного рейтинга клиента для предоплаты.");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var order1 = new Order(1, 150.00m);
        order1.Process(new StandardOrderProcessing());

        var order2 = new Order(2, 500.00m);
        order2.Process(new ExpressOrderProcessing());

        // Демонстрация нового типа заказа (с предоплатой)
        var order3 = new Order(3, 999.99m);
        order3.Process(new PrepaidOrderProcessing());
    }
}