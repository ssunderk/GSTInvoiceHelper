using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GSTInvoiceHelper
{
    class InvoiceResponse
    {
        [JsonPropertyName("error")]
        public List<Error> Error;

        [JsonPropertyName("status")]
        public string Status;

        [JsonPropertyName("info")]
        public List<Info> Info;
    }

    public class Error
    {
        [JsonPropertyName("errorCodes")]
        public string ErrorCodes;

        [JsonPropertyName("errorMsg")]
        public string ErrorMsg;
    }

    public class Desc
    {
        [JsonPropertyName("Irn")]
        public string Irn;

        [JsonPropertyName("AckNo")]
        public long AckNo;

        [JsonPropertyName("AckDt")]
        public string AckDt;
    }

    public class Info
    {
        [JsonPropertyName("Desc")]
        public Desc Desc;

        [JsonPropertyName("InfCd")]
        public string InfCd;
    }

 
}
