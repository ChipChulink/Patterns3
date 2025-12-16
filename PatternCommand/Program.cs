using PatternCommand;

public abstract class Command
{
    protected Elevator Receiver;

    public Command(Elevator receiver)
    {
        Receiver = receiver;
    }

    public abstract void Execute();
    public abstract void Unexecute();
}

public class MoveUpCommand : Command
{
    public MoveUpCommand(Elevator receiver) : base(receiver) { }

    public override void Execute()
    {
        Receiver.MoveTo(Receiver.CurrentFloor + 1);
    }

    public override void Unexecute()
    {
        Receiver.MoveTo(Receiver.CurrentFloor - 1);
    }
}

public class MoveDownCommand : Command
{
    public MoveDownCommand(Elevator receiver) : base(receiver) { }

    public override void Execute()
    {
        Receiver.MoveTo(Receiver.CurrentFloor - 1);
    }

    public override void Unexecute()
    {
        Receiver.MoveTo(Receiver.CurrentFloor + 1);
    }
}

public class OpenDoorCommand : Command
{
    public OpenDoorCommand(Elevator receiver) : base(receiver) { }

    public override void Execute()
    {
        Receiver.OpenDoor();
    }

    public override void Unexecute()
    {
        Receiver.CloseDoor();
    }
}

public class CloseDoorCommand : Command
{
    public CloseDoorCommand(Elevator receiver) : base(receiver) { }

    public override void Execute()
    {
        Receiver.CloseDoor();
    }

    public override void Unexecute()
    {
        Receiver.OpenDoor();
    }
}

public class CommandHistory
{
    private List<Command> _history = new List<Command>();

    public void SaveCommand(Command command)
    {
        _history.Add(command);
    }
    public Command GetLastCommand()
    {
        if (_history.Count > 0)
        {
            return _history.Last();
        }
        return null;
    }

    public void RemoveLastCommand()
    {
        if (_history.Count > 0)
        {
            _history.RemoveAt(_history.Count - 1);
        }
    }

    public int Count => _history.Count;
}


public class LiftControl
{
    private CommandHistory _history = new CommandHistory();
    private Elevator _elevator;

    public LiftControl(Elevator elevator)
    {
        _elevator = elevator;
    }

    public void ExecuteCommand(Command command)
    {
        Console.WriteLine("\n--- Выполнение новой команды ---");
        command.Execute();
        _history.SaveCommand(command);
        Console.WriteLine($"Статус после выполнения: Этаж:{_elevator.CurrentFloor}, Дверь {(_elevator.DoorIsOpened ? "открыта" : "закрыта")}");
    }

    public void UndoLastCommand()
    {
        Command lastCommand = _history.GetLastCommand();

        if (lastCommand != null)
        {
            Console.WriteLine("\n--- Отмена последней команды ---");
            lastCommand.Unexecute();
            _history.RemoveLastCommand();
            Console.WriteLine($"Статус после отмены: {_elevator.CurrentFloor}, Дверь {(_elevator.DoorIsOpened ? "открыта" : "закрыта")}");
        }
        else
        {
            Console.WriteLine("\nИстория команд пуста. Нечего отменять.");
        }
    }

    public CommandHistory History => _history;
}

public class Program
{
    public static void Main(string[] args)
    {
        Building building = Building.GetInstance();

        Elevator elevator = building.Elevator;
        var controller = new LiftControl(elevator);

        controller.ExecuteCommand(new OpenDoorCommand(elevator));
        controller.ExecuteCommand(new CloseDoorCommand(elevator));
        controller.ExecuteCommand(new MoveUpCommand(elevator));
        controller.ExecuteCommand(new MoveUpCommand(elevator));

        controller.UndoLastCommand();


        Console.WriteLine("\nОтмена нескольких команд");
        UndoMultipleCommands(controller, 2);

        Console.WriteLine($"\nВсего команд в истории: {controller.History.Count}");
    }


    public static void UndoMultipleCommands(LiftControl controller, int count)
    {
        for (int i = 0; i < count; i++)
        {
            controller.UndoLastCommand();
        }
    }
}

