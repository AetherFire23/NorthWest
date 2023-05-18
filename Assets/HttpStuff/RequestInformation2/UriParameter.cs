using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

