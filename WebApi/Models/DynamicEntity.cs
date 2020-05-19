using System.Data.Entity;
namespace WebApi.Models
{
    public partial class DynamicEntity:DbContext
    {
        public DynamicEntity(string connectionString)
            : base(connectionString)
        {
        }
    }
}