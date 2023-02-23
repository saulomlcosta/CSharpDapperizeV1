using System.Data;
using BaltaAccessV1.Models;
using Dapper;
using Microsoft.Data.SqlClient;

const string connectionString = "Server=localhost,1450; Database=balta; User ID=SA;Password= Numsey#2022; TrustServerCertificate=True;";

// Using ADO.NET 
// using (var connection = new SqlConnection(connectionString))
// {
//     Console.WriteLine("Conectado!");
//     connection.Open();

//     using (var command = new SqlCommand())
//     {
//         command.Connection = connection;
//         command.CommandType = System.Data.CommandType.Text;
//         command.CommandText = "SELECT [ID], [Title] FROM [Category]";

//         var reader = command.ExecuteReader();
//         while (reader.Read())
//         {
//             Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
//         }
//     }
// }

// Using Dapper

using (var connection = new SqlConnection(connectionString))
{
    // CreateCategory(connection);
    // UpdateCategory(connection);
    // DeleteCategory(connection);
    // CreateManyCategories(connection);
    // GetCategory(connection);
    // ExecuteProcedure(connection);
    // ReadProcedure(connection);
    // ExecuteScalar(connection);
    // ListCategories(connection);
}

static void ListCategories(SqlConnection connection)
{
    var categories = connection.Query<Category>("SELECT [ID], [Title] FROM [Category]");
    foreach (var item in categories)
    {
        Console.WriteLine($"{item.Id} - {item.Title}");
    }
}

static void GetCategory(SqlConnection connection)
{
    var category = connection.QueryFirstOrDefault<Category>("SELECT [Id], [Title] FROM [Category] WHERE [Id] = @id", new { id = new Guid("25d510c8-3108-44c2-86c5-924d9832aa8c") });
    Console.WriteLine($"{category.Id} - {category.Title}");
}

static void CreateCategory(SqlConnection connection)
{
    var category = new Category();
    category.Id = Guid.NewGuid();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços da Amazon";
    category.Order = 8;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var insertSql = $@"INSERT INTO [Category] VALUES (@Id, @Title, @Url, @Description, @Order, @Summary, @Featured)";

    var rows = connection.Execute(insertSql, new
    {
        Id = category.Id,
        Title = category.Title,
        Url = category.Url,
        Description = category.Description,
        Order = category.Order,
        Summary = category.Summary,
        Featured = category.Featured
    });

    Console.WriteLine($"{rows} linhas inseridas!");
}

static void CreateManyCategories(SqlConnection connection)
{
    var category = new Category();
    category.Id = Guid.NewGuid();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços da Amazon";
    category.Order = 8;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var category2 = new Category();
    category2.Id = Guid.NewGuid();
    category2.Title = "Docker";
    category2.Url = "docker";
    category2.Description = "Categoria destinada a serviços com Docker";
    category2.Order = 9;
    category2.Summary = "Docker";
    category2.Featured = false;

    var insertSql = $@"INSERT INTO [Category] VALUES (@Id, @Title, @Url, @Description, @Order, @Summary, @Featured)";

    var rows = connection.Execute(insertSql, new[]{
        new
        {
          Title = category.Title,
          Id = category.Id,
          Url = category.Url,
          Description = category.Description,
          Order = category.Order,
          Summary = category.Summary,
          Featured = category.Featured
        },
         new
        {
          Title = category2.Title,
          Id = category2.Id,
          Url = category2.Url,
          Description = category2.Description,
          Order = category2.Order,
          Summary = category2.Summary,
          Featured = category2.Featured
        }
    });

    Console.WriteLine($"{rows} linhas inseridas!");
}

static void UpdateCategory(SqlConnection connection)
{
    var updateQuery = "UPDATE [Category] SET [Title] = @title WHERE [Id] = @id";
    var rows = connection.Execute(updateQuery, new
    {
        id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
        title = "Frontend"
    });

    Console.WriteLine($"{rows} registros atualizados!");
}

static void DeleteCategory(SqlConnection connection)
{
    var deleteQuery = "DELETE FROM [Category] WHERE [Id] = @id";
    var rows = connection.Execute(deleteQuery, new
    {
        id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4")
    });

    Console.WriteLine($"{rows} linhas removidas!");
}

//Procedures
static void ExecuteProcedure(SqlConnection connection)
{
    var procedure = "[spDeleteStudent]";
    var param = new { StudentId = "ed624047-4884-43da-b332-51d0b106dc30" };
    var affectedRows = connection.Execute(
        procedure,
        param,
        commandType: CommandType.StoredProcedure);

    Console.WriteLine($"{affectedRows} linhas afetadas!");
}

static void ReadProcedure(SqlConnection connection)
{
    var procedure = "[spGetCoursesByCategory]";
    var param = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
    var courses = connection.Query<Category>(
        procedure,
        param,
        commandType: CommandType.StoredProcedure);

    foreach (var item in courses)
    {
        Console.WriteLine($"{item.Id} - {item.Title}");
    }
}

//ExecuteScalar
static void ExecuteScalar(SqlConnection connection)
{
    var category = new Category();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços da Amazon";
    category.Order = 10;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var insertSql = $@"
        INSERT INTO 
            [Category] 
        OUTPUT inserted.[Id]
        VALUES (
            NEWID(), 
            @Title, 
            @Url, 
            @Description, 
            @Order, 
            @Summary, 
            @Featured)";

    // If we use int as ID, Return a ID after created with SCOPE_IDENTITY
    // var insertSql = $@"
    //     INSERT INTO 
    //         [Category] 
    //     VALUES (
    //         NEWID(), 
    //         @Title, 
    //         @Url, 
    //         @Description, 
    //         @Order, 
    //         @Summary, 
    //         @Featured)
    //     SELECT SCOPE_IDENTITY()";

    var categoryId = connection.ExecuteScalar<Guid>(insertSql, new
    {
        Title = category.Title,
        Url = category.Url,
        Description = category.Description,
        Order = category.Order,
        Summary = category.Summary,
        Featured = category.Featured
    });

    Console.WriteLine($"Categoria {categoryId} inserida com sucesso!");
}

Console.ReadLine();