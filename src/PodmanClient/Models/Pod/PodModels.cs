namespace MaksIT.PodmanClientDotNet.Models.Pod;

/// <summary>
/// Libpod API request body for Pod Create request.
/// </summary>

public sealed class PodCreateRequest {
  public string? Name { get; set; }
  public string? CgroupParent { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? RestartPolicy { get; set; }
  public ulong? StopTimeout { get; set; }
  public string? ShareIpc { get; set; }
  public string? ShareNet { get; set; }
  public string? SharePid { get; set; }
  public string? ShareUts { get; set; }
  public string? ShareUser { get; set; }
  public string? Hostname { get; set; }
  public List<string>? DNSOption { get; set; }
  public List<string>? DNSSearch { get; set; }
  public List<string>? DNSServer { get; set; }
  public List<string>? Sysctl { get; set; }
  public List<string>? NetNsPath { get; set; }
}
