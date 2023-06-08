using MQTT.Processor.BL;
using System;

namespace MQTT.Processor
{
    internal class Processor
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"========= INICIO PROCESO {DateTime.UtcNow.ToString()}========");
            ProcessorBL processorBL = new ProcessorBL();

            processorBL.ProcessMessages();

        }
    }
}
