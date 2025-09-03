# SN Tools
A mod for SN that provides tools to play the game in unintended ways

## Work In Progress...

## Licensing

This project is licensed under the [MIT License](LICENSE).

### Used third-party libraries:

- [Gameloop.Vdf](https://github.com/shravan2x/Gameloop.Vdf) — MIT License ([source](https://github.com/shravan2x/Gameloop.Vdf/blob/master/LICENSE))  
- [HarmonyX](https://github.com/BepInEx/HarmonyX) — MIT License ([source](https://github.com/BepInEx/HarmonyX/blob/master/LICENSE))  
- [Il2CppInterop](https://github.com/BepInEx/Il2CppInterop) — LGPL-3.0 License ([source](https://github.com/BepInEx/Il2CppInterop/blob/master/LICENSE))  
- [Dobby (BepInEx fork)](https://github.com/BepInEx/Dobby) — Apache-2.0 License ([source](https://github.com/BepInEx/Dobby/blob/master/LICENSE))
- [Semver](https://github.com/WalkerCodeRanger/semver) — MIT License ([source](https://github.com/WalkerCodeRanger/semver/blob/master/License.txt))
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) — MIT License ([source](https://github.com/CommunityToolkit/dotnet/blob/main/License.md))
- [Tomlet](https://github.com/SamboyCoding/Tomlet) - MIT License ([source](https://github.com/SamboyCoding/Tomlet/blob/master/LICENSE))

### Disclaimer:

This project contains [assemblies generated via Il2CppInterop](src/SNTools/Libs/Il2CppInteropAssemblies/) that provide the API surfaces of various third-party libraries.  

- These assemblies do **not include any original library code**.  
- Method implementations act as **proxies** that redirect calls to the game at runtime.  
- They are provided solely to allow compilation and interaction with the game's APIs without distributing proprietary code.  

No copyrighted code from the original libraries is included or modified.