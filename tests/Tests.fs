module Tests

open Expecto

open SimpleOptics
open SimpleOptics.Presets

let tests =
    testList
        "Tests"
        [
            testList
                "ListOptic Preset"
                [
                    test "empty get" {
                        let result = Optic.get (ListOptic.index 0) []
                        Expect.isNone result "Get on an empty list should return None"
                    }

                    testProperty "set >> get"
                    <| fun (value: int) ->
                        let readValue =
                            [ 123 ]
                            |> Optic.set (ListOptic.index 0) value
                            |> Optic.get (ListOptic.index 0)

                        readValue = Some value
                ]

            testList
                "ArrayOptic Preset"
                [
                    test "empty get" {
                        let result = Optic.get (ArrayOptic.index 0) [||]
                        Expect.isNone result "Get on an empty array should return None"
                    }

                    testProperty "set >> get"
                    <| fun (value: int) ->
                        let readValue =
                            [| 123 |]
                            |> Optic.set (ArrayOptic.index 0) value
                            |> Optic.get (ArrayOptic.index 0)

                        readValue = Some value
                ]
        ]
