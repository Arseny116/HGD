    using MassTransit;

    var builder = WebApplication.CreateBuilder(args);


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

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
