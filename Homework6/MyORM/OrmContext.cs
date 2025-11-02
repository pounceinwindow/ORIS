using System.Text;
using Npgsql;

namespace MyORMLibrary;

public class ORMContext
{
    private readonly string _connectionString;

    public ORMContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public T Create<T>(T entity, string tableName) where T : class, new()
    {
        using var dataSource = NpgsqlDataSource.Create(_connectionString);
        var props = typeof(T).GetProperties().ToList();
        var cols = props.Skip(1).Select(p => p.Name.ToLower());
        var sb = new StringBuilder();

        sb.Append($"INSERT INTO {tableName.ToLower()} (");
        sb.Append(string.Join(",", cols));
        sb.Append(") VALUES (");
        sb.Append(string.Join(",", cols.Select(c => "@" + c)));
        sb.Append(") RETURNING *;");

        var cmd = dataSource.CreateCommand(sb.ToString());

        foreach (var p in props.Skip(1))
            cmd.Parameters.AddWithValue(p.Name.ToLower(), p.GetValue(entity) ?? DBNull.Value);

        using var r = cmd.ExecuteReader();
        if (r.Read()) return MapRecord<T>(r);
        return entity;
    }

    public T ReadById<T>(int id, string tableName) where T : class, new()
    {
        using var dataSource = NpgsqlDataSource.Create(_connectionString);

        var sql = $"SELECT * FROM {tableName.ToLower()} WHERE id = @id LIMIT 1";
        var cmd = dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", id);

        using var r = cmd.ExecuteReader();
        if (r.Read()) return MapRecord<T>(r);
        return null;
    }

    public void Update<T>(int id, T entity, string tableName)
    {
        using var dataSource = NpgsqlDataSource.Create(_connectionString);

        var props = typeof(T).GetProperties().ToList();
        var sets = string.Join(",", props.Skip(1).Select(p => $"{p.Name.ToLower()}=@{p.Name.ToLower()}"));
        var sql = $"UPDATE {tableName.ToLower()} SET {sets} WHERE id=@id";
        var cmd = dataSource.CreateCommand(sql);

        foreach (var p in props.Skip(1))
            cmd.Parameters.AddWithValue(p.Name.ToLower(), p.GetValue(entity) ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id, string tableName)
    {
        using var dataSource = NpgsqlDataSource.Create(_connectionString);
        var sql = $"DELETE FROM {tableName.ToLower()} WHERE id = @id";
        var cmd = dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    private static T MapRecord<T>(NpgsqlDataReader r) where T : class, new()
    {
        var obj = new T();
        var props = typeof(T).GetProperties().ToList();
        var cols = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < r.FieldCount; i++) cols[r.GetName(i)] = i;

        foreach (var p in props)
        {
            var key = p.Name;
            if (!cols.TryGetValue(key, out var idx) && !cols.TryGetValue(key.ToLower(), out idx)) continue;
            var val = r.IsDBNull(idx) ? null : r.GetValue(idx);
            var t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
            p.SetValue(obj, val == null ? null : Convert.ChangeType(val, t));
        }

        return obj;
    }

    public List<T> ReadAll<T>(string tableName) where T : class, new()
    {
        using var dataSource = NpgsqlDataSource.Create(_connectionString);
        var cmd = dataSource.CreateCommand($"SELECT * FROM {tableName.ToLower()}");
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(MapRecord<T>(r));
        return list;
    }
}

