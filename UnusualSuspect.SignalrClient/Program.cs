using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

string? input;
Console.WriteLine("Write jwt token: ");
input = Console.ReadLine();
if (input == null)
  return;
//Create a connection to the hub
var connection = new HubConnectionBuilder()
  .WithUrl("http://185.252.28.45/GameHub"
    , options =>
  {
    options.AccessTokenProvider = () => Task.FromResult(input);
  }
  )
  .WithAutomaticReconnect()
.Build();

// Register a handler for receiving messages from the hub
connection.On<string, string>("ReceiveMessage", (user, message) =>
{
  Console.WriteLine($"Received: {user + " - " + message}");
});
// Start the connection
await connection.StartAsync();
Console.WriteLine("Connected to the hub");

// Loop until the user types 'exit'
while (true)
{
  // Read the user input
  input = Console.ReadLine();

  // Check if the user wants to exit
  if (input == "exit")
  {
    break;
  }

  // Send the message to the hub
  await connection.SendAsync("SendMessage", "username", input);
  Console.WriteLine($"Sent: {input}");
}

// Stop the connection
await connection.StopAsync();
Console.WriteLine("Disconnected from the hub");