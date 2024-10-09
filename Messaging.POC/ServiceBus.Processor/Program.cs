using Azure.Messaging.ServiceBus;
using ServiceBus.BLL.Implementations;
using System;
using System.Threading.Tasks;


string namespace_connection_string = "";
string queue_name = "";

ServiceBusQueueManager manager = new ServiceBusQueueManager(namespace_connection_string, queue_name);

await manager.StartListening(MessageHandler, ErrorHandler);






// handle received messages
async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");

    // complete the message. message is deleted from the queue. 
    await args.CompleteMessageAsync(args.Message);
}

// handle any errors when receiving messages
Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}