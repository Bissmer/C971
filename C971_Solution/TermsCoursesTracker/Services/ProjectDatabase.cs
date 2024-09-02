using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermsCoursesTracker.Schemas;
using SQLite;

namespace TermsCoursesTracker.Services
{
    public class ProjectDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public ProjectDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            CreateTableAsync().Wait();
        }

        private async Task CreateTableAsync()
        {
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
        } 

        #region Term data manipulation

        /// <summary>
        /// Fetches all terms from the database
        /// </summary>
        /// <returns></returns>
        public Task<List<Term>> GetTermsAsync()
        {
            return _database.Table<Term>().ToListAsync();
        }

        /// <summary>
        /// Fetches a term by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Term> GetTermAsync(int id)
        {
            return _database.Table<Term>()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Deletes a term from the database
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public Task<int> DeleteTermAsync(Term term)
        {
            return _database.DeleteAsync(term);
        }

        /// <summary>
        /// Saves a term to the database
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public Task<int> SaveTermAsync(Term term)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        #endregion

        #region Course data manipulation

        /// <summary>
        /// Fetches all courses from the database
        /// </summary>
        /// <param name="termId"></param>
        /// <returns></returns>
        public Task<List<Course>> GetCoursesAsync(int termId)
        {
            return _database.Table<Course>().Where(course => course.TermId == termId).ToListAsync();
        }

        /// <summary>
        /// Saves or updates course to the database
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Task<int> SaveCourseAsync(Course course)
        {
            if (course.CourseId != 0)
            {
                return _database.UpdateAsync(course);
            }
            else
            {
                return _database.InsertAsync(course);
            }
        }

        /// <summary>
        /// Deletes a course from the database
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Task<int> DeleteCourseAsync(Course course)
        {
            return _database.DeleteAsync(course);

        }

        #endregion

        #region Assessment data manipulation

        public Task<List<Assessment>> GetAssessmentsByTermAsync(int courseId)
        {
            return _database.Table<Assessment>().Where(a => a.CourseId == courseId).ToListAsync();
        }

        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.AssessmentId != 0)
            {
                return _database.UpdateAsync(assessment);
            }
            else
            {
                return _database.InsertAsync(assessment);
            }
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }
        #endregion
    }
}
