#r "nuget: FSharpPlus, 1.2.2"
#r "nuget: Microsoft.FSharpLu, 0.11.7"
#r "nuget: FSharp.Collections.ParallelSeq"


open System
open System.IO
open FSharpPlus
open Microsoft.FSharpLu
open Microsoft.FSharpLu.Logger
open FSharp.Collections.ParallelSeq

let defaultProjectName = "GingerCodeSite"
let defaultShortName = "ginger-code-site"
let solutionRoot = Environment.CurrentDirectory
let logger = makeLogger "bootstrapper" None

let rec getProjectName () =
    printfn "Enter a name for your site:"

    match Console.ReadLine() with
    | x when String.IsNullOrWhiteSpace x -> getProjectName ()
    | x -> x

let rec getShortName () =
    printfn "Enter a short name for your site (i.e. 'ginger-code-site'):"

    match Console.ReadLine() with
    | x when String.IsNullOrWhiteSpace x -> getShortName ()
    | x -> x

let shortName = getShortName ()

let projectName = getProjectName ()


let pathFilter path =
    [ $"bin{Path.PathSeparator}"
      $"node_modules"
      $".idea"
      $".git"
      $".vs"
      $".vscode" ]
    |> List.exists (fun s -> String.isSubString s path)
    |> not

let enumerateChildren path =
    logger.write $"Enumerating children of directory '{path}'."

    Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
    |> PSeq.append (Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
    |> PSeq.filter pathFilter
    |> PSeq.map Path.GetFullPath

let updateNpmPackageContents () =
    let path =
        Path.Join(solutionRoot, "GingerCodeSite", "package.json")

    let updatedContents =
        File.ReadAllText path
        |> String.replace defaultShortName shortName

    File.WriteAllText(path, updatedContents)


let updateProjectFileContents path =
    if Directory.Exists(path) then
        path
    else
        let contents = File.ReadAllText path

        if File.contains defaultProjectName contents then
            logger.write $"Updating file contents for file: '{path}'."

            let updatedContents =
                String.replace defaultProjectName projectName contents

            File.WriteAllText(path, updatedContents)

        path

let updateProjectPath path =
    if String.isSubString defaultProjectName path then
        if Directory.Exists(path) then
            let info = DirectoryInfo(path)

            if info.Name.Contains(defaultProjectName) then
                let newName =
                    String.replace defaultProjectName projectName info.Name

                let newPath = Path.Join(info.Parent.FullName, newName)
                logger.write $"Renaming directory '{path}' to '{newPath}'."
                Directory.Move(path, newPath)
        else
            let info = FileInfo(path)

            if info.Name.Contains(defaultProjectName) then
                let newName =
                    String.replace defaultProjectName projectName info.Name

                let newPath =
                    Path.Join(info.Directory.FullName, newName)

                logger.write $"Renaming file '{path}' to '{newPath}'."
                File.Move(path, newPath)

let updateFrontendAssets () =
    logger.write "Updating NPM package"

    updateNpmPackageContents ()

let updateProjectFiles () =
    logger.write "Updating project files"

    solutionRoot
    |> enumerateChildren
    |> PSeq.iter (updateProjectFileContents >> updateProjectPath)

let cleanUpBootstrapper () =
    logger.write "Deleting bootstrapper!"

    Directory.EnumerateFiles(solutionRoot, "bootstrap*", SearchOption.TopDirectoryOnly)
    |> Seq.iter File.Delete

let bootstrap =
    updateFrontendAssets
    >> updateProjectFiles
    >> cleanUpBootstrapper

logger.write $"Bootstrapping with project name '{projectName}'."

bootstrap ()

logger.write "All done~"
