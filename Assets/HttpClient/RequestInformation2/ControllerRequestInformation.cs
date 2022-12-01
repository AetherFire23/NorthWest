using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UriBuilder
{
    public Url Uri;
    public object Body;
    public bool HasRequiredBody;
    public bool HasRequiredParameter;
    public string SerializedBody => JsonConvert.SerializeObject(Body);
    public string Path => this.Uri.Path;

    public UriBuilder(string path, ParameterOptions options = ParameterOptions.None, object body = null) // Parameters are set after with add.
    {
        this.HasRequiredParameter = options == ParameterOptions.Required || options == ParameterOptions.BodyAndParameter;
        this.Uri = new Url(path, HasRequiredParameter); // Uri calls exceptions when it doesnt have parameters

        this.HasRequiredBody = ParameterOptions.BodyOnly == options || options == ParameterOptions.BodyAndParameter;
        this.Body = body;

        if (body == null && this.HasRequiredBody)
        {
            throw new NotImplementedException("Must have a body when parameterOptions is set to body");
        }
    }

    public UriBuilder WithParameter(string name, string value)
    {
        var uriParam = new UriParameter(name, value);
        this.Uri.requiredParameters.Add(uriParam);
        return this;
    }


    public void AddParameter(string name, string value)
    {
        var uriParam = new UriParameter(name, value);
        this.Uri.requiredParameters.Add(uriParam);
    }
}

