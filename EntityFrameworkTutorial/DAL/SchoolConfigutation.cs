using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;

namespace EntityFrameworkTutorial.DAL
{
   public class SchoolConfigutation: DbConfiguration
   {
      SchoolConfigutation() {
         SetExecutionStrategy("System.DataClient.SqlClient", () => new SqlAzureExecutionStrategy());
         // DbInterception.Add(new SchoolInterceptorTrasientErrors());
         // DbInterception.Add(new SchoolInterceptorLogging());
      }
   }
}