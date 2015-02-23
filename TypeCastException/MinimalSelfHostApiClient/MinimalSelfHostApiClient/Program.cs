using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MinimalSelfHostApiClient
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Task running");
            Run().Wait();
            /*
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
            */

            Console.WriteLine("Done! Press the Enter key to Exit...");
            Console.Read();



        }

        static async Task Run()
        {
            string hostUriString = "http://localhost:8080";
            var provider = new ApiClientProvider(hostUriString);

            string accessToken;
            Dictionary<string, string> tokenDictionary;

            try
            {
                tokenDictionary = await provider.GetTokenDictionary(
                    "john@example.com", "JohnsPassword");
                accessToken = tokenDictionary["access_token"];

                foreach (var kvp in tokenDictionary)
                {
                    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                    Console.WriteLine("");
                }
                var baseUri = new Uri(hostUriString);
                var companyClient = new CompanyClient(baseUri, accessToken);

                Console.WriteLine("Read all the companies...");
                var reslut = await companyClient.GetCompaniesAsync();
                var companies = reslut as Company[] ?? reslut.ToArray();
                WriteCompanyList(companies);

                int nextid = (from c in companies select c.Id).Max() + 1;
                Console.WriteLine("Add a new company...");
                var result = await companyClient.AddCompanyAsync(
                    new Company
                    {
                        Name = string.Format("New Company #{0}", nextid)
                    });
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated list after Add:");
                var list = await companyClient.GetCompaniesAsync();
                companies = list.ToArray();
                WriteCompanyList(companies);

                Console.WriteLine("Update a company...");
                var updateMe = await companyClient.GetCompanyAsync(nextid-1);
                Console.WriteLine("UpdateMe.id{0}",updateMe.Id);
                updateMe.Name = string.Format("Updated company #{0}", updateMe.Id);
                result = await companyClient.UpdateCompanyAsync(updateMe);
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated list after Update:");
                 list = await companyClient.GetCompaniesAsync();
                companies = list.ToArray();
                WriteCompanyList(companies);

                Console.WriteLine("Delete a comany...");
                result = await companyClient.DeleteCompanyAsync(nextid = 1);
                WriteStatusCodeResult(result);


                Console.WriteLine("Updated list after Delete:");
                list = await companyClient.GetCompaniesAsync();
                companies = list.ToArray();
                WriteCompanyList(companies);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine("Press the Enter key to exit...");
                Console.ReadLine();
                return;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press the enter key to exit...");
                Console.ReadLine();
                return;
            }
            
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
