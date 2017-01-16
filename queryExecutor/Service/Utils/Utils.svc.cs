using System;
using System.Linq;
using System.ServiceModel;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQColumn.Query;

namespace queryExecutor.Service.Utils
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Utils" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Utils.svc or Utils.svc.cs at the Solution Explorer and start debugging.
    public class Utils : IUtils
    {
        private readonly IQueryDispatcher _queryDispatcher;
        public Utils(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public IQueryable<DscQColumn> GetColumns()
        {
            

            DscQColumnQuery columnQuery = new DscQColumnQuery()
            {
                Path = "Аварийные скважины/Скважины на которых были аварии",
                DataSource = "aql.eco",
                UserId = "ECO",
                Password = "ECO"
            };

            DscQColumnQueryResult result = _queryDispatcher.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(columnQuery);
            return result.Items;
        }

        public void GetParameters()
        {
            throw new NotImplementedException();
        }

        public void GetResults()
        {
            throw new NotImplementedException();
        }
    }
}
