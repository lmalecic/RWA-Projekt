using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class LogService
    {
        private readonly BookLibraryContext _context;

        public LogService(BookLibraryContext context)
        {
            this._context = context;
        }

        public BookLog Log(string message, int level)
        {
            BookLog log = new(message, level);

            this._context.BookLogs.Add(log);
            this._context.SaveChanges();

            return log;
        }

        public IEnumerable<BookLog> GetLogs()
        {
            return this._context.BookLogs.ToList().Order();
        }

        public int Count()
        {
            return this._context.BookLogs.Count();
        }
    }
}
