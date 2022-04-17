namespace SimpleOptics.Presets

open SimpleOptics

[<RequireQualifiedAccess>]
module ListOptic =
    let index<'a> i : Prism<'a list, 'a> =
        Prism((List.tryItem i), (fun list value -> List.updateAt i value list))

[<RequireQualifiedAccess>]
module ArrayOptic =
    let index<'a> i : Prism<'a array, 'a> =
        Prism((Array.tryItem i), (fun array value -> Array.updateAt i value array))

[<RequireQualifiedAccess>]
module MapOptic =
    let find<'Key, 'Value when 'Key: comparison> key : Prism<Map<'Key, 'Value>, 'Value> =
        Prism((Map.tryFind key), (fun map value -> Map.add key value map))
