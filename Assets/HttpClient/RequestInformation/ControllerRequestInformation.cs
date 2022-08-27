using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ControllerRequestInformation
{
    public Url Uri;
    public object Body;
    public bool HasRequiredBody;
    public bool HasRequiredParameter;
    public string SerializedBody => JsonConvert.SerializeObject(Body);
    public string Path => this.Uri.Path;

    public ControllerRequestInformation(string path, ParameterOptions options = ParameterOptions.None, object body = null) // Parameters are set after with add.
    {
        this.HasRequiredParameter = options == ParameterOptions.RequiresParameter || options == ParameterOptions.RequiresBodyAndParameter;
        this.Uri = new Url(path, HasRequiredParameter); // Uri calls exceptions when it doesnt have parameters

        this.HasRequiredBody = ParameterOptions.RequiresBody == options || options == ParameterOptions.RequiresBodyAndParameter;
        this.Body = body;
        if (body == null && this.HasRequiredBody)
        {
            throw new NotImplementedException("Must have a body when parameterOptions is set to body");
        }
    }

    public void AddParameter(string name, string value)
    {
        var uriParam = new UriParameter(name, value);
        this.Uri.requiredParameters.Add(uriParam);
    }
}

