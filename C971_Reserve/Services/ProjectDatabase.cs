using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971_Reserve.Schemas;
using SQLite;

namespace C971_Reserve.Services
{
    public class ProjectDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public ProjectDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
        }

        public Task<List<Schemas.Term>> GetTermsAsync()
        {
            return _database.Table<Term>().ToListAsync();
        }

        public Task<List<Schemas.Term>> GetTermAsync(int id)
        {
            return _database.Table<Schemas.Term>()
                            .Where(i => i.Id == id)
                            .ToListAsync();
        }

        public Task<int> SaveTermAsync(Term term)
        {
            if (term.Id != 0)
            {
                return _database.UpdateAsync(term);
            }
            else
            {
                return _database.InsertAsync(term);
            }
        }
        public Task<int> DeleteTermAsync(Term term)
        {
            return _database.DeleteAsync(term);
        }

        public Task<List<Course>> GetCoursesAsync(int termId)
        {
            return _database.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        public Task<int> SaveCourseAsync(Course course)
        {
            if(course.Id != 0)
            {
                return _database.UpdateAsync(course);
            }
            else
            {
                return _database.InsertAsync(course);
            }
        }

        public Task<int> DeleteCourseAsync(Course course)
        {
            return _database.DeleteAsync(course);
        }
    }
}
