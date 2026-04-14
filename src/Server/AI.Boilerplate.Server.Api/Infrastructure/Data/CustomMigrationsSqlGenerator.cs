using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations;

namespace AI.Boilerplate.Server.Api.Infrastructure.Data;

public class CustomMigrationsSqlGenerator : NpgsqlMigrationsSqlGenerator
{
    public CustomMigrationsSqlGenerator(
        MigrationsSqlGeneratorDependencies dependencies,
        INpgsqlSingletonOptions npgsqlSingletonOptions)
        : base(dependencies, npgsqlSingletonOptions)
    {
    }

    protected override void Generate(
        CreateTableOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder,
        bool terminate = true)
    {
        // 移除外键约束
        operation.ForeignKeys.Clear();

        base.Generate(operation, model, builder, terminate);
    }

    // 如果需要拦截添加外键的操作
    protected override void Generate(
        AddForeignKeyOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder,
        bool terminate = true)
    {
        // 跳过外键添加
        return;
    }
}
