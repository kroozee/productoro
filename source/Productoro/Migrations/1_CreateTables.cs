using FluentMigrator;

namespace Productoro.Migrations
{
    [Migration(1)]
    public sealed class CreateTables : Migration
    {
        public override void Up()
        {
            Create
                .Table("Events")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().Identity()
                .WithColumn("AggregateType").AsString().NotNullable()
                .WithColumn("AggregateId").AsGuid().NotNullable()
                .WithColumn("Type").AsString().NotNullable()
                .WithColumn("Body").AsString()
                .WithColumn("Timestamp").AsDateTimeOffset().NotNullable();

            Create
                .Index("ix_AggregateType")
                .OnTable("Events")
                .OnColumn("AggregateType")
                .Descending()
                .WithOptions().NonClustered();

            Create
                .Index("ix_AggregateId")
                .OnTable("Events")
                .OnColumn("AggregateId")
                .Descending()
                .WithOptions().NonClustered();

            Create
                .Index("ix_Type")
                .OnTable("Events")
                .OnColumn("Type")
                .Descending()
                .WithOptions().NonClustered();

            Create
                .Index("ix_Timestamp")
                .OnTable("Events")
                .OnColumn("Timestamp")
                .Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Table("Events");
        }
    }
}