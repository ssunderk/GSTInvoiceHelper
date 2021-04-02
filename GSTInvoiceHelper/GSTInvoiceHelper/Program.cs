using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace GSTInvoiceHelper
{
    class Program
    {
            static void Main(string[] args)
            {
                string action = GetParamValue(args, "action", true);
                string vendortoken = GetParamValue(args, "vendortoken", false);
                string einvoicetoken = GetParamValue(args, "einvoicetoken", false);

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
                using (var client = new HttpClient())
                {
                    var urlPparams = new List<KeyValuePair<string, string>>();
                    urlPparams.Add(new KeyValuePair<string, string>("grant_type", ConfigurationManager.AppSettings["GstVendorAuthGrantType"]));// password"));
                    urlPparams.Add(new KeyValuePair<string, string>("username", ConfigurationManager.AppSettings["GstVendorAuthUsername"]));// "erp1@perennialsys.com"));
                    urlPparams.Add(new KeyValuePair<string, string>("password", ConfigurationManager.AppSettings["GstVendorAuthPassword"]));// "einv12345"));
                    urlPparams.Add(new KeyValuePair<string, string>("client_id", ConfigurationManager.AppSettings["GstVendorAuthClientId"]));// "testerpclient"));

                    string url = ConfigurationManager.AppSettings["GstVendorAuthenticateUrl"];// 
                    var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(urlPparams), };

                    req.Headers.Add("Authorization", ConfigurationManager.AppSettings["BasicAuthHeader"]);//Basic dGVzdGVycGNsaWVudDpBZG1pbkAxMjM=");
                    req.Headers.Add("gstin", ConfigurationManager.AppSettings["GstVendorAuthClientId"]); // "29AFQPB8708K000");
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
                using (var client = new HttpClient())
                {

                    var authRequest = new AuthRequest()
                    {
                        action = ConfigurationManager.AppSettings["EinvoiceAuthAction"],// "ACCESSTOKEN",                    
                        userName = ConfigurationManager.AppSettings["EinvoiceAuthUsername"], //"perennialsystems"
                        password = ConfigurationManager.AppSettings["EinvoiceAuthPassword"] //"p3r3nn!@1",
                    };

                    string einvoicAuthUrl = ConfigurationManager.AppSettings["EinvoiceAuthenticateUrl"];
                    var req = new HttpRequestMessage(HttpMethod.Post, einvoicAuthUrl);// "https://35.154.208.8/einvoice/v1.03/authentication");

                    req.Content = new StringContent(JsonConvert.SerializeObject(authRequest));

                    req.Headers.Add("Authorization", "Bearer " + token.access_token);
                    req.Headers.Add("gstin", ConfigurationManager.AppSettings["GSTIN"]); // "29AFQPB8708K000");
                    req.Headers.Add("action", ConfigurationManager.AppSettings["EinvoiceAuthAction"]);// ACCESSTOKEN");
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
