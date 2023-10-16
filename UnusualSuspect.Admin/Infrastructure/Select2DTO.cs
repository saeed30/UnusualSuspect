namespace UnusualSuspect.Admin.Infrastructure;

public class Select2DTO
{
    public Select2DTO(int id, string text)
    {
        this.id = id.ToString();
        this.text = text;
    }
    public Select2DTO(string id, string text)
    {
        this.id = id;
        this.text = text;
    }
    public string id { get; set; }
    public string text { get; set; }
}
