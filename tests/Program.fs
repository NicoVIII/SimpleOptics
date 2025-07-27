open System
#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif
open Tests

[<EntryPoint>]
let main args =
#if FABLE_COMPILER
    Mocha.runTests tests
#else
    runTestsWithCLIArgs [] args tests
#endif
