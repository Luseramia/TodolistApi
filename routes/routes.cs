
using System.Dynamic;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using models.ToDoList;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
namespace Routes;

public static class RouteConfig
{
    public static void ConfigureRoutes(this WebApplication app)
    {
     var noAuthRoute = new Dictionary<string, Func<ExpandoObject, MySqlConnection, HttpContext, Task<IResult>>>
        {
            {"/getTodoList",ToDoList.GetTodoLists},
             {"/insert-todolist",ToDoList.InsertTodo},
        };

        string connectionString = "Server=192.168.1.53;Database=ToDoList;User ID=root;Password=FROMIS_9;";
        foreach (var route in noAuthRoute)
        {
            app.MapPost(route.Key, async Task<IResult> (HttpContext context) =>
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                try
                {
                    connection.Open();

                    using var reader = new StreamReader(context.Request.Body);
                    var body = await reader.ReadToEndAsync();
                    var jsonBody = JsonSerializer.Deserialize<ExpandoObject>(body);

                    // Directly use the connection here, while it is still open and valid
                    var result = route.Value(jsonBody, connection, context); // Pass the open connection

                    return  await result; // Return the result to the client
                    // }

                }
                catch (Exception ex)
                {
                    // Log and return an error response if something goes wrong
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return Results.Problem("An error occurred while processing your request.");
                }

            });
        }
   

    }

}


