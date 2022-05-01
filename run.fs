open RunHelpers
open RunHelpers.BasicShortcuts
open RunHelpers.Templates
open System.IO

[<RequireQualifiedAccess>]
module Config =
    let project = "./src/SimpleOptics.fsproj"
    let testProject = "./tests/SimpleOptics.UnitTests.fsproj"
    let packPath = "./pack"

module Task =
    let restore () =
        job {
            DotNet.toolRestore ()
            DotNet.restore Config.project
            DotNet.restore Config.testProject
        }

    let femto () =
        job {
            dotnet [ "femto"
                     "--resolve"
                     Config.testProject ]
        }

    let build () =
        job {
            DotNet.build Config.project Debug
            DotNet.build Config.testProject Debug

            // Build stuff in fable
            dotnet [ "fable"
                     Config.project
                     "-o"
                     (Path.GetDirectoryName(Config.project) + "/dist") ]

            dotnet [ "fable"
                     Config.testProject
                     "-e"
                     ".test.js"
                     "-o"
                     (Path.GetDirectoryName(Config.testProject)
                      + "/dist") ]
        }

    let test () =
        job {
            DotNet.run Config.testProject

            // Run fable tests
            pnpm [ "run"; "test" ]
        }

    let pack version =
        DotNet.pack Config.packPath Config.project version

[<EntryPoint>]
let main args =
    args
    |> List.ofArray
    |> function
        | [ "restore" ] -> Task.restore ()
        | [ "build" ] ->
            job {
                Task.restore ()
                Task.femto ()
                Task.build ()
            }
        | []
        | [ "test" ]
        | [ "tests" ] ->
            job {
                Task.restore ()
                Task.femto ()
                Task.build ()
                Task.test ()
            }
        | [ "pack"; version ] ->
            job {
                Task.restore ()
                Task.pack version
            }
        | _ ->
            Job.error [ "Usage: dotnet run [<command>]"
                        "Look up available commands in run.fs" ]
    |> Job.execute
