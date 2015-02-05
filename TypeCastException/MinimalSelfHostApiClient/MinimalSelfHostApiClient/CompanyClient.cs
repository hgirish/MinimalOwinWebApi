using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MinimalSelfHostApiClient
{
    public class CompanyClient
    {
        private readonly Uri _baseRequestUri;
        private string _accessToken;
        public CompanyClient(Uri baseRequestUri, string accessToken)
        {
            _baseRequestUri = new Uri(baseRequestUri,"api/companies/");
            _accessToken = accessToken;
        }

        void SetClientAuthentication(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
       

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await   client.GetAsync(_baseRequestUri);
            }
            Console.WriteLine(response.IsSuccessStatusCode);
            Console.WriteLine(response.ReasonPhrase);
            var result = await response.Content.ReadAsAsync<IEnumerable<Company>>();
            return result;
        }

        public async Task<Company> GetCompanyAsync(int id)
        {
            Console.WriteLine("GetCompanyAsync id: {0}", id);
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await  client.GetAsync(
                    new Uri(_baseRequestUri,id.ToString(
                        CultureInfo.InvariantCulture)));
            }
            var result = await response.Content.ReadAsAsync<Company>();
            return result;
        }

        public async Task<HttpStatusCode> AddCompanyAsync(Company company)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await  client.PostAsJsonAsync(
                    _baseRequestUri, company);
            }
            return response.StatusCode;
        }

        public async  Task<HttpStatusCode> UpdateCompanyAsync(Company company)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.PutAsJsonAsync(_baseRequestUri, 
                    company);
            }
            return response.StatusCode;
        }

        public async  Task<HttpStatusCode> DeleteCompanyAsync(int id)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.DeleteAsync(
                    new Uri(_baseRequestUri,id.ToString()));

            }
            return response.StatusCode;
        }
    }
}