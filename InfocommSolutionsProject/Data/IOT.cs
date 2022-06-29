using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Microsoft.Azure.EventHubs;
using IotSensorWeb.Hubs;


namespace InfocommSolutionsProject.Data
{
    public class IOT
    {
        private const string EventHubsCompatibleEndpoint = "sb://ihsuprodsgres010dednamespace.servicebus.windows.net/";

        private const string EventHubsCompatiblePath = "iothub-ehub-it3681-00-18279741-4bca74d46b";

        private const string IotHubSasKey = "D/AuqJFR7mnaLXfgedX+zVnuiWYaEnkiuuZYEcrGVgQ=";

        private const string IotHubSasKeyName = "iothubowner";

        private static EventHubClient _eventHubClient;
        private static List<Task> _tasks;
        private readonly IServiceProvider _applicationServices;



        public IOT(IServiceProvider applicationServices)
        {
            _applicationServices = applicationServices;
            Run();
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition)
        {
            // Create the receiver using the default consumer group.
            // For the purposes of this sample, read only messages sent since 
            // the time the receiver is created. Typically, you don't want to skip any messages.
            var eventHubReceiver = _eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
            
            while (true)
            {
                var events = await eventHubReceiver.ReceiveAsync(100);
                System.Diagnostics.Debug.WriteLine("test!");
                if (events != null)
                {
                    System.Diagnostics.Debug.WriteLine("Hello there!12312321132213");
                    foreach (var eventData in events)
                    {
                        var data = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(eventData.Body.Array));
                   
                        System.Diagnostics.Debug.WriteLine($"data: {data}");
                        var hub = _applicationServices.GetService(typeof(IHubContext<SensorHub>)) as IHubContext<SensorHub>;
                        await hub.Clients.All.SendAsync("Broadcast", "Hugo", JsonConvert.SerializeObject(data));
                        //await hub.Clients.All.SendAsync("Broadcast", data);
                    }
                }
            }
        }
        private async void Run()
        {
            // Create an EventHubClient instance to connect to the
            // IoT Hub Event Hubs-compatible endpoint.
            var connectionString = new EventHubsConnectionStringBuilder(new Uri(EventHubsCompatibleEndpoint), EventHubsCompatiblePath, IotHubSasKeyName, IotHubSasKey);
            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());

            // Create a PartitionReciever for each partition on the hub.
            var runtimeInfo = await _eventHubClient.GetRuntimeInformationAsync();
            var d2cPartitions = runtimeInfo.PartitionIds;
            System.Diagnostics.Debug.WriteLine("Sussybaka423432");
            _tasks = new List<Task>();
            foreach (string partition in d2cPartitions)
            {
                _tasks.Add(ReceiveMessagesFromDeviceAsync(partition));
            }
        }
    }
       

}

