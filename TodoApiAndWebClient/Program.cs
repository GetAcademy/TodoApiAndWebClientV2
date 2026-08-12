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
    //new() { Id = 1, Text = "Handle melk", IsDone = false },
    //new() { Id = 2, Text = "Svare på e-post", IsDone = true }
};

app.MapGet("/todos", () =>
{
    return Results.Ok(todos);
});
app.MapGet("/todos/{id}", (int id) =>
{
    var todoItem = todos.FirstOrDefault(todo => todo.Id == id);

    return todoItem == null ? Results.NotFound() : Results.Ok(todoItem);
});
app.MapPost("/todos", (CreateTodoDto dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Text))
    {
        return Results.BadRequest("Text cannot be empty.");
    }

    var id = todos.Count == 0 ? 1 : todos.Max(t => t.Id) + 1;
    var todo = new TodoItem()
    {
        Id = id,
        Text = dto.Text,
        IsDone = false,
    };
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});
app.MapPut("/todos/{id}", (int id, UpdateTodoDto dto) =>
{
    var todo = todos.FirstOrDefault(todo => todo.Id == id);

    if (todo == null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrWhiteSpace(dto.Text))
    {
        return Results.BadRequest("Text cannot be empty.");
    }

    todo.Text = dto.Text;
    todo.IsDone = dto.IsDone;

    return Results.Ok(todo);
});
app.MapDelete("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(todo => todo.Id == id);

    if (todo == null)
    {
        return Results.NotFound();
    }

    todos.Remove(todo);

    return Results.NoContent();
});

/*
 * Alternativt flytte mest mulig til separat klasse
   app.MapGet("/todos", TodoService.GetAll);
   app.MapGet("/terje", TodoService.GetTerje);
 */
app.Run();
