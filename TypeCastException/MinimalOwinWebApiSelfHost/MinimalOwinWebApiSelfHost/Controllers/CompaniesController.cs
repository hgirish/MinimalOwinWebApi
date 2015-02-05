using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using MinimalOwinWebApiSelfHost.Models;

namespace MinimalOwinWebApiSelfHost.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : ApiController
    {
        ApplicationDbContext _db = new ApplicationDbContext();

       

        public IEnumerable<Company> Get()
        {
           // Console.WriteLine("Call to companies get");
           
            var companies = _db.Companies;
            Console.WriteLine("number of companies :{0}", companies.Count());
            return companies;
        }

        public async Task<Company> Get(int id)
        {
            Console.WriteLine("Get id{0}:", id);
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                Console.WriteLine("Get company null");
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
                Console.WriteLine("Company null put");
                return BadRequest("Argument null");
            }
            Console.WriteLine("Put company id: {0}", company.Id);
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