using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace GSTInvoiceHelper
{
    class Program
    {
        static IConfiguration Configuration;
            static void Main(string[] args)
            {
            //string[] args = { "/action=geteinvoicetoken", "/vendortoken=43e6e2c8-6812-4c21-9945-a265583a1648" };

                Configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .AddCommandLine(args)
              .Build();

            string action = GetParamValue(args, "action", false);
                string vendortoken = GetParamValue(args, "vendortoken", false);
                string einvoicetoken = GetParamValue(args, "einvoicetoken", false);

                if (string.IsNullOrEmpty(action))
                    action = "getbothtokens";

                Token token = null;
                Token einvoiceToken = null;

                token = new Token() { access_token = vendortoken };
                einvoiceToken = new Token() { access_token = einvoicetoken };

                if (action.ToLower().Contains("getgstvendortoken") || action.ToLower().Contains("getbothtokens"))
                {
                    token = getToken();
                    Console.WriteLine("Token:" + token.ToString());
                }

                if (action.ToLower().Contains("geteinvoicetoken") || action.ToLower().Contains("getbothtokens"))
                {
                    einvoiceToken = Authenticate(token);
                    Console.WriteLine("Einvoice Token:" + einvoiceToken.access_token.ToString()); 
                }
            

            }



            private static Token getToken()
            {
            Console.WriteLine("Getting Authentication Token from GST Vendor");
                using (var client = new HttpClient())
                {
                    var urlPparams = new List<KeyValuePair<string, string>>();
                    urlPparams.Add(new KeyValuePair<string, string>("grant_type", Configuration["GstVendorAuthGrantType"]));// password"));
                    urlPparams.Add(new KeyValuePair<string, string>("username", Configuration["GstVendorAuthUsername"]));// "erp1@perennialsys.com"));
                    urlPparams.Add(new KeyValuePair<string, string>("password", Configuration["GstVendorAuthPassword"]));// "einv12345"));
                    urlPparams.Add(new KeyValuePair<string, string>("client_id", Configuration["GstVendorAuthClientId"]));// "testerpclient"));

                    string url = Configuration["GstVendorAuthenticateUrl"];// 
                    var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(urlPparams), };

                    req.Headers.Add("Authorization", Configuration["BasicAuthHeader"]);//Basic dGVzdGVycGNsaWVudDpBZG1pbkAxMjM=");
                    req.Headers.Add("gstin", Configuration["GstVendorAuthClientId"]); // "29AFQPB8708K000");
                    //req.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    var responseTask = client.SendAsync(req);
                    responseTask.Wait();
                    var result = responseTask.Result;


                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        var strResult = readTask.Result;

                        var token = JsonConvert.DeserializeObject<Token>(strResult);
                        Console.WriteLine(strResult);

                        return token;

                    }
                    else
                    {
                        throw new Exception("not working");
                    }
                }
            }

            private static Token Authenticate(Token token)
            {
            Console.WriteLine("Second stage Authentication Token from E-invoice API");
            using (var client = new HttpClient())
                {

                    var authRequest = new AuthRequest()
                    {
                        action = Configuration["EinvoiceAuthAction"],// "ACCESSTOKEN",                    
                        username = Configuration["EinvoiceAuthUsername"], //"perennialsystems"
                        password = Configuration["EinvoiceAuthPassword"] //"p3r3nn!@1",
                    };

                    string einvoicAuthUrl = Configuration["EinvoiceAuthenticateUrl"];
                    var req = new HttpRequestMessage(HttpMethod.Post, einvoicAuthUrl);// "https://35.154.208.8/einvoice/v1.03/authentication");

                    req.Content = new StringContent(JsonConvert.SerializeObject(authRequest), Encoding.UTF8, "application/json");
                
                //req.Headers.Add("Content-Type", "application/json");
                //Console.WriteLine("Header: Content-Type: application/json");
                req.Headers.Add("Accept", "application/json");
                Console.WriteLine("Header: Accept: application/json");
                req.Headers.Add("Authorization", "Bearer " + token.access_token);
                Console.WriteLine("Header: Authorization: " + "Bearer " + token.access_token);
                    req.Headers.Add("gstin", Configuration["GSTIN"]); // "29AFQPB8708K000");
                Console.WriteLine("Header: gstin: " + Configuration["GSTIN"]);
                req.Headers.Add("action", Configuration["EinvoiceAuthAction"]);// ACCESSTOKEN");
                Console.WriteLine("Header: action: " + Configuration["EinvoiceAuthAction"]);
                req.Headers.Add("X-Connector-Auth-Token", Configuration["EinvoiceXConnectorToken"]);// ACCESSTOKEN");
                Console.WriteLine("Header: X-Connector-Auth-Token: " + Configuration["EinvoiceXConnectorToken"]);

                Console.WriteLine("Body:" + JsonConvert.SerializeObject(authRequest));

                var responseTask = client.SendAsync(req);
                    responseTask.Wait();
                    var result = responseTask.Result;


                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        var strResult = readTask.Result;

                        var authToken = JsonConvert.DeserializeObject<Token>(strResult);
                        Console.WriteLine(strResult);

                        return authToken;

                    }
                    else
                    {
                        Console.WriteLine("Result:"+result);
                        throw new Exception("not working");
                    }
                }

            }


        private static InvoiceResponse GenerateIRN(string einvoiceToken, GSTInvoice invoiceRequest)
        {
            Console.WriteLine("Submit invoice and Generate Invoice Registration number from E-invoice API");
            using (var client = new HttpClient())
            {

                string einvoicIrnUrl = Configuration["EinvoiceIrnUrl"];
                var req = new HttpRequestMessage(HttpMethod.Post, einvoicIrnUrl);// "https://35.154.208.8/einvoice/v1.03/invoice");

                req.Content = new StringContent(JsonConvert.SerializeObject(invoiceRequest), Encoding.UTF8, "application/json");

                //req.Headers.Add("Content-Type", "application/json");
                //Console.WriteLine("Header: Content-Type: application/json");
                req.Headers.Add("Accept", "application/json");
                Console.WriteLine("Header: Accept: application/json");
                req.Headers.Add("Authorization", "Bearer " + einvoiceToken);
                Console.WriteLine("Header: Authorization: " + "Bearer " + einvoiceToken);
                req.Headers.Add("gstin", Configuration["GSTIN"]); // "29AFQPB8708K000");
                Console.WriteLine("Header: gstin: " + Configuration["GSTIN"]);
                req.Headers.Add("action", Configuration["EinvoiceGenerateAction"]);// GENERATEIRN");
                Console.WriteLine("Header: action: " + Configuration["EinvoiceGenerateAction"]);
                req.Headers.Add("X-Connector-Auth-Token", Configuration["EinvoiceXConnectorTokenForGenerate"]);// l7xxba7aa16e968646b992298b377e955e7c:20180519134451:29AFQPB8708K000");
                Console.WriteLine("Header: X-Connector-Auth-Token: " + Configuration["EinvoiceXConnectorTokenForGenerate"]);

                Console.WriteLine("Request Body:" + JsonConvert.SerializeObject(invoiceRequest));

                var responseTask = client.SendAsync(req);
                responseTask.Wait();
                var result = responseTask.Result;


                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResult = readTask.Result;

                    var invoiceResponse = JsonConvert.DeserializeObject<InvoiceResponse>(strResult);
                    Console.WriteLine(strResult);

                    return invoiceResponse;

                }
                else
                {
                    Console.WriteLine("Result:" + result);
                    throw new Exception("not working");
                }
            }

        }

        /// Gets the parameter value.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
        /// <returns></returns>
        public static string GetParamValue(string[] args, string paramName, bool isMandatory)
        {
            string value = string.Empty;
            var paramSection = args.ToList().Where(x => x.Contains("/" + paramName + "=")).FirstOrDefault();
            if (null != paramSection)
            {
                value = Regex.Split(paramSection, "/" + paramName + "=", RegexOptions.IgnoreCase)[1].Trim();
            }
            else
            {
                if (isMandatory)
                {
                    string response = "Missing argument '" + paramName + "' in Library call .";
                    Console.WriteLine(response);
                    throw new Exception(response);
                }
            }
            return value;
        }
    }
}
