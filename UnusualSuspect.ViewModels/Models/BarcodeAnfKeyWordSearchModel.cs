namespace UnusualSuspect.ViewModels.Models;

public class BarcodeAnfKeyWordSearchModel
{
    public string Searchwithbarcode { set; get; }
    public int? Searchwithkeyword { set; get; }
    public string VariantListJson { set; get; }
    public List<int> VariantIdsList { set; get; }
}
