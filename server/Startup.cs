using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using System.Text;

namespace server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {                
                string first_name;
                //string last_name;
                //string genre;
                //string instrument;
                //string message;

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "pluggedindb.database.windows.net";
                builder.UserID = "sqladmin";
                builder.Password = "$had0wrunBlu3";
                builder.InitialCatalog = "pluggedin_db";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT first_name ");
                    sb.Append("FROM musicians ");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                first_name = reader.GetString(0);
                                //last_name = reader.GetString(1);
                                //genre = reader.GetString(2);
                                //instrument = reader.GetString(3);

                                //message = first_name + last_name + " a " + genre + " musician plays the " + instrument;
                                await context.Response.WriteAsync("Hello World! \n" + first_name);
                            }
                        }
                    }
                }

            });
        }
    }
}
