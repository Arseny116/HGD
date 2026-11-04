using Amazon.S3;
using MassTransit;
using Microsoft.Extensions.Options;
using PdfWorker.Configuration;
using PdfWorker.Services;
using PdfWorker.Services.Storage;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddScoped<PdfGenerator>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Configure<S3Options>(builder.Configuration.GetSection("S3"));
builder.Services.AddScoped<S3FileStorage>();
builder.Services.AddSingleton<IAmazonS3>(serviceProvider =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<S3Options>>()
        .Value;

    var config = new AmazonS3Config
    {
        ServiceURL = options.ServiceUrl,
        ForcePathStyle = options.ForcePathStyle
    };

    return new AmazonS3Client(
        options.AccessKey,
        options.SecretKey,
        config);
});

// Переделать более чисто с .Configure 
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
