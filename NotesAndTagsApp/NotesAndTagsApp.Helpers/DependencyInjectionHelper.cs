using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using NotesAndTagsApp.DataAccess;
using NotesAndTagsApp.DataAccess.Implementations;
using NotesAndTagsApp.DataAccess.Interfaces;
using NotesAndTagsApp.Domain.Models;
using NotesAndTagsApp.Services.Implementation;
using NotesAndTagsApp.Services.Interfaces;

namespace NotesAndTagsApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection service)
        {
            service.AddDbContext<NotesAndTagsAppDbContext>(x =>
            x.UseSqlServer("Server=.\\SQLExpress;Database = NotesAppDb;Trusted_Connection=True; TrustServerCertificate = True"));
        }

        public static void InjectRepositories(IServiceCollection service)
        {
            service.AddTransient<IRepository<Note>, NoteRepository>();
            service.AddTransient<IUserRepository, UserRepository>();
        }

        public static void InjectServices(IServiceCollection service)
        {
            service.AddTransient<INoteService, NoteService>();
            service.AddTransient<IUserService, UserService>();
        }     
    }
}
