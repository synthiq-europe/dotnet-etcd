# Synthiq.Etcd.Protobuf

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![REUSE status](https://api.reuse.software/badge/github.com/synthiq-europe/etcd)](https://api.reuse.software/info/github.com/synthiq-europe/etcd)

gRPC/Protocol Buffers definitions for interacting with etcd from .NET. \
This package is primarily meant for authors of service client libraries.

## Usage

There are various ways to integrate this into your build.
For a client, you could integrate it into your build like so:

```xml
<!-- TODO: Ensure you have the correct (latest?) versions for your project. -->
<ItemGroup>
  <PackageReference Include="Google.Protobuf" Version="3.31.1" />
  <PackageReference Include="Grpc.Net.Client" Version="2.71.0" />
  <!-- etcd version 3.6.4, package revision 0. -->
  <PackageReference Include="Synthiq.Etcd.Protobuf" Version="3.6.4.2" />
</ItemGroup>

<ItemGroup>
  <PackageReference Include="Grpc.Tools" Version="2.72.0">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
</ItemGroup>

<!-- Source the protobuf files from the Synthiq.Etcd.Protobuf package. -->
<PropertyGroup>
  <EtcdProtoRoot>$(PkgSynthiq_Etcd_Protobuf)\contentFiles\any\any</EtcdProtoRoot>
</PropertyGroup>

<ItemGroup>
  <!-- Generated files must not overlap, to prevent compile errors. -->
  <Protobuf
    Include="$(EtcdProtoRoot)\**\*.proto"
    OutputDir="$(IntermediateOutputPath)proto\%(RecursiveDir)"
    GrpcServices="Client"
    AdditionalImportDirs="$(EtcdProtoRoot)" />
</ItemGroup>
```

Of course, you could tweak the `Protobuf` item properties to your use case.

## Versioning

The package trails the etcd version, and uses a revision suffix. \
Version `3.6.4.0` is the first release for etcd `3.6.4`.

Note that NuGet considers versions `X.X.X` and `X.X.X.0` equivalent.

## License

This package is released under the Apache 2.0 license.
