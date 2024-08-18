using ICSharpCode.SharpZipLib.Tar;

namespace MaksIT.PodmanClientDotNet.Tests.Archives
{
    public static class Tar
    {
        public static void CreateTarFromDirectory(string sourceDirectory, Stream outputStream)
        {
            using (var tarOutputStream = new TarOutputStream(outputStream))
            {
                tarOutputStream.IsStreamOwner = false;
                AddDirectoryFilesToTar(tarOutputStream, sourceDirectory, true);
            }
        }

        static void AddDirectoryFilesToTar(TarOutputStream tarOutputStream, string sourceDirectory, bool recursive, string baseDirectory = null)
        {
            // If baseDirectory is null, set it to the sourceDirectory to start with
            if (baseDirectory == null)
            {
                baseDirectory = sourceDirectory;
            }

            var directoryInfo = new DirectoryInfo(sourceDirectory);

            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                // Calculate the relative path for the file within the base directory
                string relativePath = Path.GetRelativePath(baseDirectory, fileInfo.FullName);

                // Create tar entry with the relative path
                var entry = TarEntry.CreateEntryFromFile(fileInfo.FullName);
                entry.Name = relativePath.Replace(Path.DirectorySeparatorChar, '/'); // Use Unix-style path separators
                tarOutputStream.PutNextEntry(entry);

                using (var fileStream = fileInfo.OpenRead())
                {
                    fileStream.CopyTo(tarOutputStream);
                }

                tarOutputStream.CloseEntry();
            }

            if (recursive)
            {
                foreach (var subDirectory in directoryInfo.GetDirectories())
                {
                    // Recurse into subdirectories, passing the base directory
                    AddDirectoryFilesToTar(tarOutputStream, subDirectory.FullName, true, baseDirectory);
                }
            }
        }
    }
}
