using Jay.Core.Application.Interfaces.Repositories;
using Jay.Infrastructure.Persistence.Contexts;
using Jay.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Infrastructure.Persistence
{
    public static class PServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("useInMemoryDB"))
            {
                services.AddDbContext<JayContext>(
                    options => options.UseInMemoryDatabase("JayDB"));
            }
            else {
                services.AddDbContext<JayContext>(
                    options => options.UseSqlServer(
                        configuration.GetConnectionString("JayConnection"),
                        m=>m.MigrationsAssembly(typeof(JayContext).Assembly.FullName)));
            }

            //Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IFriendRepository, FriendRepository>();
        }
    }
}
