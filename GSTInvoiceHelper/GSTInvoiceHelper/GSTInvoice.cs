using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GSTInvoiceHelper
{

    class GSTInvoice
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class TranDtls
    {
        [JsonPropertyName("taxSch")]
        public string TaxSch { get; set; }

        [JsonPropertyName("supTyp")]
        public string SupTyp { get; set; }

        [JsonPropertyName("regRev")]
        public string RegRev { get; set; }

        [JsonPropertyName("igstOnIntra")]
        public string IgstOnIntra { get; set; }

    }

    public class DocDtls
    {
        [JsonPropertyName("Typ")]
        public string Typ { get; set; }

        [JsonPropertyName("No")]
        public string No { get; set; }

        [JsonPropertyName("Dt")]
        public string Dt { get; set; }
    }

    public class SellerDtls
    {
        [JsonPropertyName("Gstin")]
        public string Gstin { get; set; }

        [JsonPropertyName("TrdNm")]
        public string TrdNm { get; set; }

        [JsonPropertyName("lglNm")]
        public string LglNm { get; set; }

        [JsonPropertyName("addr1")]
        public string Addr1 { get; set; }

        [JsonPropertyName("addr2")]
        public string Addr2 { get; set; }

        [JsonPropertyName("Bno")]
        public string Bno { get; set; }

        [JsonPropertyName("Bnm")]
        public string Bnm { get; set; }

        [JsonPropertyName("Flno")]
        public string Flno { get; set; }

        [JsonPropertyName("Loc")]
        public string Loc { get; set; }

        [JsonPropertyName("Dst")]
        public string Dst { get; set; }

        [JsonPropertyName("Pin")]
        public int Pin { get; set; }

        [JsonPropertyName("Stcd")]
        public int Stcd { get; set; }

        [JsonPropertyName("Ph")]
        public long Ph { get; set; }

        [JsonPropertyName("Em")]
        public string Em { get; set; }
    }

    public class BuyerDtls
    {
        [JsonPropertyName("gstin")]
        public string Gstin { get; set; }

        [JsonPropertyName("trdNm")]
        public string TrdNm { get; set; }

        [JsonPropertyName("lglNm")]
        public string LglNm { get; set; }
        
        [JsonPropertyName("addr1")]
        public string Addr1 { get; set; }

        [JsonPropertyName("addr2")]
        public string Addr2 { get; set; }

        [JsonPropertyName("Bno")]
        public string Bno { get; set; }

        [JsonPropertyName("Bnm")]
        public string Bnm { get; set; }

        [JsonPropertyName("Flno")]
        public string Flno { get; set; }

        [JsonPropertyName("Loc")]
        public string Loc { get; set; }

        [JsonPropertyName("Dst")]
        public string Dst { get; set; }

        [JsonPropertyName("Pin")]
        public int Pin { get; set; }

        [JsonPropertyName("Stcd")]
        public int Stcd { get; set; }

        [JsonPropertyName("Ph")]
        public long Ph { get; set; }

        [JsonPropertyName("Em")]
        public string Em { get; set; }

        [JsonPropertyName("pos")]
        public string Pos { get; set; }
    }

    public class ValDtls
    {
        [JsonPropertyName("AssVal")]
        public int AssVal { get; set; }

        [JsonPropertyName("SgstVal")]
        public int SgstVal { get; set; }

        [JsonPropertyName("CgstVal")]
        public int CgstVal { get; set; }

        [JsonPropertyName("IgstVal")]
        public int IgstVal { get; set; }

        [JsonPropertyName("CesVal")]
        public int CesVal { get; set; }

        [JsonPropertyName("StCesVal")]
        public int StCesVal { get; set; }

        [JsonPropertyName("CesNonAdVal")]
        public int CesNonAdVal { get; set; }

        [JsonPropertyName("Disc")]
        public int Disc { get; set; }

        [JsonPropertyName("OthChrg")]
        public int OthChrg { get; set; }

        [JsonPropertyName("TotInvVal")]
        public int TotInvVal { get; set; }
    }

    public class BchDtls
    {
        [JsonPropertyName("Nm")]
        public string Nm { get; set; }

        [JsonPropertyName("ExpDt")]
        public string ExpDt { get; set; }

        [JsonPropertyName("WrDt")]
        public string WrDt { get; set; }
    }

    public class ItemList
    {
        
        [JsonPropertyName("slNo")]
        public string SlNo { get; set; }

        [JsonPropertyName("PrdNm")]
        public string PrdNm { get; set; }

        [JsonPropertyName("PrdDesc")]
        public string PrdDesc { get; set; }

        [JsonPropertyName("isServc")]
        public string IsServc { get; set; }
        
        [JsonPropertyName("HsnCd")]
        public string HsnCd { get; set; }

        [JsonPropertyName("Barcde")]
        public string Barcde { get; set; }

        [JsonPropertyName("Qty")]
        public int Qty { get; set; }

        [JsonPropertyName("FreeQty")]
        public int FreeQty { get; set; }

        [JsonPropertyName("Unit")]
        public string Unit { get; set; }

        [JsonPropertyName("UnitPrice")]
        public int UnitPrice { get; set; }

        [JsonPropertyName("TotAmt")]
        public int TotAmt { get; set; }

        [JsonPropertyName("Discount")]
        public int Discount { get; set; }

        [JsonPropertyName("OthChrg")]
        public int OthChrg { get; set; }

        [JsonPropertyName("AssAmt")]
        public int AssAmt { get; set; }

        [JsonPropertyName("SgstRt")]
        public int SgstRt { get; set; }

        [JsonPropertyName("gstRt")]
        public int GstRt { get; set; }

        [JsonPropertyName("CgstRt")]
        public int CgstRt { get; set; }

        [JsonPropertyName("IgstRt")]
        public int IgstRt { get; set; }

        [JsonPropertyName("CesRt")]
        public int CesRt { get; set; }

        [JsonPropertyName("CesNonAdval")]
        public int CesNonAdval { get; set; }

        [JsonPropertyName("StateCes")]
        public int StateCes { get; set; }

        [JsonPropertyName("TotItemVal")]
        public double TotItemVal { get; set; }

        [JsonPropertyName("BchDtls")]
        public BchDtls BchDtls { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("TaxSch")]
        public string TaxSch { get; set; }

        [JsonPropertyName("Version")]
        public string Version { get; set; }

        [JsonPropertyName("Irn")]
        public string Irn { get; set; }

        [JsonPropertyName("TranDtls")]
        public TranDtls TranDtls { get; set; }

        [JsonPropertyName("DocDtls")]
        public DocDtls DocDtls { get; set; }

        [JsonPropertyName("SellerDtls")]
        public SellerDtls SellerDtls { get; set; }

        [JsonPropertyName("BuyerDtls")]
        public BuyerDtls BuyerDtls { get; set; }

        [JsonPropertyName("ValDtls")]
        public ValDtls ValDtls { get; set; }

        [JsonPropertyName("ItemList")]
        public List<ItemList> ItemList { get; set; }
    }
}
