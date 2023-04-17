using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Threading;
using MQTT.Subscriber.BL;

namespace MQTT.Subscriber
{
    internal class Subscriber
    {
        static bool _keepRun = true;
        static SubscriberBL _subscriberBL = new SubscriberBL();
        static long _idLog;
        [Obsolete]
        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"====================================================");
            Console.WriteLine($"================= START PROCESS ===================");
            await InitProcess();

            while (_keepRun)
            {
                var command = Console.ReadLine();

                ReadLine(command);
            }

        }
        static async Task InitProcess()
        {
            string msg = string.Empty;
            try
            {

                DateTime dateTime = DateTime.UtcNow;
                _idLog = _subscriberBL.AddLogExecution();
                Console.WriteLine($">>>>>>>>>>>>>>> Date Start (UTC): {DateTime.UtcNow}");
                var topic = AppSettings.Instance.Configuration["appSettings:Topic"].ToString();
                var tcpServer = AppSettings.Instance.Configuration["appSettings:TCPServer"].ToString();
                var port = Convert.ToInt32(AppSettings.Instance.Configuration["appSettings:port"].ToString());
                var clientId = Guid.NewGuid().ToString();

                Console.WriteLine($">>>>>>>>>>>>>>> Id Log Execution: {_idLog}");
                Console.WriteLine($">>>>>>>>>>>>>>> Client MQTT Id: {clientId}");
                Console.WriteLine($">>>>>>>>>>>>>>> TCP Server: {tcpServer}");
                Console.WriteLine($">>>>>>>>>>>>>>> Port: {port}");
                Console.WriteLine($">>>>>>>>>>>>>>> Topic (ClientMQTT Tag): {topic}");
                Console.WriteLine($">>>>>>>>>>>>>>> Connecting to MQTT Server...");

                await ProcessMQTT(topic, tcpServer, port, clientId);
            }
            catch (Exception ex)
            {
                msg = $"{ex.Message} {ex.InnerException}";
                Console.WriteLine(msg);
                _subscriberBL.UpdateLogExecution(_idLog, msg);
            }
        }
        static async Task ProcessMQTT(string topic, string tcpServer, int port, string clientId)
        {
            var mqttFactory = new MqttFactory();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(tcpServer, port)
                .WithCleanSession()
                .Build();

            IMqttClient client = mqttFactory.CreateMqttClient();

            client.UseConnectedHandler(async e =>
            {
                Console.WriteLine($">>>>>>>>>>>>> Connected to the broker successfully");
                var topicFilter = new MqttTopicFilterBuilder()
                        .WithTopic(topic)
                        .Build();

                await client.SubscribeAsync(topicFilter);
            });


            client.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine($">>>>>>>>>>>>>>> Disconnected to the broker.");
                Console.WriteLine($">>>>>>>>>>>>> Reason: {e.Reason}.");

                Console.WriteLine($">>>>>>>>>>>>>>> MQTT Reconnecting...");
                await Task.Delay(TimeSpan.FromSeconds(5));
                await client.ConnectAsync(options, CancellationToken.None);
                _subscriberBL.UpdateLogExecution(_idLog, $"Disconnected to the broker. Reason: {e.Exception.Message} {e.Exception.InnerException}");
            });

            client.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine($"====================================================");
                Console.WriteLine($">>>>>>>>>>>>>>> Received message - {DateTime.UtcNow}:");

                string msgIn = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                Console.WriteLine($">>>>>>>>>>>>> {msgIn}");

                Console.WriteLine($">>>>>>>>>>>>> Processing message...");

                SubscriberBL subscriberBL = new SubscriberBL();
                subscriberBL.ProccessMessageIn(msgIn);
                Console.WriteLine($">>>>>>>>>>>>> Message Processed successfully - {DateTime.UtcNow}");

            });

            await client.ConnectAsync(options);
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (_keepRun)
            {
                _subscriberBL.UpdateLogExecution(_idLog, $"Abruptly closed");
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.FriendlyName);
            }
            else
            {
                _subscriberBL.UpdateLogExecution(_idLog, "Closed with SA permissions");
            }
        }

        static void ReadLine(string command)
        {
            try
            {
                if (command.ToUpper().Equals("EXIT"))
                {
                    Console.WriteLine($"===================");
                    Console.Write("User: ");
                    var user = Console.ReadLine();

                    if (string.IsNullOrEmpty(user))
                    {
                        Console.WriteLine("Wrong user");
                        Console.WriteLine($"===================");
                    }
                    else
                    {
                        Console.Write("Password: ");
                        var pwd = Console.ReadLine();

                        if (string.IsNullOrEmpty(pwd))
                        {
                            Console.WriteLine("Wrong password");
                            Console.WriteLine($"===================");
                        }
                        else
                        {
                            SubscriberBL subscriberBL = new SubscriberBL();
                            Console.WriteLine($"===================");
                            Console.WriteLine("Validating credentials...");
                            if (subscriberBL.CheckUser(user, pwd))
                            {
                                Console.WriteLine("Correct credentials, closing program...");
                                _keepRun = false;
                            }
                            else
                            {
                                Console.WriteLine("Wrong credentials");
                                Console.WriteLine($"===================");
                            }

                        }
                    }

                    if (!_keepRun)
                    {
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
