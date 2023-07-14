using System;
using System.Collections.Generic;
using System.Linq;


public class Url
{

    public string Path => _path + GetParameterLine();




    public List<UriParameter> requiredParameters = new List<UriParameter>(); // add through field initializer
    private string _path;
    private bool HasRequiredParameter;
    //
    public Url(string path, bool hasRequiredParameter)
    {
        _path = path;
        HasRequiredParameter = hasRequiredParameter;
    }

    public string GetParameterLine()
    {
        bool HasLackingParameters = !requiredParameters.Any() && HasRequiredParameter;
        if (HasLackingParameters)
            throw new NotImplementedException($"Parameters of a Uri can not be empty when {nameof(HasRequiredParameter)} is set to true.");

        // return empty parameter line if there are no parameters
        bool hasNoParameter = requiredParameters.Count == 0 && !HasRequiredParameter;
        if (hasNoParameter)
        {
            return String.Empty;
        }

        // If there ARE parameters but parameter options is set to false, throw exception
        if (!HasRequiredParameter)
            throw new NotImplementedException($"problem:{_path} when {nameof(HasRequiredParameter)} is set to false.");

        string parameterLine = "?";

        if (requiredParameters.Count == 1)
        {
            return parameterLine += requiredParameters.First().NameValuePair;
        }

        else
        {
            return parameterLine += ConstructMultipleParameterLine();
        }
    }

    private string ConstructMultipleParameterLine()
    {
        string separator = "&";
        string parameterLine = String.Empty;
        foreach (UriParameter parameter in requiredParameters)
        {
            bool isLastParameter = parameter == requiredParameters.Last();
            if (isLastParameter)
            {
                parameterLine += parameter.NameValuePair;
            }
            else
            {
                parameterLine += parameter.NameValuePair + separator;
            }

        }
        return parameterLine;
    }

}

