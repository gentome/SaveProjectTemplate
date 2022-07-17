using System.IO;

namespace Gentome
{
    public class PackageFolderCreator
    {
        public string DirPath;

        public readonly string CachePath;
        public readonly string PackagePath;
        public readonly string ProjectDataPath;
        public PackageFolderCreator()
        {
            CachePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "gentomecache";
            PackagePath = $"{CachePath}{Path.DirectorySeparatorChar}package";
            ProjectDataPath = $"{PackagePath}{Path.DirectorySeparatorChar}ProjectData~";
            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
            }

            if (!Directory.Exists(PackagePath))
            {
                Directory.CreateDirectory(PackagePath);
            }
            
        }
        public string Create()
        {

            return "";
        }

        public void AddDir(string assetDirPaht)
        {
            if (string.IsNullOrEmpty(assetDirPaht))
            {
                return;
            }

            if (string.IsNullOrEmpty(DirPath))
            {
                return;
            }
            DirectoryInfo info = new DirectoryInfo(assetDirPaht);
            if (!info.Exists)
            {
                return;
            }
            Directory.Move(info.FullName,DirPath);
        }
    }
}
