using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Gentome.Test
{


    public class ProjectTemplateTest
    {
        private string tgzTestDirPath = Directory.GetCurrentDirectory() +
                                        Path.DirectorySeparatorChar + "Assets";

        private string CachePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "gentomecache";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Directory.CreateDirectory(CachePath);
        }
        [Test]
        public void packagesファイルをtargzして規則にしたがってフォルダを命名する()
        {
            var testDir = Directory.CreateDirectory(tgzTestDirPath);
            var  outputPath = TarGz.Archive(testDir.FullName);
           
            Assert.That(File.Exists(outputPath),$"ファイルを生成できていない");
        }

        [Test]
        public void packagesフォルダを作る()
        {
            Extractor extractor = new Extractor(CachePath);
            string packagesPath = extractor.PackagesExtract();
            Assert.That(packagesPath,Is.SameFolder(Directory.GetCurrentDirectory()+Path.DirectorySeparatorChar+"Packages"));
        }
        [Test]
        public void packageJsonファイルを作る()
        {
            PackageJsonCreator packageJsonCreator = new PackageJsonCreator(CachePath);
            packageJsonCreator.Data = new TemplateData();
            var file = packageJsonCreator.CreatePackageFile();
            var data = file.JsonFromFile<TemplateData>();
            Assert.That(data,NUnit.Framework.Is.EqualTo(packageJsonCreator.Data));
        }

        [Test]
        public void LibraryからArtifacsをとる()
        {
            Extractor extractor = new Extractor(CachePath);
            string path = extractor.LibraryExtract();

            Assert.That(File.Exists(path + Path.DirectorySeparatorChar + "ArtifactDB"), $"ArtifactDBファイルが抽出できてない");
            Assert.That(File.Exists(path + Path.DirectorySeparatorChar + "SourceAssetDB"),
                $"SourceassetDBファイルが抽出できてない");
            var artifactDir = Directory.GetDirectories(path).First();
            string expectArtifactPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Library" +
                                        Path.DirectorySeparatorChar + "Artifacts";
            Assert.That(path + Path.DirectorySeparatorChar + "Artifacts", Is.SameFolder(expectArtifactPath));

        }

        [Test]
        public void ProjectSettingsを抽出してディレクトリに入れる()
        {
            Extractor extractor = new Extractor(CachePath);
            string projectsettingsFolder = extractor.ProjectSettingsExtract();
            Assert.That(projectsettingsFolder
                ,Is.SameFolder(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "ProjectSettings"));
        }

        [Test]
        public void ProjectVersionTextは入れない()
        {
            Extractor extractor = new Extractor(CachePath);
            string outputpath = extractor.ProjectSettingsExtract();
            Assert.False(File.Exists(outputpath+Path.DirectorySeparatorChar+"ProjectVersion.txt"));
        }

        [Test]
        public void Assets以下から欲しいファイルを抜き出して新しいディレクトリに入れる()
        {
            Extractor extractor = new Extractor(CachePath);
            //todo ここのパスをCacheとかにして終わったら消えるようにする
            extractor.AssetTagetDirs.Add(
                $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Assets{Path.DirectorySeparatorChar}Scenes");

            string resultPath = extractor.AssetsExtract();
            var dirs = Directory.GetDirectories(resultPath);
            Assert.That(resultPath, Is.SameFolder(extractor.AssetTagetDirs.First()));
        }

        [Test]
        public void Assets以下を全ていれる設定が出来る()
        {
            Extractor extractor = new Extractor(CachePath);
            extractor.AllAssets = true;
            string resulatPath = extractor.AssetsExtract();
            var dirs = Directory.GetDirectories(resulatPath);
            //全部確認するの大変だからざつに
            Assert.That(dirs.Length,NUnit.Framework.Is.GreaterThan(1));

        }


        [Test]
        [Category("EndToEnd")]
        public void packageフォルダにデータを入れる()
        {
            PackageFolderCreator creator = new PackageFolderCreator();
            PackageJsonCreator packageJsonCreator = new PackageJsonCreator(creator.PackagePath);
            Extractor extractor = new Extractor(creator.ProjectDataPath);
            packageJsonCreator.Data = new TemplateData();
            packageJsonCreator.CreatePackageFile();
            extractor.AssetsExtract();
            extractor.LibraryExtract();
            extractor.PackagesExtract();
            extractor.ProjectSettingsExtract();
            
            TarGz.TemplateName_Version = packageJsonCreator.TemplateName_Version;
            TarGz.Archive(creator.CachePath);
          
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            string GentomeTestDir = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "GentomeTest";
            if (Directory.Exists(GentomeTestDir))
            {
                Directory.Delete(GentomeTestDir,true);
            }


            if (Directory.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "gentomecache"))
            {
                Directory.Delete(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "gentomecache",true);
            }
        }
    }
    public static class Extend{

        public static T JsonFromFile<T>(this string filepath)
        {
            T json;
            using (var reader = new StreamReader(filepath))
            {
                string text = reader.ReadToEnd();
                json = JsonUtility.FromJson<T>(text);
            }
            return json;
        }
    }
}