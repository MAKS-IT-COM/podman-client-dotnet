using System.Text.Json.Serialization;

using MaksIT.PodmanClientDotNet.Dtos.Build;
using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Container;
using MaksIT.PodmanClientDotNet.Dtos.Exec;
using MaksIT.PodmanClientDotNet.Dtos.Generate;
using MaksIT.PodmanClientDotNet.Dtos.Image;
using MaksIT.PodmanClientDotNet.Dtos.Manifest;
using MaksIT.PodmanClientDotNet.Dtos.Network;
using MaksIT.PodmanClientDotNet.Dtos.Pod;
using MaksIT.PodmanClientDotNet.Dtos.System;
using MaksIT.PodmanClientDotNet.Dtos.Volume;
using MaksIT.PodmanClientDotNet.Models.Container;
using MaksIT.PodmanClientDotNet.Models.Exec;
using MaksIT.PodmanClientDotNet.Models.Network;
using MaksIT.PodmanClientDotNet.Models.Pod;
using MaksIT.PodmanClientDotNet.Models.Volume;

namespace MaksIT.PodmanClientDotNet;

// ----- Response / DTO types -----

// Common
[JsonSerializable(typeof(ErrorResponseDto))]
[JsonSerializable(typeof(IdResponseDto))]
[JsonSerializable(typeof(PruneReportDto))]
[JsonSerializable(typeof(ReportDto))]

// Build
[JsonSerializable(typeof(BuildProgressLineDto))]
[JsonSerializable(typeof(BuildReportDto))]

// Container
[JsonSerializable(typeof(ContainerChangesDto))]
[JsonSerializable(typeof(ContainerCommitDto))]
[JsonSerializable(typeof(ContainerHealthCheckDto))]
[JsonSerializable(typeof(ContainerInspectDto))]
[JsonSerializable(typeof(ContainerListEntryDto))]
[JsonSerializable(typeof(List<ContainerListEntryDto>))]
[JsonSerializable(typeof(ContainerMountDto))]
[JsonSerializable(typeof(ContainerStatsDto))]
[JsonSerializable(typeof(Dictionary<string, ContainerStatsDto>))]
[JsonSerializable(typeof(ContainerTopDto))]
[JsonSerializable(typeof(ContainerWaitDto))]
[JsonSerializable(typeof(CreateContainerResponseDto))]
[JsonSerializable(typeof(DeleteContainerResponseDto))]
[JsonSerializable(typeof(DeleteContainerResponseDto[]))]
[JsonSerializable(typeof(MountedContainersResponseDto))]

// Exec
[JsonSerializable(typeof(CreateExecResponseDto))]
[JsonSerializable(typeof(InspectExecResponseDto))]

// Generate
[JsonSerializable(typeof(GenerateSystemdDto))]
[JsonSerializable(typeof(PlayKubeReportDto))]

// Image
[JsonSerializable(typeof(ImageChangesDto))]
[JsonSerializable(typeof(ImageDeleteDto))]
[JsonSerializable(typeof(ImageDeleteDto[]))]
[JsonSerializable(typeof(ImageHistoryEntryDto))]
[JsonSerializable(typeof(List<ImageHistoryEntryDto>))]
[JsonSerializable(typeof(ImageImportDto))]
[JsonSerializable(typeof(ImageInspectDto))]
[JsonSerializable(typeof(ImageListEntryDto))]
[JsonSerializable(typeof(List<ImageListEntryDto>))]
[JsonSerializable(typeof(ImageLoadDto))]
[JsonSerializable(typeof(ImageRemoveResponseDto))]
[JsonSerializable(typeof(ImageSearchResultDto))]
[JsonSerializable(typeof(List<ImageSearchResultDto>))]
[JsonSerializable(typeof(ImageTreeDto))]
[JsonSerializable(typeof(PullImageResponseDto))]

// Manifest
[JsonSerializable(typeof(ManifestCreateDto))]
[JsonSerializable(typeof(ManifestInspectDto))]

// Network
[JsonSerializable(typeof(NetworkInspectDto))]
[JsonSerializable(typeof(NetworkListEntryDto))]
[JsonSerializable(typeof(List<NetworkListEntryDto>))]

// Pod
[JsonSerializable(typeof(PodInspectDto))]
[JsonSerializable(typeof(PodListEntryDto))]
[JsonSerializable(typeof(List<PodListEntryDto>))]
[JsonSerializable(typeof(PodTopDto))]
[JsonSerializable(typeof(PodStatsResponseDto))]

// System
[JsonSerializable(typeof(InfoDto))]
[JsonSerializable(typeof(LibpodPingDto))]
[JsonSerializable(typeof(LibpodVersionDto))]
[JsonSerializable(typeof(SystemDfDto))]

// Volume
[JsonSerializable(typeof(VolumeInspectResponseDto))]
[JsonSerializable(typeof(VolumeListEntryDto))]
[JsonSerializable(typeof(List<VolumeListEntryDto>))]

// ----- Request / model types -----
[JsonSerializable(typeof(CreateContainerRequest))]
[JsonSerializable(typeof(CreateExecRequest))]
[JsonSerializable(typeof(StartExecRequest))]
[JsonSerializable(typeof(NetworkCreateRequest))]
[JsonSerializable(typeof(NetworkConnectRequest))]
[JsonSerializable(typeof(NetworkDisconnectRequest))]
[JsonSerializable(typeof(PodCreateRequest))]
[JsonSerializable(typeof(CreateVolumeRequest))]
[JsonSerializable(typeof(ManifestAddRequestDto))]

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified, PropertyNameCaseInsensitive = true)]
internal sealed partial class PodmanJsonContext : JsonSerializerContext {
}
