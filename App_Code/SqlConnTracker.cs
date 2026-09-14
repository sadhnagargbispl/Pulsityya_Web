using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// Remembers every SqlConnection created during the current request and closes the ones
/// still open when the request ends (Global.asax -> Application_EndRequest).
/// Use SqlConnTracker.Create(cs) instead of new SqlConnection(cs) in pages.
/// </summary>
public static class SqlConnTracker
{
    private const string ItemsKey = "__SqlConnTracker";

    public static SqlConnection Create(string connectionString)
    {
        return Track(new SqlConnection(connectionString));
    }

    public static SqlConnection Track(SqlConnection connection)
    {
        HttpContext context = HttpContext.Current;
        if (connection == null || context == null)
            return connection;

        List<SqlConnection> list = context.Items[ItemsKey] as List<SqlConnection>;
        if (list == null)
        {
            list = new List<SqlConnection>();
            context.Items[ItemsKey] = list;
        }
        if (!list.Contains(connection))
            list.Add(connection);
        return connection;
    }

    public static void CloseAll()
    {
        HttpContext context = HttpContext.Current;
        if (context == null)
            return;

        List<SqlConnection> list = context.Items[ItemsKey] as List<SqlConnection>;
        if (list == null)
            return;

        foreach (SqlConnection connection in list)
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            catch
            {
                // ignore errors while closing
            }
        }
        list.Clear();
    }
}
