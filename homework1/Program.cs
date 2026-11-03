using homework1.classes;
using homework1.interfaces;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IUserCounter,UserCounter>();
builder.Services.AddTransient<IDateTimeService,DateTimeService>();

var app = builder.Build();


app.Use ( async (context, next) =>
{
    if(context.Request.Path == "/counter")
    {
        IUserCounter counter = context.RequestServices.GetRequiredService<IUserCounter>();
        int views = counter.UpdateCounter();
        
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync($"Welcome user {views}");
        return;
    }
    await next();
});

app.Use (async (context, next) =>
{   
    if(context.Request.Path == "/time")
    {
        IDateTimeService timeService = context.RequestServices.GetRequiredService<IDateTimeService>();
        await context.Response.WriteAsync($"Current time is: {timeService.Time}");
        return;
    }
    await next();
});

app.Use(async (context, next) =>
{
    if(context.Request.Path == "/items")
    {
        var products = new Product[]
        {
            new Product {Name = "product 1"},
            new Product {Name = "product 2"},
            new Product {Name = "product 3"},
            new Product {Name = "product 4"},
            new Product {Name = "product 5"},
            new Product {Name = "product 6"},
            new Product {Name = "product 7"},
            new Product {Name = "product 8"},
            new Product {Name = "product 9"},
            new Product {Name = "product 10"},

        }; 
        var rows = string.Join("", products.Select(u => $"<tr><td>{u.Name}</td></tr>"));

        context.Response.ContentType = "text/html; charset=utf-8";

        await context.Response.WriteAsync($@"
            <!DOCTYPE html>
            <html>
            <head><title>Current Time</title></head>
            <body>
                <table border='1'>
                    <tr><th>Product Name</th></tr>
                    {rows}
                </table>
            </body>
            </html>
        ");
        return;
    }
    await next();
});

app.Use(async (context, next) =>
{
    if(context.Request.Path == "/headers")
    {
        context.Response.ContentType = "text/html; charset=utf-8";

        var rows = string.Join("", context.Request.Headers.Select(h => $"<tr ><td>{h.Key}</td> <td>{h.Value}</td> </tr>"));

        await context.Response.WriteAsync($@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Request Headers</title>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 30px; }}
                    table {{ border-collapse: collapse; width: 100%; }}
                    th, td {{ border: 1px solid #ccc; padding: 10px; text-align: left; }}
                    th {{ background-color: #f4f4f4; }}
                </style>
            </head>
            <body>
                <h2>All headers for you, Olexandr</h2>
                <table border='1'>
                    {rows}
                </table>
            </body>
            </html>
        ");
        return;
    }
    await next();
});

app.Use (async (context, next) =>
{
    if(context.Request.Path == "/query")
    {
        string name = context.Request.Query["name"]!;
        string surname = context.Request.Query["surname"]!;
        string age = context.Request.Query["age"]!;
        string city = context.Request.Query["city"]!;
        string profession = context.Request.Query["profession"]!;
        string hobby = context.Request.Query["hobby"]!;
        string email = context.Request.Query["email"]!;

        context.Response.ContentType = "text/html; charset=utf-8";
        
        await context.Response.WriteAsync($@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>User profile</title>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 30px; background-color: #f9f9f9; }}
                    .profile-card {{ background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); width: 400px; }}
                    table {{ border-collapse: collapse; width: 100%; margin-top: 15px; }}
                    th, td {{ border: 1px solid #ddd; padding: 10px; text-align: left; }}
                    th {{ background-color: #4CAF50; color: white; }}
                </style>
            </head>
            <body>
                <div class='profile-card'>
                    <h2>Perfect profile of user</h2>
                    <table>
                        <tr><th>Характеристика</th><th>Значення</th></tr>
                        <tr><td><strong>Name</strong></td><td>{name}</td></tr>
                        <tr><td><strong>Surname</strong></td><td>{surname}</td></tr>
                        <tr><td><strong>Age</strong></td><td>{age}</td></tr>
                        <tr><td><strong>City</strong></td><td>{city}</td></tr>
                        <tr><td><strong>Profession</strong></td><td>{profession}</td></tr>
                        <tr><td><strong>Hobby</strong></td><td>{hobby}</td></tr>
                        <tr><td><strong>Email</strong></td><td>{email}</td></tr>
                    </table>
                </div>
            </body>
            </html>
        ");
        return;
    }
    await next();
});

app.Run();
