module Tests

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open SimpleOptics
open SimpleOptics.Presets

// Workaround for Fable for testProperty
[<AbstractClass>]
type PropertyTest() =
#if FABLE_COMPILER
    static member Create(label, fnc: int -> bool) =
        test label {
            for x in [ 0; 1; -1; 1000; -1000 ] do
                Expect.isTrue (fnc x) $"Property test is not true with value {x}"
        }

    static member Create(label, fnc: string -> bool) =
        test label {
            for x in [ ""; "testtesttesttest"; "\"²§" ] do
                Expect.isTrue (fnc x) $"Property test is not true with value {x}"
        }
#else
    static member Create(label, fnc: 'a -> bool) = testProperty label fnc
#endif

let tests =
    testList "Tests" [
        testList "ListOptic Preset" [
            test "empty get" {
                let result = Optic.get (ListOptic.index 0) []
                Expect.isNone result "Get on an empty list should return None"
            }

            PropertyTest.Create(
                "set >> get",
                fun (value: int) ->
                    let readValue =
                        [ 123 ] |> Optic.set (ListOptic.index 0) value |> Optic.get (ListOptic.index 0)

                    readValue = Some value
            )
        ]

        testList "ArrayOptic Preset" [
            test "empty get" {
                let result = Optic.get (ArrayOptic.index 0) [||]
                Expect.isNone result "Get on an empty array should return None"
            }

            PropertyTest.Create(
                "set >> get",
                fun (value: int) ->
                    let readValue =
                        [| 123 |]
                        |> Optic.set (ArrayOptic.index 0) value
                        |> Optic.get (ArrayOptic.index 0)

                    readValue = Some value
            )
        ]

        testList "MapOptic Preset" [
            test "empty get" {
                let result = Optic.get (MapOptic.find 0) (new Map<int, string>([]))
                Expect.isNone result "Get on an empty map should return None"
            }

            PropertyTest.Create(
                "set >> get",
                fun (value: string) ->
                    let readValue =
                        new Map<int, string>([])
                        |> Optic.set (MapOptic.find 0) value
                        |> Optic.get (MapOptic.find 0)

                    readValue = Some value
            )
        ]

        testList "Optic compose" [
            PropertyTest.Create(
                "basic composition",
                fun (value: int) ->
                    let optic = Optic.compose (ListOptic.index 0) (ListOptic.index 0)
                    let result = Optic.get optic [ [ value ] ]
                    result = (Some value)
            )
        ]
    ]
