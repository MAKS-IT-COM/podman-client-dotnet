using MaksIT.PodmanClientDotNet.Dtos.Network;
using MaksIT.PodmanClientDotNet.Models.Network;
using MaksIT.Results;

public partial class PodmanClient {
  public Task<Result<NetworkListEntryDto?>> CreateNetworkAsync(
    NetworkCreateRequest request,
    CancellationToken cancellationToken = default
  ) =>
    PostJsonAsync<NetworkCreateRequest, NetworkListEntryDto>(
      "/libpod/networks/create",
      "Create network",
      request,
      cancellationToken: cancellationToken
    );

  public Task<Result<List<NetworkListEntryDto>?>> ListNetworksAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<List<NetworkListEntryDto>>("/libpod/networks/json", "List networks", cancellationToken: cancellationToken);

  public Task<Result<NetworkInspectDto?>> InspectNetworkAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<NetworkInspectDto>($"/libpod/networks/{Uri.EscapeDataString(name)}/json", "Inspect network", cancellationToken: cancellationToken);

  public Task<Result> DeleteNetworkAsync(string name, CancellationToken cancellationToken = default) =>
    DeleteWithoutBodyAsync($"/libpod/networks/{Uri.EscapeDataString(name)}", "Delete network", cancellationToken: cancellationToken);

  public Task<Result> ConnectNetworkAsync(
    string name,
    NetworkConnectRequest request,
    CancellationToken cancellationToken = default
  ) =>
    PostJsonWithoutBodyAsync(
      $"/libpod/networks/{Uri.EscapeDataString(name)}/connect",
      "Connect network",
      request,
      cancellationToken: cancellationToken
    );

  public Task<Result> DisconnectNetworkAsync(
    string name,
    NetworkDisconnectRequest request,
    CancellationToken cancellationToken = default
  ) =>
    PostJsonWithoutBodyAsync(
      $"/libpod/networks/{Uri.EscapeDataString(name)}/disconnect",
      "Disconnect network",
      request,
      cancellationToken: cancellationToken
    );
}
