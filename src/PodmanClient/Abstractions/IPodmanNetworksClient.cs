using MaksIT.PodmanClientDotNet.Dtos.Network;
using MaksIT.PodmanClientDotNet.Models.Network;
using MaksIT.Results;

/// <summary>
/// Network create, list, inspect, delete, connect, and disconnect endpoints.
/// </summary>
public interface IPodmanNetworksClient {
  Task<Result<NetworkListEntryDto?>> CreateNetworkAsync(NetworkCreateRequest request, CancellationToken cancellationToken = default);
  Task<Result<List<NetworkListEntryDto>?>> ListNetworksAsync(CancellationToken cancellationToken = default);
  Task<Result<NetworkInspectDto?>> InspectNetworkAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> DeleteNetworkAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> ConnectNetworkAsync(string name, NetworkConnectRequest request, CancellationToken cancellationToken = default);
  Task<Result> DisconnectNetworkAsync(string name, NetworkDisconnectRequest request, CancellationToken cancellationToken = default);
}
