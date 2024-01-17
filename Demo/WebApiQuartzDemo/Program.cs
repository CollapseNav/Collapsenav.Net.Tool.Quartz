using Collapsenav.Net.Tool.Ext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
.AddSwaggerGen();
builder.Services
// .AddSingleton(new DIModel { Name = "123123" })
.AddAllJob()
.AddQuartzJsonConfig(builder.Configuration.GetSection("Quartz2"))
// .AddQuartzJsonConfig(builder.Configuration.GetSection("Quartz2"))
// .AddQuartzJsonConfig(new[] { new QuartzConfigNode { JobName = "DIJob", Len = 1 } })
.AddDefaultQuartzService()
;
var app = builder.Build();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.MapControllers();
app.Run();
