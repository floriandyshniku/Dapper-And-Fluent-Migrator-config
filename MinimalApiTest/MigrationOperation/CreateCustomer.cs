using FluentMigrator;

namespace MinimalApiTest.MigrationOperation
{
    [Migration(1)]
    public class CreateCustomer : Migration
    {
        public override void Down()
        {
            Delete.Table("Customer");
        }

        public override void Up()
        {
            Create.Table("Customer").
                WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity().Indexed()
                .WithColumn("Name").AsString(50);
        }
    }
}
