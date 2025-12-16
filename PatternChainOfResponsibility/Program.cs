using System;


public abstract class Handler
{
    protected Handler next;

    public void SetNext(Handler handler)
    {
        next = handler;
    }

    public abstract void HandleRequest(string request);
}

public class ManagerHandler : Handler
{
    public override void HandleRequest(string request)
    {
        if (request == "малый возврат")
        {
            Console.WriteLine("Менеджер обработал запрос");
        }
        else if (next != null)
        {
            Console.WriteLine("Менеджер передал запрос дальше");
            next.HandleRequest(request);
        }
    }
}

public class SupervisorHandler : Handler
{
    public override void HandleRequest(string request)
    {
        if (request == "средний возврат")
        {
            Console.WriteLine("Руководитель обработал запрос");
        }
        else if (next != null)
        {
            Console.WriteLine("Руководитель передал запрос дальше");
            next.HandleRequest(request);
        }
    }
}

public class SupportHandler : Handler
{
    public override void HandleRequest(string request)
    {
        if (request == "крупный возврат")
        {
            Console.WriteLine("Служба поддержки обработала запрос");
        }
        else
        {
            Console.WriteLine("Никто не смог обработать запрос");
        }
    }
}

class Program
{
    static void Main()
    {
        Handler manager = new ManagerHandler();
        Handler supervisor = new SupervisorHandler();
        Handler support = new SupportHandler();

        manager.SetNext(supervisor);
        supervisor.SetNext(support);

        Console.WriteLine("Запрос: малый возврат");
        manager.HandleRequest("малый возврат");

        Console.WriteLine("\nЗапрос: средний возврат");
        manager.HandleRequest("средний возврат");

        Console.WriteLine("\nЗапрос: крупный возврат");
        manager.HandleRequest("крупный возврат");

        Console.WriteLine("\nЗапрос: неизвестный запрос");
        manager.HandleRequest("неизвестный запрос");

        // Вопрос: Как вы будете обрабатывать ситуацию, когда запрос не обрабатывается ни одним из обработчиков? Какие изменения внесете в систему?

        // В конце цепочки добавлю звено, которое не передаёт запрос дальше и уведомляет отправителя об ошибке.
    }
}