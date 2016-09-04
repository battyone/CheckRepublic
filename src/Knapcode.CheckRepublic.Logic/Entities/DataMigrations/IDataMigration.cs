using System;

namespace Knapcode.CheckRepublic.Logic.Entities.DataMigrations
{
    public interface IDataMigration
    {
        void Up(CheckContext context);
    }
}