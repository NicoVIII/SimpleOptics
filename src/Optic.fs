namespace SimpleOptics

open System

[<RequireQualifiedAccess>]
module Optic =
    let inline compose optic1 optic2 = optic1 >-> optic2

    let inline get optic source = source ^. optic

    let inline set optic value = optic ^= value

    let inline map optic mapper = optic ^% mapper
