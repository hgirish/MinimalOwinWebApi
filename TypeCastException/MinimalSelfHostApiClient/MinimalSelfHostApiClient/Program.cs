using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MinimalSelfHostApiClient
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Read all the companies...");
            var companyClient = new CompanyClient("http://localhost:8080");

            var companies = companyClient.GetCompanies();

            WriteCompanyList(companies);

            int nextId = (from c in companies select c.Id).Max() + 1;
            Console.WriteLine("Add a new company...");
            var result = companyClient.AddCompany(
                new Company
                {
                   // Id = nextId,
                    Name = string.Format("New Company #{0}", nextId)
                });
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after add: ");
             companies = companyClient.GetCompanies();
            WriteCompanyList(companies);

            Console.WriteLine("Update a compnay");
            var updateMe = companyClient.GetCompany(nextId);
            updateMe.Name = string.Format("Update company #{0}", updateMe.Id);
            result = companyClient.UpdateCompany(updateMe);
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after update: ");
            companies = companyClient.GetCompanies();
            WriteCompanyList(companies);

            Console.WriteLine("Delete a company...");
            result = companyClient.DeleteCompany(nextId - 1);
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after delete: ");
            companies = companyClient.GetCompanies();
            WriteCompanyList(companies);

            Console.Read();



        }

        private static void WriteCompanyList(IEnumerable<Company> companies)
        {
            foreach (var company in companies)
            {
                Console.WriteLine("Id: {0} Name: {1}", company.Id, company.Name);
            }
            Console.WriteLine("");
        }

        static void WriteStatusCodeResult(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("Operation Succedded -  status code {0}");
            }
            else
            {
                Console.WriteLine("Operation failed - status code {0}", statusCode);
            }
            Console.WriteLine("");
        }
    }
}
