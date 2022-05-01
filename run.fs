open RunHelpers
open RunHelpers.BasicShortcuts
open RunHelpers.Templates

[<RequireQualifiedAccess>]
module Config =
    let project = "./src/SimpleOptics.fsproj"
    let testProject = "./tests/SimpleOptics.UnitTests.fsproj"
    let fableOutPath = "./build"
    let packPath = "./pack"

module Task =
    let restore () =
        job {
            DotNet.toolRestore ()
            DotNet.restore Config.project
        }

    let build () =
        job {
            DotNet.build Config.project Debug

            dotnet [ "fable"
                     Config.project
                     "-o"
                     Config.fableOutPath ]
        }

    let test () = DotNet.run Config.testProject

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
                Task.build ()
            }
        | []
        | [ "test" ]
        | [ "tests" ] ->
            job {
                Task.restore ()
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
