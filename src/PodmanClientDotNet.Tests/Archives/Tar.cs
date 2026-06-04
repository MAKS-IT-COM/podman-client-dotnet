using System.Text;

using ICSharpCode.SharpZipLib.Tar;

namespace MaksIT.PodmanClientDotNet.Tests.Archives;

public static class Tar {
  public static void CreateTarFromDirectory(string sourceDirectory, Stream outputStream) {
    using var tarOutputStream = new TarOutputStream(outputStream, Encoding.UTF8);
    tarOutputStream.IsStreamOwner = false;
    AddDirectoryFilesToTar(tarOutputStream, sourceDirectory, recursive: true);
  }

  static void AddDirectoryFilesToTar(
    TarOutputStream tarOutputStream,
    string sourceDirectory,
    bool recursive,
    string? baseDirectory = null
  ) {
    baseDirectory ??= sourceDirectory;

    var directoryInfo = new DirectoryInfo(sourceDirectory);

    foreach (var fileInfo in directoryInfo.GetFiles()) {
      var relativePath = Path.GetRelativePath(baseDirectory, fileInfo.FullName);

      var entry = TarEntry.CreateEntryFromFile(fileInfo.FullName);
      entry.Name = relativePath.Replace(Path.DirectorySeparatorChar, '/');
      tarOutputStream.PutNextEntry(entry);

      using var fileStream = fileInfo.OpenRead();
      fileStream.CopyTo(tarOutputStream);

      tarOutputStream.CloseEntry();
    }

    if (!recursive)
      return;

    foreach (var subDirectory in directoryInfo.GetDirectories())
      AddDirectoryFilesToTar(tarOutputStream, subDirectory.FullName, recursive: true, baseDirectory);
  }
}
