using System.Text;
using System.Text.Json;

using MaksIT.PodmanClientDotNet.Extensions;
using MaksIT.PodmanClientDotNet.Models.Exec;
using MaksIT.PodmanClientDotNet.Models;

namespace MaksIT.PodmanClientDotNet {
  public partial class PodmanClient {

    public async Task<CreateExecResponse> CreateExecAsync(
        string containerName,
        string[] cmd,
        bool attachStderr = true,
        bool attachStdin = false,
        bool attachStdout = true,
        string detachKeys = null,
        string[] env = null,
        bool privileged = false,
        bool tty = false,
        string user = null,
        string workingDir = null
      ) {
      // Construct the request object
      var execRequest = new CreateExecRequest {
        AttachStderr = attachStderr,
        AttachStdin = attachStdin,
        AttachStdout = attachStdout,
        Cmd = cmd,
        DetachKeys = detachKeys,
        Env = env,
        Privileged = privileged,
        Tty = tty,
        User = user,
        WorkingDir = workingDir
      };

      // Serialize the request object to JSON
      var jsonRequest = execRequest.ToJson();
      var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

      // Create the request URL
      var requestUrl = $"/containers/{Uri.EscapeDataString(containerName)}/exec";

      // Send the POST request
      var response = await _httpClient.PostAsync(requestUrl, content);

      if (response.IsSuccessStatusCode) {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return jsonResponse.ToObject<CreateExecResponse>();
      }
      else {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(jsonResponse);

        // Handle different response codes
        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such container: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.Conflict:
            Console.WriteLine($"Conflict error: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorResponse?.Message}");
            break;
          default:
            Console.WriteLine($"Error creating exec instance: {errorResponse?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
        return null;
      }
    }

    public async Task StartExecAsync(
      string execId,
      bool detach = false,
      bool tty = false,
      int? height = null,
      int? width = null,
      string outputFilePath = "exec_output.log"
    ) {

      outputFilePath = Path.Combine(Path.GetTempPath(), outputFilePath);

      // Construct the request object
      var startExecRequest = new StartExecRequest {
        Detach = detach,
        Tty = tty,
        Height = height,
        Width = width
      };

      // Serialize the request object to JSON
      var jsonRequest = JsonSerializer.Serialize(startExecRequest);
      var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

      // Create the request URL
      var requestUrl = $"/exec/{Uri.EscapeDataString(execId)}/start";

      // Send the POST request
      var response = await _httpClient.PostAsync(requestUrl, content);

      if (response.IsSuccessStatusCode) {
        // Write the response stream directly to a file
        using (var responseStream = await response.Content.ReadAsStreamAsync())
        using (var fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write)) {
          await responseStream.CopyToAsync(fileStream);
        }
        var test = File.ReadAllText(outputFilePath);
        Console.WriteLine($"Exec instance started and output written to {outputFilePath}");
      }
      else {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(jsonResponse);

        // Handle different response codes
        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such exec instance: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.Conflict:
            Console.WriteLine($"Conflict error: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorResponse?.Message}");
            break;
          default:
            Console.WriteLine($"Error starting exec instance: {errorResponse?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode(); // Throws an exception if the response indicates an error
      }
    }

    public async Task<InspectExecResponse?> InspectExecAsync(string execId) {
      var requestUrl = $"/exec/{Uri.EscapeDataString(execId)}/json";
      var response = await _httpClient.GetAsync(requestUrl);

      if (response.IsSuccessStatusCode) {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return jsonResponse.ToObject<InspectExecResponse>();
      }
      else {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(jsonResponse);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such exec instance: {errorResponse?.Message}");
            return null;

          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorResponse?.Message}");
            break;

          default:
            Console.WriteLine($"Error inspecting exec instance: {errorResponse?.Message}");
            break;
        }

        return null;
      }
    }

  }
}
