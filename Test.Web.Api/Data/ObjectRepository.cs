using System.Data;
using System.Data.SqlClient;
using Dapper;
using Test.Web.Api.Infrastructure;
using Test.Web.Api.Infrastructure.Models;
using Test.Web.Api.Models;

namespace Test.Web.Api.Data;

public interface IObjectRepository
{
    Task<int> AddObjects(List<ObjectInsertModel> items);
    Task<PagedListModel<ObjectItemModel>> GetObjects(ObjectFilterModel filter);
}

public class ObjectRepository : IObjectRepository
{
    private readonly string _connectionString;

    public ObjectRepository(MicrosoftSqlConnectionString connectionString)
    {
        _connectionString = connectionString.Value;
    }

    public async Task<int> AddObjects(List<ObjectInsertModel> items)
    {
        using var connection = createConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        const string truncateTableSql = "TRUNCATE TABLE OBJECTS";
        await connection.ExecuteAsync(truncateTableSql, null, transaction);

        var batchSize = 500;
        var currentBatch = 0;
        var insertedCount = 0;
        while (currentBatch * batchSize < items.Count)
        {
            var nextBatch = items.Skip(currentBatch * batchSize).Take(batchSize).ToList();
            
            const string insertSql = @"
            INSERT INTO OBJECTS
                (
                    code,
                    value
                 )
            VALUES 
                (
                    @Code,
                    @Value
                )";

            insertedCount = await connection.ExecuteAsync(insertSql, nextBatch, transaction);
            currentBatch++;
        }

        if (insertedCount > 0)
        {
            transaction.Commit();
        }
        else
        {
            transaction.Rollback();
        }

        return insertedCount;
    }

    public async Task<PagedListModel<ObjectItemModel>> GetObjects(ObjectFilterModel filter)
    {
        using var connection = createConnection();

        const string countSql = @$"
            SELECT 
                COUNT(id)
            FROM OBJECTS o";

        var count = await connection.QueryFirstOrDefaultAsync<int>(countSql);

        var sql = @$"
            SELECT
                o.id as {nameof(ObjectItemModel.Id)},
                o.code as {nameof(ObjectItemModel.Code)},
                o.value as {nameof(ObjectItemModel.Value)}
            FROM OBJECTS o
            {(string.IsNullOrWhiteSpace(filter.Keyword) ? "" : "WHERE (LOWER(o.code) LIKE @Keyword OR LOWER(o.value) LIKE @Keyword)")}
            ORDER BY o.id ASC
            OFFSET @Skip ROWS
            FETCH FIRST @Take ROWS ONLY";

        var results = (await connection.QueryAsync<ObjectItemModel>(sql, new
        {
            Keyword = string.IsNullOrWhiteSpace(filter.Keyword) ? "" : $"%{filter.Keyword.ToLower()}%",
            filter.Skip,
            filter.Take
        })).ToList();

        return new PagedListModel<ObjectItemModel>(items: results, itemsCount: count);
    }

    private IDbConnection createConnection()
    {
        return new SqlConnection(_connectionString);
    }
}