using Azure.Messaging.ServiceBus;
using ServiceBus.BLL.Implementations;
using System;
using System.Threading.Tasks;


string namespace_connection_string = "";
string queue_name = "";



try
{
    ServiceBusQueueManager manager = new ServiceBusQueueManager(namespace_connection_string, queue_name);

    await manager.StartListening(MessageHandler, ErrorHandler);
    Console.WriteLine($"\nListening Started.");

    Console.WriteLine($"\nEnter message One:");
    string message1 = Console.ReadLine();

    await manager.SendMessage(message1);


    Console.WriteLine($"\nMessage One send. Press any key.");
    Console.ReadLine();


    Console.WriteLine($"\nEnter message Two: ");
    string message2 = Console.ReadLine();


    await manager.SendMessage(message2);

    Console.WriteLine($"\nMessage Two send. Press any key to exit.");
    Console.ReadLine();

    await manager.StopListening();
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.Message}");
}


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