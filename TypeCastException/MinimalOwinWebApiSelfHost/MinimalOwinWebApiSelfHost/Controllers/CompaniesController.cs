using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using MinimalOwinWebApiSelfHost.Models;

namespace MinimalOwinWebApiSelfHost.Controllers
{
    public class CompaniesController : ApiController
    {
        ApplicationDbContext _db = new ApplicationDbContext();

       

        public IEnumerable<Company> Get()
        {
            return _db.Companies;
        }

        public async Task<Company> Get(int id)
        {
            var company = await  _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                throw new HttpResponseException(
                    System.Net.HttpStatusCode.NotFound);
            }
            return company;
        }

        public async Task<IHttpActionResult> Post(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }
            var companyExists = await  _db.Companies.AnyAsync(c => c.Id == company.Id);

            if (companyExists)
            {
                return BadRequest("Exists");
            }

            _db.Companies.Add(company);
            await _db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Put(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }
            var existing = await  _db.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);

            if (existing == null)
            {
                return NotFound();
            }
            existing.Name = company.Name;
            await _db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            _db.Companies.Remove(company);
            await _db.SaveChangesAsync();
            return Ok();
        }

    }
}