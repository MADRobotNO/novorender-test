using novorender;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();


app.MapGet("/{files}", async (int files, CancellationToken cancellationToken) =>
{
    var memoryStream = new MemoryStream();

    await Task.Run(() => Zip.Write(memoryStream, files, cancellationToken), cancellationToken);

    // Prevent 'closed stream' error
    var copyStream = new MemoryStream(memoryStream.ToArray());

    // Trigger garbage collection
    GC.Collect();

    return Results.File(copyStream, "application/octet-stream", "test.zip");
});

app.Run();
