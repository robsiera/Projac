using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represent a SQL non query statement.
    /// </summary>
    public class SqlNonQueryStatement : SqlNonQueryCommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlNonQueryStatement" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown when <paramref name="text" /> or <paramref name="parameters" />
        ///     is <c>null</c>.
        /// </exception>
        public SqlNonQueryStatement(string text, DbParameter[] parameters) : base(text, parameters, CommandType.Text)
        {
        }
    }
}