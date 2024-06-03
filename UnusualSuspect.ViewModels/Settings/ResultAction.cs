namespace UnusualSuspect.ViewModels.Settings;

public class ResultAction
{
    public bool Success { set; get; }
    public string TitleResult { set; get; }
    public string MessageList { set; get; }
    public string Id { set; get; }

    public string Params1 { set; get; }
    public string Params2 { set; get; }

    public string Params3 { set; get; }
    public bool CreateEdit { set; get; }
    public string Url { set; get; }

}
public class ResultAction<T> : ResultAction
{
    public T Data { set; get; }
}
public class ResultActionMessage
{
    public bool Success { set; get; }
    public string TitleResult { set; get; }
    public string MessageList { set; get; }
    public int Id { set; get; }
    public string ReceiverUserName { get; set; }

    public string SenderUserName { set; get; }

    public string ConnectionId { set; get; }
    public bool CreateEdit { set; get; }
    public string FileContentType { get; set; }
    public string FilePatch { get; set; }
    public string sendDate { get; set; }
    public int MessageTypeId { get; set; }
}
