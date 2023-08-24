using LinkTree.Build;
using Statiq.Extensions.WellKnown;
using Statiq.Web.Pipelines;

await Bootstrapper
    .Factory
    .CreateWeb(args)
    .AddSetting("SiteTitle", "Nils Andresen [Me]")
    .AddSetting("EditRoot", "https://github.com/nils-a/linktree/edit/main/input")
    .AddSetting("RootDomain", "nils-a.me")
    .AddSetting(SettingKeys.WebFingerAlias.FromTemplate, "@nils_andresen@dotnet.social")
    .ConfigureFileSystem(fs => { fs.ExcludedPaths.Add("aliases/schema.json"); })
    .ConfigureEngine(engine =>
    {
        engine.Pipelines.Remove(nameof(Feeds));
        engine.Pipelines.Remove(nameof(Sitemap));
        engine.Pipelines.Remove(nameof(Redirects));
    })
    .AddPipeline<SubdomainRedirects>()
    .AddPipeline<WellKnownFolderPipeline>()
    .RunAsync();

