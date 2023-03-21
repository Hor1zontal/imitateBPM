using Microsoft.AspNetCore.Http.Features;
using SqlSugar;

namespace imitateBPM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("http://*:8000");
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                "AllowAll",
                builder => builder
                .WithOrigins("http://localhost:8000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                );
            });

            //builder.Services.Configure<FormOptions>(options =>
            //{
            //    options.ValueLengthLimit = 209715200;
            //});

            // Add services to the container.

            //注册上下文：AOP里面可以获取IOC对象，如果有现成框架比如Furion可以不写这一行
            builder.Services.AddHttpContextAccessor();
            //注册SqlSugar
            builder.Services.AddSingleton<ISqlSugarClient>(s =>
            {
                SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
                {
                    
                    DbType = DbType.SqlServer,
                    ConnectionString = "Data Source=.; Initial Catalog = ParkerBPM; Integrated Security = True; MultipleActiveResultSets = true",
                    IsAutoCloseConnection = true,
                },
               db =>
               {
                    //单例参数配置，所有上下文生效
                   db.Aop.OnLogExecuting = (sql, pars) =>
                   {
                        //获取作IOC作用域对象
                       //var appServive = builder.Services.BuildServiceProvider().GetService<IHttpContextAccessor>();
                       //var obj = appServive?.HttpContext?.RequestServices.GetService<>();
                       Console.WriteLine(sql);
                   };
                   db.Aop.OnLogExecuted = (sql, pars) =>
                   {
                       Console.WriteLine($"执行时间：{db.Ado.SqlExecutionTime.TotalMilliseconds}");
                   };
                        db.Aop.OnError = (exp) =>
            {
                Console.WriteLine(exp.Sql);
            };
               });
                return sqlSugar;
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}