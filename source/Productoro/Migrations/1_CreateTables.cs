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
                .WithColumn("Aggregate").AsString().NotNullable()
                .WithColumn("InstanceId").AsString().NotNullable()
                .WithColumn("Type").AsString().NotNullable()
                .WithColumn("Body").AsString()
                .WithColumn("Timestamp").AsDateTimeOffset().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Events");
        }

    }
}