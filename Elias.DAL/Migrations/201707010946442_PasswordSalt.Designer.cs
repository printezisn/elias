// <auto-generated />
namespace Elias.DAL.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class PasswordSalt : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(PasswordSalt));
        
        string IMigrationMetadata.Id
        {
            get { return "201707010946442_PasswordSalt"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
