
public interface IPaymentStrategy
{
    void ProcessPayment(float amount);
}


public class CreditCardPayment : IPaymentStrategy
{
    public void ProcessPayment(float amount)
    {
        Console.WriteLine($"Оплата {amount} будет произведена с помощью кредитной карты.");
    }
}


public class OnlineWallet : IPaymentStrategy
{
    public void ProcessPayment(float amount)
    {
        Console.WriteLine($"Оплата {amount} будет произведена c помощью онлайн кошелька.");
    }
}


public class Order
{
    private IPaymentStrategy _paymentStrategy;
    private float _totalAmount;

    public Order(float totalAmount)
    {
        _totalAmount = totalAmount;
    }

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy;
        Console.WriteLine($"--- Установлен новый метод оплаты: {_paymentStrategy.GetType().Name.Replace("Payment", "")} ---");
    }

    public void Checkout()
    {
        if (_paymentStrategy == null)
        {
            Console.WriteLine("Не выбран метод оплаты");
            return;
        }

        Console.WriteLine($"Обработка заказа на сумму {_totalAmount}");
        _paymentStrategy.ProcessPayment(_totalAmount);
        Console.WriteLine("Заказ успешно оформлен.\n");
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        float orderAmount1 = 1499.99f;
        Order order1 = new Order(orderAmount1);

        order1.SetPaymentStrategy(new CreditCardPayment());
        order1.Checkout();

        float orderAmount2 = 999.00f;
        Order order2 = new Order(orderAmount2);

        order2.SetPaymentStrategy(new OnlineWallet());
        order2.Checkout();

        // -------------------------Инструкция по добавлению нового метода оплаты-------------------------------------------

        /*                
        1. Создать новый класс-стратегию: например, CryptocurrencyPayment.
        
        2. Реализовать интерфейс: этот новый класс должен реализовать интерфейс IPaymentStrategy.

        Пример:
           public class CryptocurrencyPayment : IPaymentStrategy
           {
               public void ProcessPayment(float amount)
               {
                   Console.WriteLine($"Оплата {amount} будет произведена через Криптовалюту.");
               }
           }
                      
        3. Использовать новую стратегию: создать экземпляр нового класса и передать его объекту 
        */
    }
}