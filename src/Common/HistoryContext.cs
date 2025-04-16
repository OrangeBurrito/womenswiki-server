using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

namespace WomensWiki.src.Common;

public class HistoryContext : NpgsqlHistoryRepository {
    public HistoryContext(HistoryRepositoryDependencies dependencies) : base(dependencies) {}

    protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history) {
        base.ConfigureTable(history);
        
        history.Property(h => h.MigrationId).HasColumnName("MigrationId");
        history.Property(h => h.ProductVersion).HasColumnName("ProductVersion");
    }
}