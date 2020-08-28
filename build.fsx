#r @"tools/FAKE.Core/tools/FakeLib.dll"
#load "tools/SourceLink.Fake/tools/SourceLink.fsx"
open Fake 
open System
open SourceLink

let authors = ["Geoffrey Huntley"]

// project name and description
let projectName = "GeoCoordinate.NetStandard2"
let projectDescription = "GeoCoordinate is a :Net Standard 2.1 compatible implementation of System.Device.Location.GeoCoordinate. It is an exact 1:1 API compliant implementation and will be supported until MSFT sees it fit to embed the type. Which at that point this implementation will cease development/support and you will be able to simply remove this package and everything will still work."
let projectSummary = projectDescription

// directories
let buildDir = "./src/GeoCoordinate.NetStandard2/bin"
let testResultsDir = "./testresults"
let packagingRoot = "./packaging/"
let packagingDir = packagingRoot @@ "GeoCoordinate.NetStandard2"

let releaseNotes = 
    ReadFile "RELEASENOTES.md"
    |> ReleaseNotesHelper.parseReleaseNotes

let buildMode = getBuildParamOrDefault "buildMode" "Release"

MSBuildDefaults <- { 
    MSBuildDefaults with 
        ToolsVersion = Some "14.0"
        Verbosity = Some MSBuildVerbosity.Minimal }

Target "Clean" (fun _ ->
    CleanDirs [buildDir; testResultsDir; packagingRoot; packagingDir]
)

open Fake.AssemblyInfoFile
open Fake.Testing

Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo "./SolutionInfo.cs"
      [ Attribute.Product projectName
        Attribute.Version releaseNotes.AssemblyVersion
        Attribute.FileVersion releaseNotes.AssemblyVersion
        Attribute.ComVisible false ]
)

Target "CheckProjects" (fun _ ->
    !! "./src/GeoCoordinate.NetStandard2/GeoCoordinate.NetStandard2*.csproj"
    |> Fake.MSBuild.ProjectSystem.CompareProjectsTo "./src/GeoCoordinate.NetStandard2/GeoCoordinate.NetStandard2.csproj"
)


Target "FixProjects" (fun _ ->
    !! "./src/GeoCoordinate.NetStandard2/GeoCoordinate.NetStandard2*.csproj"
    |> Fake.MSBuild.ProjectSystem.FixProjectFiles "./src/GeoCoordinate.NetStandard2/GeoCoordinate.NetStandard2.csproj"
)

let setParams defaults = {
    defaults with
        ToolsVersion = Some("14.0")
        Targets = ["Build"]
        Properties =
            [
                "Configuration", buildMode
            ]
    }

let Exec command args =
    let result = Shell.Exec(command, args)
    if result <> 0 then failwithf "%s exited with error %d" command result

Target "BuildApp" (fun _ ->
    build setParams "./src/GeoCoordinatePortable.sln"
        |> DoNothing
)

//Target "BuildMono" (fun _ ->
//    // xbuild does not support msbuild  tools version 14.0 and that is the reason
//    // for using the xbuild command directly instead of using msbuild
//    Exec "xbuild" "./src/GeoCoordinatePortable.sln /t:Build /tv:12.0 /v:m  /p:RestorePackages='False' /p:Configuration='Release' /logger:Fake.MsBuildLogger+ErrorLogger,'../src/GeoCoordinatePortable.net/tools/FAKE.Core/tools/FakeLib.dll'"

//)

// Target "UnitTests" (fun _ ->
//     !! (sprintf "./src/GeoCoordinatePortable.Tests/bin/%s/**/GeoCoordinatePortable.Tests*.dll" buildMode)
//     |> xUnit2 (fun p -> 
//             {p with
//                 HtmlOutputPath = Some (testResultsDir @@ "xunit.html") })
// )

Target "SourceLink" (fun _ ->
    [ "./src/GeoCoordinate.NetStandard2/GeoCoordinate.NetStandard2.csproj" ]
    |> Seq.iter (fun pf ->
        let proj = VsProj.LoadRelease pf
        let url = "https://raw.githubusercontent.com/charlesRollandy/GeoCoordinate.NetStandard2/{0}/%var2%"
        SourceLink.Index proj.Compiles proj.OutputFilePdb __SOURCE_DIRECTORY__ url
    )
)

Target "CreateGeoCoordinatePortablePackage" (fun _ ->
    let portableDir = packagingDir @@ "lib/portable-net45+wp80+win+wpa81/"
    CleanDirs [portableDir]

    CopyFile portableDir (buildDir @@ "Release/GeoCoordinate.NetStandard2.dll")
    CopyFile portableDir (buildDir @@ "Release/GeoCoordinate.NetStandard2.XML")
    CopyFile portableDir (buildDir @@ "Release/GeoCoordinate.NetStandard2.pdb")
    CopyFiles packagingDir ["LICENSE.md"; "README.md"; "RELEASENOTES.md"]

    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName
            Description = projectDescription
            OutputPath = packagingRoot
            Summary = projectSummary
            WorkingDir = packagingDir
            Version = releaseNotes.AssemblyVersion
            ReleaseNotes = toLines releaseNotes.Notes
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey" }) "src/GeoCoordinate.nuspec"
)

Target "Default" DoNothing

Target "CreatePackages" DoNothing

"Clean"
   ==> "AssemblyInfo"
   ==> "CheckProjects"
   ==> "BuildApp"

"Clean"
   ==> "AssemblyInfo"
   ==> "CheckProjects"
   ==> "BuildMono"

//"UnitTests"
//   ==> "Default"

"SourceLink"
   ==> "CreatePackages"

"CreateGeoCoordinatePortablePackage"
   ==> "CreatePackages"

RunTargetOrDefault "Default"