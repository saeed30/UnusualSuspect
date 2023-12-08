using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

// Create a connection to the hub
var connection = new HubConnectionBuilder()
  .WithUrl("http://185.252.28.45/GameHub")
  .Build();

// Register a handler for receiving messages from the hub
connection.On<string>("SendMessageToAll", (string message) =>
{
  Console.WriteLine($"Received: {message}");
});

// Start the connection
await connection.StartAsync();
Console.WriteLine("Connected to the hub");

// Loop until the user types 'exit'
while (true)
{
  // Read the user input
  var input = Console.ReadLine();

  // Check if the user wants to exit
  if (input == "exit")
  {
    break;
  }

  // Send the message to the hub
  await connection.SendAsync("ReceiveMessage", input);
  Console.WriteLine($"Sent: {input}");
}

// Stop the connection
await connection.StopAsync();
Console.WriteLine("Disconnected from the hub");