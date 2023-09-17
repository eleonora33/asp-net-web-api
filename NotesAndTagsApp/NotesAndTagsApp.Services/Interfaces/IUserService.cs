using NotesAndTagsApp.DTOs;

namespace NotesAndTagsApp.Services.Interfaces
{
    public interface IUserService
    {
        void Register(RegisterUserDto registerUserDto);
        string LoginUser(LoginDto loginDto);
    }
}
