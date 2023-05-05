// See https://aka.ms/new-console-template for more information
using DB.Data;

Console.WriteLine("Hello, World!");

using (var context = new MqttservicesbdContext()) 
{
    Console.WriteLine(context.TbMessages.ToList().Count);
    foreach(var comands in context.TbMessages.ToList())
    {
        Console.WriteLine(comands.FechaHoraEnvioDato );

    }
}