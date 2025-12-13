using System.Linq.Expressions;
using System.Reflection;
using HttpServer.Framework.Settings;
using Npgsql;

namespace HttpServer;

public class LinqExpressionToSql
{
    private readonly string _connectionString = SettingsManager.Instance.Settings.ConnectionString!;

    public T? FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : new()
    {
        return Where(predicate).FirstOrDefault();
    }

    public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : new()
    {
        var sql = BuildSqlQuery(predicate.Body);
        return ExecuteQuery<T>($"SELECT * FROM {GetTableName<T>()} WHERE {sql}");
    }

    private string BuildSqlQuery(Expression expression)
    {
        return expression switch
        {
            BinaryExpression b => $"({BuildSqlQuery(b.Left)} {GetSqlOperator(b.NodeType)} {BuildSqlQuery(b.Right)})",
            MemberExpression m => m.Member.Name.ToLower(),
            ConstantExpression c => FormatConstant(c.Value),
            UnaryExpression u when u.NodeType == ExpressionType.Convert => BuildSqlQuery(u.Operand),
            MethodCallExpression mc => HandleMethodCall(mc),
            _ => throw new NotSupportedException($"Unsupported expression: {expression.GetType().Name}")
        };
    }

    private string HandleMethodCall(MethodCallExpression mc)
    {
        var obj = BuildSqlQuery(mc.Object!);
        var arg = FormatConstant(GetValue(mc.Arguments[0]));

        return mc.Method.Name switch
        {
            "Contains" => $"{obj} LIKE '%' || {arg} || '%'",
            "StartsWith" => $"{obj} LIKE {arg} || '%'",
            "EndsWith" => $"{obj} LIKE '%' || {arg}",
            _ => throw new NotSupportedException($"Method {mc.Method.Name} not supported")
        };
    }

    private static string GetSqlOperator(ExpressionType type)
    {
        return type switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.GreaterThan => ">",
            ExpressionType.LessThan => "<",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.AndAlso => "AND",
            ExpressionType.OrElse => "OR",
            _ => throw new NotSupportedException($"Operator {type} not supported")
        };
    }

    private static string FormatConstant(object? value)
    {
        return value switch
        {
            null => "NULL",
            string s => $"'{s.Replace("'", "''")}'",
            bool b => b ? "TRUE" : "FALSE",
            _ => value.ToString()!
        };
    }

    private static object? GetValue(Expression expr)
    {
        return Expression.Lambda(expr).Compile().DynamicInvoke();
    }

    private static string GetTableName<T>()
    {
        return typeof(T).Name.ToLower().Replace("model", "") + "s";
    }

    private IEnumerable<T> ExecuteQuery<T>(string sql) where T : new()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        using var cmd = new NpgsqlCommand(sql, conn);

        conn.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            yield return Map<T>(reader);
    }

    private static T Map<T>(NpgsqlDataReader reader) where T : new()
    {
        var obj = new T();

        for (var i = 0; i < reader.FieldCount; i++)
        {
            var prop = typeof(T).GetProperty(reader.GetName(i),
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);

            if (prop?.CanWrite == true && !reader.IsDBNull(i))
                prop.SetValue(obj, reader.GetValue(i));
        }

        return obj;
    }
}