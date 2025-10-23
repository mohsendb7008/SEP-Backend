using SEP_Backend;

var builder = WebApplication.CreateBuilder(args);
AppBuilderBootstrap.Run(builder);
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.Run();