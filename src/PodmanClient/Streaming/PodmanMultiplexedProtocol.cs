using System.Buffers.Binary;

namespace MaksIT.PodmanClientDotNet.Streaming;

internal static class PodmanMultiplexedProtocol {
  public const int HeaderSize = 8;

  public static void WriteFrame(Stream stream, PodmanStreamType type, ReadOnlySpan<byte> payload) {
    Span<byte> header = stackalloc byte[HeaderSize];
    header[0] = (byte)type;
    BinaryPrimitives.WriteUInt32BigEndian(header[4..], (uint)payload.Length);
    stream.Write(header);
    if (!payload.IsEmpty)
      stream.Write(payload);
  }

  public static async Task<(PodmanStreamType Type, int PayloadSize)?> TryReadHeaderAsync(
    Stream stream,
    byte[] headerBuffer,
    CancellationToken cancellationToken
  ) {
    var read = await ReadExactAsync(stream, headerBuffer, cancellationToken).ConfigureAwait(false);
    if (read == 0)
      return null;
    if (read < HeaderSize)
      throw new EndOfStreamException("Unexpected end of multiplexed stream while reading header.");

    var size = BinaryPrimitives.ReadUInt32BigEndian(headerBuffer.AsSpan(4));
    return ((PodmanStreamType)headerBuffer[0], (int)size);
  }

  public static async Task<int> ReadExactAsync(Stream stream, Memory<byte> buffer, CancellationToken cancellationToken) {
    var total = 0;
    while (total < buffer.Length) {
      var read = await stream.ReadAsync(buffer[total..], cancellationToken).ConfigureAwait(false);
      if (read == 0)
        return total;
      total += read;
    }
    return total;
  }
}
