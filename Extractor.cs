using System.Collections.Generic;
using System.IO;

namespace Gentome
{
    public class Extractor
    {
        private readonly string ProjectDataPath;
        

        private string PackagesOutputPath => ProjectDataPath + Path.DirectorySeparatorChar + "Packages";

        private string PackagesSourcePath =>
            Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Packages";
        private string _assetsOutputPath;
        public string AssetsOutputPath => $"{ProjectDataPath}{Path.DirectorySeparatorChar}Assets";
        public string ProjectSettingsOutputPath => $"{ProjectDataPath}{Path.DirectorySeparatorChar}ProjectSettings";

        private string ProjectSettingsSourcePath =>
            $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}ProjectSettings";
        public List<string> AssetTagetDirs;
        public bool AllAssets;

        public Extractor(string projectDataPath)
        {
            if (!Directory.Exists(projectDataPath))
            {
                Directory.CreateDirectory(projectDataPath);
            }
            AssetTagetDirs = new List<string>();
            ProjectDataPath = projectDataPath;
        }
        public string ProjectSettingsExtract()
        {
            var info = Directory.CreateDirectory(ProjectSettingsOutputPath);
            DirExtend.CopyDirectory(ProjectSettingsSourcePath, info.FullName);
            File.Delete(ProjectSettingsOutputPath + Path.DirectorySeparatorChar + "ProjectVersion.txt");
            return ProjectSettingsOutputPath;

        }

        public string PackagesExtract()
        {
            var info = Directory.CreateDirectory(PackagesOutputPath);
            DirExtend.CopyDirectory(PackagesSourcePath,PackagesOutputPath);
            return PackagesOutputPath;
        }

        public string LibraryExtract()
        {
            string destLibraryPath = ProjectDataPath + Path.DirectorySeparatorChar + "Library";
            if (!Directory.Exists(destLibraryPath))
            {
                Directory.CreateDirectory(destLibraryPath);
            }

            destLibraryPath = destLibraryPath + Path.DirectorySeparatorChar;
            string sourceLibraryPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Library{Path.DirectorySeparatorChar}"; 
            File.Copy($"{sourceLibraryPath}ArtifactDB"
            ,destLibraryPath + "ArtifactDB",true );
            File.Copy($"{sourceLibraryPath}SourceAssetDB",
                destLibraryPath+"SourceAssetDB",true);
            string destArtifactPath = destLibraryPath + Path.DirectorySeparatorChar + "Artifacts";
            //todo 作ったらすぐにPackagesフォルダいかに移すので必要ないし、Packages以下でチェックした方が良いかもしれない
            if (Directory.Exists(destArtifactPath))
            {
                Directory.Delete(destArtifactPath,true);
            }
            DirExtend.CopyDirectory(sourceLibraryPath+"Artifacts",destArtifactPath);
            return destLibraryPath;
        }

        public string AssetsExtract()
        {

            if (string.IsNullOrEmpty(AssetsOutputPath))
            {
                return "";
            }
            var info = Directory.CreateDirectory(AssetsOutputPath);
            //todo 選択したフォルダだけ追加出来るようにする
            if (AllAssets)
            {
                AssetTagetDirs.Clear();
                AssetTagetDirs.Add(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Assets");
            }
            foreach (var assetTagetDir in AssetTagetDirs)
            {
                DirExtend.CopyDirectory(
                    assetTagetDir,
                    info.FullName
                );
            }
            return $"{info.FullName}";
        }

        public void RemoveCache()
        {
            //変数使うと最悪どんなフォルダ消すか分からないので直接書く
            if (Directory.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "gentomecache"))
            {
                Directory.Delete(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "gentomecache",true);
            }
        }
    }

    public static class DirExtend
    {
        //todo コピー処理が上手くいかない
        //      目的のフォルダを入れるとフォルダ自体が入らない
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirName);
            DirectoryInfo destinationDirectory = new DirectoryInfo(destDirName);
	
            //コピー先のディレクトリがなければ作成する
            if(destinationDirectory.Exists == false)
            {
                destinationDirectory.Create();
                destinationDirectory.Attributes = sourceDirectory.Attributes;
            }

            //ファイルのコピー
            foreach(FileInfo fileInfo in sourceDirectory.GetFiles()) 
            {
                //同じファイルが存在していたら、常に上書きする
                fileInfo.CopyTo(destinationDirectory.FullName + @"\" + fileInfo.Name, true);
            }

            //ディレクトリのコピー（再帰を使用）
            foreach(DirectoryInfo directoryInfo in sourceDirectory.GetDirectories())
            {
                CopyDirectory(directoryInfo.FullName, destinationDirectory.FullName + @"\" + directoryInfo.Name);
            }
        }

    }
}
