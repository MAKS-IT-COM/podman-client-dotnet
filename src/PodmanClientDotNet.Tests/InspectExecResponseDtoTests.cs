using System.Text.Json;


namespace MaksIT.PodmanClientDotNet.Tests;

public class InspectExecResponseDtoTests {
  [Fact]
  public void Deserialize_WhenProcessConfigIsObject_Succeeds() {
    const string json = """
      {
        "CanRemove": true,
        "ContainerID": "abc123",
        "DetachKeys": "",
        "ExitCode": 0,
        "ID": "exec456",
        "OpenStderr": true,
        "OpenStdin": false,
        "OpenStdout": true,
        "Running": false,
        "Pid": 0,
        "ProcessConfig": {
          "arguments": ["-c", "echo hi"],
          "entrypoint": "sh",
          "privileged": false,
          "tty": false,
          "user": ""
        }
      }
      """;

    var dto = JsonSerializer.Deserialize(json, PodmanJsonContext.Default.InspectExecResponseDto);

    Assert.NotNull(dto);
    Assert.Equal(0, dto.ExitCode);
    Assert.Equal("exec456", dto.ID);
    Assert.NotNull(dto.ProcessConfig);
    Assert.Equal("sh", dto.ProcessConfig.Entrypoint);
    Assert.Equal(new[] { "-c", "echo hi" }, dto.ProcessConfig!.Arguments);
  }
}
