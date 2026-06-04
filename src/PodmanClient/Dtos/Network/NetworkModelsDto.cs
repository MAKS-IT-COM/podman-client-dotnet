namespace MaksIT.PodmanClientDotNet.Dtos.Network;
/// <summary>
/// Deserialized Podman libpod API payload (Network List Entry).
/// </summary>

public sealed class NetworkListEntryDto {
  public string? Name { get; set; }
  public string? Id { get; set; }
  public string? Driver { get; set; }
  public DateTime Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public Dictionary<string, object>? Options { get; set; }
  public bool? IPv6Enabled { get; set; }
  public bool? Internal { get; set; }
  public bool? DNSEnabled { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Network Inspect).
/// </summary>

public sealed class NetworkInspectDto {
  public string? Name { get; set; }
  public string? Id { get; set; }
  public string? Driver { get; set; }
  public DateTime Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public Dictionary<string, object>? Options { get; set; }
  public bool? IPv6Enabled { get; set; }
  public bool? Internal { get; set; }
  public bool? DNSEnabled { get; set; }
  public Dictionary<string, NetworkInspectContainerDto>? Containers { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Network Inspect Container).
/// </summary>

public sealed class NetworkInspectContainerDto {
  public string? Name { get; set; }
  public string? EndpointID { get; set; }
  public string? MacAddress { get; set; }
  public string? IPv4Address { get; set; }
  public string? IPv6Address { get; set; }
}
