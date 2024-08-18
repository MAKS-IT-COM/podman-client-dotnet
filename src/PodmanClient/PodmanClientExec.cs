using System.Text;

using Microsoft.Extensions.Logging;

using MaksIT.PodmanClientDotNet.Models;
using MaksIT.PodmanClientDotNet.Models.Exec;
using MaksIT.PodmanClientDotNet.Extensions;


namespace MaksIT.PodmanClientDotNet {
  public partial class PodmanClient {

    public async Task<CreateExecResponse?> CreateExecAsync(
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
      var requestUrl = $"/{_apiVersion}/containers/{Uri.EscapeDataString(containerName)}/exec";

      // Send the POST request
      var response = await _httpClient.PostAsync(requestUrl, content);

      if (response.IsSuccessStatusCode) {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return !string.IsNullOrWhiteSpace(jsonResponse) 
          ? jsonResponse.ToObject<CreateExecResponse>()
          : null;
      }
      else {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var errorResponse = jsonResponse.ToObject<ErrorResponse>();

        // Handle different response codes
        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.NotFound:
            _logger.LogInformation($"No such container: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.Conflict:
            _logger.LogInformation($"Conflict error: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.InternalServerError:
            _logger.LogInformation($"Internal server error: {errorResponse?.Message}");
            break;
          default:
            _logger.LogInformation($"Error creating exec instance: {errorResponse?.Message}");
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
    int? width = null
) {
      // Construct the request object
      var startExecRequest = new StartExecRequest {
        Detach = detach,
        Tty = tty,
        Height = height,
        Width = width
      };

      // Serialize the request object to JSON
      var jsonRequest = startExecRequest.ToJson();
      var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

      // Create the request URL
      var requestUrl = $"/{_apiVersion}/exec/{Uri.EscapeDataString(execId)}/start";

      // Send the POST request
      var response = await _httpClient.PostAsync(requestUrl, content);

      if (response.IsSuccessStatusCode) {
        var stringResponse = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"Command executed successfully.\n\n{stringResponse}".Trim());
      }
      else {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var errorResponse = jsonResponse.ToObject<ErrorResponse>();

        // Handle different response codes
        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.NotFound:
            _logger.LogWarning($"No such exec instance: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.Conflict:
            _logger.LogError($"Conflict error: {errorResponse?.Message}");
            break;
          case System.Net.HttpStatusCode.InternalServerError:
            _logger.LogError($"Internal server error: {errorResponse?.Message}");
            break;
          default:
            _logger.LogError($"Error starting exec instance: {errorResponse?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode(); // Throws an exception if the response indicates an error
      }
    }


    public async Task<InspectExecResponse?> InspectExecAsync(string execId) {
      var requestUrl = $"/{_apiVersion}/exec/{Uri.EscapeDataString(execId)}/json";
      var response = await _httpClient.GetAsync(requestUrl);

      if (response.IsSuccessStatusCode) {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return !string.IsNullOrWhiteSpace(jsonResponse)
         ? jsonResponse.ToObject<InspectExecResponse>()
         : null;
      }
      else {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var errorResponse = jsonResponse.ToObject<ErrorResponse>();

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.NotFound:
            _logger.LogWarning($"No such exec instance: {errorResponse?.Message}");
            return null;

          case System.Net.HttpStatusCode.InternalServerError:
            _logger.LogError($"Internal server error: {errorResponse?.Message}");
            break;

          default:
            _logger.LogError($"Error inspecting exec instance: {errorResponse?.Message}");
            break;
        }

        return null;
      }
    }

  }
}
