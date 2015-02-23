using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using MinimalOwinWebApiSelfHost.Models;

namespace MinimalOwinWebApiSelfHost.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : ApiController
    {
        ApplicationDbContext Db
        {
            get { return Request.GetOwinContext().Get<ApplicationDbContext>(); }
        }

       

        public IEnumerable<Company> Get()
        {
           // Console.WriteLine("Call to companies get");
           
            var companies = Db.Companies;
            Console.WriteLine("number of companies :{0}", companies.Count());
            return companies;
        }

        public async Task<Company> Get(int id)
        {
            Console.WriteLine("Get id{0}:", id);
            var company = await Db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                Console.WriteLine("Get company null");
                throw new HttpResponseException(
                    HttpStatusCode.NotFound);
            }
            return company;
        }

        public async Task<IHttpActionResult> Post(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }
            var companyExists = await  Db.Companies.AnyAsync(c => c.Id == company.Id);

            if (companyExists)
            {
                return BadRequest("Exists");
            }

            Db.Companies.Add(company);
            await Db.SaveChangesAsync();
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
            var existing = await  Db.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);

            if (existing == null)
            {
                return NotFound();
            }
            existing.Name = company.Name;
            await Db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var company = await Db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            Db.Companies.Remove(company);
            await Db.SaveChangesAsync();
            return Ok();
        }

    }
}