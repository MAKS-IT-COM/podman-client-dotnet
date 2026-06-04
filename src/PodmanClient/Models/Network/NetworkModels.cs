namespace MaksIT.PodmanClientDotNet.Models.Network;

/// <summary>
/// Libpod API request body for Network Create request.
/// </summary>

public sealed class NetworkCreateRequest {
  public string? Name { get; set; }
  public bool? DisableDNS { get; set; }
  public string? Driver { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public Dictionary<string, string>? Options { get; set; }
  public bool? Internal { get; set; }
  public List<string>? IPAMDriverConfigs { get; set; }
  public List<string>? DNSServers { get; set; }
  public List<string>? DNSSearchDomains { get; set; }
  public List<string>? DNSOptions { get; set; }
  public List<string>? Subnets { get; set; }
  public List<string>? IPv6Subnets { get; set; }
  public List<string>? NetworkInterface { get; set; }
  public List<string>? NetworkID { get; set; }
  public List<string>? NetworkName { get; set; }
  public List<string>? OptionsList { get; set; }
}

/// <summary>
/// Libpod API request body to connect a container to networks.
/// </summary>
public sealed class NetworkConnectRequest {
  public string? Container { get; set; }
  public List<string>? Networks { get; set; }
}

/// <summary>
/// Libpod API request body to disconnect a container from a network.
/// </summary>
public sealed class NetworkDisconnectRequest {
  public string? Container { get; set; }
  public bool? Force { get; set; }
}
