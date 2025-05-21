using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public partial class BookLog
    {
        public BookLog()
        {
            this.Timestamp = DateTime.UtcNow;
        }

        public BookLog(string message, int level)
        {
            this.Message = message;
            this.Level = level;
            this.Timestamp = DateTime.UtcNow;
        }

        public int CompareTo(BookLog? other)
        {
            return other != null ? -Timestamp.CompareTo(other.Timestamp) : 0;
        }
    }

    public static partial class Extensions
    {
        public static IOrderedEnumerable<BookLog> Order(this IEnumerable<BookLog> logs)
        {
            return logs.OrderByDescending(x => x.Timestamp);
        }
        public static IOrderedEnumerable<BookLog> Order(this IQueryable<BookLog> logs)
        {
            return (IOrderedEnumerable<BookLog>)logs.OrderByDescending(x => x.Timestamp);
        }
    }
}
