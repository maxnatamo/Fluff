using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    [Parameter("Name of the NuGet source")]
    readonly string NugetSourceName = "gitlab";

    [Parameter("NuGet Source for packages")]
    readonly string NugetSource;

    [Parameter("NuGet username")]
    readonly string NugetUsername;

    [Parameter("NuGet password")]
    readonly string NugetPassword;

    Target Pack => _ => _
        .DependsOn(Compile, Format)
        .Produces(NuGetArtifactsDirectory / "*.nupkg")
        .Produces(NuGetArtifactsDirectory / "*.snupkg")
        .Requires(() => Configuration.IsRelease)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(MainSolutionFile)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetIncludeSymbols(true)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .SetIncludeSource(false)
                .SetDescription("Homebrew HTTP server for .NET Core")
                .SetAuthors("Max T. Kristiansen")
                .SetCopyright("Copyright (c) Max T. Kristiansen 2023")
                .SetPackageTags("c# core library log pretty-print")
                .SetPackageProjectUrl("https://github.com/maxnatamo/fluff")
                .SetNoDependencies(false)
                .SetOutputDirectory(NuGetArtifactsDirectory)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetVersion(GitVersion.SemVer));
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Consumes(Pack)
        .Requires(() => NugetSourceName)
        .Requires(() => NugetSource)
        .Requires(() => NugetUsername)
        .Requires(() => NugetPassword)
        .Requires(() => Configuration.IsRelease)
        .Executes(() =>
        {
            var packages = NuGetArtifactsDirectory.GlobFiles("*.nupkg");

            DotNetNuGetAddSource(c => c
                .SetName(NugetSourceName)
                .SetSource(NugetSource)
                .SetUsername(NugetUsername)
                .SetPassword(NugetPassword)
                .SetStorePasswordInClearText(true));

            DotNetNuGetPush(c => c
                .SetSource(NugetSource)
                .CombineWith(packages, (s, v) => s.SetTargetPath(v)));
        });
}