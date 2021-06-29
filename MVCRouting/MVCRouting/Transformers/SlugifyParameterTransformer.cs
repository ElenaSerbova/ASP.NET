using Microsoft.AspNetCore.Routing;
using System;
using System.Text.RegularExpressions;


namespace MVCRouting.Transformers
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null) { return null; }

            return Regex.Replace(value.ToString(),
                                 @"\s",
                                 "_",
                                 RegexOptions.CultureInvariant,
                                 TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
        }
    }
}
