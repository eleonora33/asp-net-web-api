using Microsoft.EntityFrameworkCore;
using NotesAndTagsApp.DataAccess.Interfaces;
using NotesAndTagsApp.Domain.Models;

namespace NotesAndTagsApp.DataAccess.Implementations
{
    public class NoteRepository : IRepository<Note>
    {
        private readonly NotesAndTagsAppDbContext _notesAppDbContext;

        public NoteRepository(NotesAndTagsAppDbContext notesAppDbContext)
        {
            _notesAppDbContext = notesAppDbContext;
        }

        public void Add(Note entity)
        {
           _notesAppDbContext.Notes.Add(entity);
           _notesAppDbContext.SaveChanges();
        }

        public void Delete(Note entity)
        {
            _notesAppDbContext.Notes.Remove(entity);
            _notesAppDbContext.SaveChanges();
        }

        public List<Note> GetAll()
        {
            return _notesAppDbContext
                .Notes
                .Include(x => x.User)
                .ToList();
        }

        public Note GetById(int id)
        {
            return _notesAppDbContext
                .Notes
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Update(Note entity)
        {
            _notesAppDbContext .Notes .Update(entity);
            _notesAppDbContext.SaveChanges();
        }
    }
}
