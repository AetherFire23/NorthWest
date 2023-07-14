public class UriParameter
{
    public string Name;
    public string Value;
    public string NameValuePair => $"{Name}={Value}";

    public UriParameter(string name, string value)
    {
        Name = name;
        Value = value;
    }
}

