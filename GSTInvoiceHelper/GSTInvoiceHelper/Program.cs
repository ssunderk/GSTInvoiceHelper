using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

namespace GSTInvoiceHelper
{
    class Program
    {
            static void Main(string[] args)
            {

                var token = getToken();
                var authToken = Authenticate(token);
                Console.ReadLine();

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

                    string vendorAccesstokenUrl = ConfigurationManager.AppSettings["EinvoiceAuthenticateUrl"];
                    var req = new HttpRequestMessage(HttpMethod.Post, vendorAccesstokenUrl);// "https://35.154.208.8/einvoice/v1.03/authentication");

                    req.Content = new StringContent(JsonConvert.SerializeObject(authRequest));

                    req.Headers.Add("Authorization", "Bearer " + token.access_token);
                    req.Headers.Add("gstin", "29AFQPB8708K000");
                    req.Headers.Add("action", "ACCESSTOKEN");
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
        

    }
}
