# SimpleOptics

[![Release](https://img.shields.io/github/v/release/NicoVIII/SimpleOptics?display_name=tag&sort=semver&style=flat-square)](https://github.com/NicoVIII/SimpleOptics/releases)
[![Last commit](https://img.shields.io/github/last-commit/NicoVIII/SimpleOptics?style=flat-square)](https://github.com/NicoVIII/SimpleOptics/commits/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE.txt)

This is a F# library for simple optics which is highly inspired by [Aether](https://github.com/xyncro/aether).
I also had a look at [FSharpPlus](https://github.com/fsprojects/FSharpPlus) (but I didn't understand it tbh).

I created this library to learn what optics are and how they work.

**Supported optics**
 * Lenses
 * Prisms

## What are optics

Optics are a way to work conveniently with deeply nested data structures in an immutable way.
For that you define elemental optics and then compose them to get optics for more complex structures.

To further explain this, I chose a phone book as an example.

```fsharp
type Address = {
    street: string
    city: string
}
type User = {
    adress: Address
    phoneNumber: string
}
```

If you need to change the city of a user in plain F# you have to do something like this:

```fsharp
// user: User
let user' = { user with address = { user.address with city = "Berlin" }}
```
And this doesn't get better for more nested structures. But you can use Optics to do that more conveniently.

At first you define an optic called lens for the address field in User. We'll call it UserOptic.address. Then you can create a lens for city in Address, we'll call it AddressOptic.city.
Then you can define a UserOptic for city by composing both lenses. If you have that boilerplate code you can simply set the city with the defined lens.
With this library this looks like this:
```fsharp
open SimpleOptics

// user: User
let user' = Optic.set UserOptic.city user "Berlin"
```
And this does always looks the same for more nested structures. What changes is only the optics boiler plate.

Then there are Prisms. Those are for data which could not be there like an entry in a map. You can combine Lenses and Prisms and will always get a prism back.

## How to use

For all examples you have to open the `SimpleOptics` namespace:
```fsharp
open SimpleOptics
```

### Define optics
Like in Aether you define your optics as a pair of getter and setter.
e.g.
```fsharp
// Lens<Address,City>
Lens((fun address -> address.city), (fun address city' -> { address with city = city'}))

// Prism<User,FirstName>
Prism((fun user -> user.firstName), (fun user firstName' -> { user with firstName = Some firstName' }))
```

### Combine optics

To combine optics you can either use the compose function from the Optic module or use the >-> operator:

```fsharp
module UserOptic =
    let city = Optic.compose UserOptic.address AddressOptic.city
    let street = UserOptic.address >-> AddressOptic.city
```

### Use optics

To use optics you can either use the functions from the Optic module or the equivalent operator:

```fsharp
    // Get
    let street = Optic.get UserOptic.street user
    let city = user ^. UserOptic.city

    // Set
    let user1 = Optic.set UserOptic.street "Main Street" user
    let user2 = (UserOptic.city ^= "Berlin") user

    // Map
    let user3 = Optic.map UserOptic.street (fun street -> street.ToUpper()) user
    let user4 = UserOptic.city ^% (fun city -> city.ToUpper()) user
```

### Presets

There are a few generic presets for common structures you can use. To use them you have
to open the `SimpleOptics.Presets` namespace:

```fsharp
open SimpleOptics.Presets
```

You can then use the defined presets. At the moment these are:

 * ListOptic
 * ArrayOptic
