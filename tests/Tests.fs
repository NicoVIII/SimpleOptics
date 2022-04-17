module Tests

open Expecto

open SimpleOptics
open SimpleOptics.Presets

let tests =
    testList
        "Tests"
        [
            testProperty "set > get: List"
            <| fun (value: int) ->
                let readValue =
                    [ 123 ]
                    |> Optic.set (ListOptic.index 0) value
                    |> Optic.get (ListOptic.index 0)

                readValue = Some value

            testProperty "set > get: Array"
            <| fun (value: int) ->
                let readValue =
                    [| 123 |]
                    |> Optic.set (ArrayOptic.index 0) value
                    |> Optic.get (ArrayOptic.index 0)

                readValue = Some value
        ]
