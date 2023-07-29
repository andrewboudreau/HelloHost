using System;

public class MyMessage
{
    public DateTime Date { get; private set; }
    public Guid Id { get; private set; }

    public MyMessage()
    {
        Date = DateTime.Now;
        Id = Guid.NewGuid();
    }
}
