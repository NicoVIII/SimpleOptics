module Tests

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open SimpleOptics
open SimpleOptics.Presets

// Stub for fable for testProperty
#if FABLE_COMPILER
let testProperty (label: string) (fnc: 'a -> bool) = test $"[not functional] {label}" { () }
#endif

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

            testList
                "MapOptic Preset"
                [
                    test "empty get" {
                        let result = Optic.get (MapOptic.find 0) (new Map<int, string>([]))
                        Expect.isNone result "Get on an empty map should return None"
                    }

                    testProperty "set >> get"
                    <| fun (value: string) ->
                        let readValue =
                            new Map<int, string>([])
                            |> Optic.set (MapOptic.find 0) value
                            |> Optic.get (MapOptic.find 0)

                        readValue = Some value
                ]

            testList
                "Optic compose"
                [
                    test "basic compose" {
                        let constant = 5
                        let optic = Optic.compose (ListOptic.index 0) (ListOptic.index 0)
                        let result = Optic.get optic [ [ constant ] ]
                        Expect.equal result (Some constant) "Composing optics should work"
                    }
                ]
        ]
