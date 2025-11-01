﻿using Npgsql;

namespace MyORMLibrary.Tests;

[TestClass]
public class OrmContextCrudTests
{
    private const string Table = "users_test";

    private static readonly string _conn =
        "Host=localhost;Port=5432;Database=music;Username=developer;Password=developer;";

    [TestInitialize]
    public void Init()
    {
        using var con = new NpgsqlConnection(_conn);
        con.Open();
        var ddl = $@"
CREATE TABLE IF NOT EXISTS {Table}(
  id SERIAL PRIMARY KEY,
  name TEXT NOT NULL,
  email TEXT NOT NULL
);
TRUNCATE {Table};";
        using var cmd = new NpgsqlCommand(ddl, con);
        cmd.ExecuteNonQuery();
    }

    [TestMethod]
    public void Crud_Works()
    {
        var orm = new ORMContext(_conn);
        var created = orm.Create(new TestUser { Name = "Alice", Email = "a@b.com" }, Table);
        Assert.IsTrue(created.Id > 0);
        var byId = orm.ReadById<TestUser>(created.Id, Table);
        Assert.IsNotNull(byId);
        Assert.AreEqual("a@b.com", byId.Email);
        orm.Update(created.Id, new TestUser { Id = created.Id, Name = "Alice2", Email = "a2@b.com" }, Table);
        var afterUpd = orm.ReadById<TestUser>(created.Id, Table);
        Assert.AreEqual("Alice2", afterUpd.Name);
        orm.Delete(created.Id, Table);
        var afterDel = orm.ReadById<TestUser>(created.Id, Table);
        Assert.IsNull(afterDel);
    }
}

public class TestUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}