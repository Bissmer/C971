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
            _database.CreateTableAsync<Terms>().Wait();
        }

        public Task<List<Schemas.Terms>> GetTermsAsync()
        {
            return _database.Table<Terms>().ToListAsync();
        }

        public Task<List<Schemas.Terms>> GetTermAsync(int id)
        {
            return _database.Table<Schemas.Terms>()
                            .Where(i => i.Id == id)
                            .ToListAsync();
        }

        public Task<int> SaveTermAsync(Terms term)
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
        public Task<int> DeleteTermAsync(Terms term)
        {
            return _database.DeleteAsync(term);
        }
    }
}
