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
    }
}
