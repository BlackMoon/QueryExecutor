using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using queryExecutor.Models;

namespace queryExecutor.Controllers
{
    [EnableQuery]
    [Route("odata/Queries")]
    public class QueriesController : Controller
    {
        private readonly QueryService _queryService;

        public QueriesController(QueryService queryService)
        {
            _queryService = queryService;
        }

        // GET: api/Queries
        [HttpGet]
        public IEnumerable<DscQuery> Get()
        {
            return _queryService.Queries;
        }
    }
}
