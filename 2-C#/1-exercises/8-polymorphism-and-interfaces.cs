/* --------------------------------- Polymorphism & Interfaces --------------------------------- */

/* Method Overriding:
   Create a base class Notification with a virtual void Send(string message) method. 
   Derive two classes — EmailNotification and SmsNotification — each with override Send(). 
   Call via a Notification reference to confirm dynamic dispatch. */

class Notification
{
    public virtual void Send(string message)
    {
        Console.WriteLine(message);
    }
}

class EmailNotification() : Notification
{
    public override void Send(string message)
    {
        Console.WriteLine(message);
    }
}

class SmsNotification() : Notification
{
    public override void Send(string message)
    {
        Console.WriteLine(message);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Notification notificationMessage = new Notification();
        notificationMessage.Send("Notification sent.");

        EmailNotification emailNotificationMessage = new EmailNotification();
        emailNotificationMessage.Send("Email notification sent.");

        SmsNotification smsNotificationMessage = new SmsNotification();
        smsNotificationMessage.Send("SMS notification sent.");
    }
}


/* Interface Contract
   Declare an interface INotifier with a void Send(string message) method. 
   Refactor EmailNotification and SmsNotification to implement INotifier. 
   Inject INotifier into an OrderService constructor — demonstrating polymorphism through interfaces. */

interface INotifier
{
    void Send(string message);
}

class Notification : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine("Sending notification: " + message);
    }
}

class EmailNotification() : Notification, INotifier
{
    public void Send(string message)
    {
        Console.WriteLine("Sending email notification: " + message);
    }
}

class SmsNotification() : Notification, INotifier
{
    public void Send(string message)
    {
        Console.WriteLine("Sending SMS notification: " + message);
    }
}

class OrderService
{
    private INotifier _notifier;

    public OrderService(INotifier notifier)
    {
        _notifier = notifier;
    }

    public void SendOrder(string orderId)
    {
        _notifier.Send($"your order number {orderId} has been confirmed.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        INotifier notificationMessage = new Notification();
        notificationMessage.Send("You have 1 message.");

        INotifier emailNotificationMessage = new EmailNotification();
        emailNotificationMessage.Send("Want to receive more updates?");

        INotifier smsNotificationMessage = new SmsNotification();
        smsNotificationMessage.Send("OTP: 1234");

        OrderService orderServiceNotification = new OrderService(new Notification());
        orderServiceNotification.SendOrder("O-12");
    }
}