using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoFunctionApp;
using ToDoFunctionApp.Models;

namespace TodoFunctionApp;

public class TodoFunction
{
    private readonly ILogger<TodoFunction> _logger;

    public TodoFunction(ILogger<TodoFunction> logger)
    {
        _logger = logger;
    }

    [Function("GetTodos")]
    public async Task<HttpResponseData> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequestData req)
    {
        var todos = TodoService.GetAll();
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonSerializer.Serialize(todos));
        return response;
    }

    [Function("AddTodo")]
    public async Task<HttpResponseData> AddTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] HttpRequestData req)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var body = await JsonSerializer.DeserializeAsync<Todo>(req.Body, options);

        if (body == null || string.IsNullOrEmpty(body.Task))
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Invalid Body");
            return bad;
        }

        var added = TodoService.Add(body.Task);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonSerializer.Serialize(added));
        return response;
    }
}