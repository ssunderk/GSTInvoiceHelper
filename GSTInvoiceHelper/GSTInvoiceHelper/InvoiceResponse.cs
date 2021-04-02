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

        [JsonPropertyName("data")]
        public Data Data;

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

    public class Data
    {
        [JsonPropertyName("AckNo")]
        public string AckNo;

        [JsonPropertyName("AckDt")]
        public string AckDt;

        [JsonPropertyName("Irn")]
        public string Irn;

        [JsonPropertyName("SignedInvoice")]
        public string SignedInvoice;

        [JsonPropertyName("SignedQRCode")]
        public string SignedQRCode;

        [JsonPropertyName("Status")]
        public string Status;

        [JsonPropertyName("EwbNo")]
        public string EwbNo;

        [JsonPropertyName("EwbDt")]
        public string EwbDt;

        [JsonPropertyName("EwbValidTill")]
        public string EwbValidTill;
    }

}
