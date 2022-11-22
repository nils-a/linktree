using System.Text;
using Statiq.Web.Pipelines;

namespace LinkTree.Build;

///
/// Write _redirects containing all the redirects
/// TODO: Should we integrate this in the default "Redirects" pipeline? 
///
internal class SubdomainRedirects : Pipeline
{
    public SubdomainRedirects()
    {
        Dependencies.Add(nameof(Data));
        
        ProcessModules = new ModuleList(
            new RedirectsFile()
        );

        OutputModules = new ModuleList
        {
            new WriteFiles(),
        };
    }

    private class RedirectsFile : Module
    {
        protected override Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            var redirects = new StringBuilder();
            var inputs = context.Outputs.FromPipeline(nameof(Data));

            // "static" redirect from "www"
            var domain = (string)context.Settings["RootDomain"];
            redirects.AppendLine($"/forward/www https://{domain}/ 302");

            foreach (var input in inputs)
            {
                if (input == null)
                {
                    continue;
                }

                var alias = (string)input["alias"];
                var target = (string)input["target"];
                
                redirects.AppendLine($"/forward/{alias} {target} 302");
                
                var more = ((object[]?)input["alternatives"])?.Cast<string>() ?? ArraySegment<string>.Empty;
                foreach (var additionalAlias in more)
                {
                    redirects.AppendLine($"/forward/{additionalAlias} {target} 302");
                }
            }
            
            return Task.FromResult((IEnumerable<IDocument>)new[]
            {
                context.CreateDocument(
                    (NormalizedPath)"_redirects",
                    redirects.ToString()
                ),
            });
        }
    }
}