// See https://aka.ms/new-console-template for more information
using DB.Data;

Console.WriteLine("Hello, World!");
string contex = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDBAssaabloy;User Id=administrador;Password=2022/M4n4t334zur3;";
using (var context = new PuertasTransmilenioDbassaabloyContext(contex)) 
{
    var latestMessages = context
        .TbMessages
        .Where(m => m.FechaHoraEnvioDato >= new DateTime())
        .OrderByDescending(m => m.FechaHoraEnvioDato)            
        .Take(30000)
        .ToList();

    Console.WriteLine(latestMessages.Count);
    foreach (var message in latestMessages)
    {
        Console.WriteLine(message.CodigoEvento);
    }
}