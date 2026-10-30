using MassTransit;
using PdfWorker.Services;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddScoped<PdfGenerator>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
IConfiguration configuration = builder.Configuration;
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PdfGenerationConsumer>();


    //context для доступа из di(подтягивает все зависимости обработчика) , cfg для подключения к рэбиту 
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            configuration["RabbitMQ:Host"] ?? "localhost",
            "/",
            h =>
            {
                h.Username(
                    configuration["RabbitMQ:Username"] ?? "guest");

                h.Password(
                    configuration["RabbitMQ:Password"] ?? "guest");
            });

        cfg.ReceiveEndpoint("pdf-generation-queue", e =>
        {
            e.ConfigureConsumer<PdfGenerationConsumer>(context);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
