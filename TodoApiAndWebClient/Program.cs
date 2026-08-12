using TodoApiAndWebClient;
using TodoApiAndWebClient.DTOs;
using TodoApiAndWebClient.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer(); // Crucial for Minimal APIs to discover endpoints
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Generates the JSON endpoint (e.g., /swagger/v1/swagger.json)
    app.UseSwaggerUI(); // Generates the interactive UI web page (/swagger)
}
app.UseHttpsRedirection();

// All kode rett i program:
var todos = new List<TodoItem>
{
    new() { Id = 1, Text = "Handle melk", IsDone = false },
    new() { Id = 2, Text = "Svare på e-post", IsDone = true }
};

app.MapGet("/todos", () =>
{
    return todos;
});
app.MapGet("/todos/{id}", (int id) =>
{
    return todos.FirstOrDefault(todo => todo.Id == id);
});
app.MapGet("/terje", () =>
{
    return new { FirstName = "Terje", LastName = "Kolderup" };
});
app.MapPost("/todos", (CreateTodoDto createTodoDto) =>
{
    var todoItem = new TodoItem()
    {
        Id = todos.Max(t => t.Id) + 1,
        Text = createTodoDto.Text,
        IsDone = false,
    };
    todos.Add(todoItem);
    return todoItem;
});

/*
 * Alternativt flytte mest mulig til separat klasse
   app.MapGet("/todos", TodoService.GetAll);
   app.MapGet("/terje", TodoService.GetTerje);
 */
app.Run();
