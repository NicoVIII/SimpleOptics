open RunHelpers
open RunHelpers.Templates

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
        }

    let build () = DotNet.build Config.project Debug

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
        | [ "test" ] ->
            job {
                Task.restore ()
                Task.test ()
            }
        | [ "pack"; version ] ->
            job {
                Task.restore ()
                Task.pack version
            }
        | _ ->
            let msg =
                [
                    "Usage: dotnet run [<command>]"
                    "Look up available commands in run.fs"
                ]

            Error(1, msg)
    |> ProcessResult.wrapUp
