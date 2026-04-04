using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRagChunkHnswIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE INDEX IF NOT EXISTS "IX_RagChunks_Embedding_HNSW"
                ON "RagChunks"
                USING hnsw ("Embedding" vector_cosine_ops)
                WHERE "Embedding" IS NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "IX_RagChunks_Embedding_HNSW";""");
        }
    }
}
