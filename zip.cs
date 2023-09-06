using System.IO.Compression;

namespace novorender
{
    static class Zip
    {
        // Write a zip file with specified number of files of 1MiB each with random content.
        public static void Write(Stream stream, int files, CancellationToken cancellationToken)
        {
            var seed = 123;
            var rnd = new Random(seed);
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                var buffer = new byte[0x100000];
                int i = 0;

                // Added cancellation token to break any ongoing loop
                while (i < files && !cancellationToken.IsCancellationRequested)
                {
                    rnd.NextBytes(buffer);
                    var name = i.ToString();
                    var entry = archive.CreateEntry(name);
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(buffer);
                    }
                    i++;
                }
            }
        }
    }
}
