using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Models;

namespace queryExecutor.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using queryExecutor.CQRS.DscQueryParameter;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DscQParameter>("DscQParameters");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DscQParametersController : ODataController
    {
        private queryExecutorContext db = new queryExecutorContext();

        // GET: odata/DscQParameters
        [EnableQuery]
        public IQueryable<DscQParameter> GetDscQParameters()
        {
            return db.DscQParameters;
        }

        // GET: odata/DscQParameters(5)
        [EnableQuery]
        public SingleResult<DscQParameter> GetDscQParameter([FromODataUri] long key)
        {
            return SingleResult.Create(db.DscQParameters.Where(dscQParameter => dscQParameter.No == key));
        }

        // PUT: odata/DscQParameters(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Delta<DscQParameter> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DscQParameter dscQParameter = await db.DscQParameters.FindAsync(key);
            if (dscQParameter == null)
            {
                return NotFound();
            }

            patch.Put(dscQParameter);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DscQParameterExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dscQParameter);
        }

        // POST: odata/DscQParameters
        public async Task<IHttpActionResult> Post(DscQParameter dscQParameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DscQParameters.Add(dscQParameter);
            await db.SaveChangesAsync();

            return Created(dscQParameter);
        }

        // PATCH: odata/DscQParameters(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<DscQParameter> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DscQParameter dscQParameter = await db.DscQParameters.FindAsync(key);
            if (dscQParameter == null)
            {
                return NotFound();
            }

            patch.Patch(dscQParameter);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DscQParameterExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dscQParameter);
        }

        // DELETE: odata/DscQParameters(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            DscQParameter dscQParameter = await db.DscQParameters.FindAsync(key);
            if (dscQParameter == null)
            {
                return NotFound();
            }

            db.DscQParameters.Remove(dscQParameter);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DscQParameterExists(long key)
        {
            return db.DscQParameters.Count(e => e.No == key) > 0;
        }
    }
}
