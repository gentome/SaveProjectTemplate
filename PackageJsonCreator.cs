using System;
using System.IO;
using UnityEngine;

namespace Gentome
{
    public class PackageJsonCreator
    {
        private readonly string PackagePath;
        public PackageJsonCreator(string packagePath)
        {
            Data = new TemplateData();
            PackagePath = packagePath;
        }
        public string CreatePackageFile()
        {
            string packagejsonFilepathPath = $"{PackagePath}{Path.DirectorySeparatorChar}package.json";
            using (var stream = File.Create(packagejsonFilepathPath))
            {
                using (var writer = new StreamWriter(stream))
                {
                    string dataJsonText = JsonUtility.ToJson(Data);
                    writer.Write(dataJsonText);
                }
            }
            return packagejsonFilepathPath;
        }

        public TemplateData Data;

        public string TemplateName_Version
        {
            get
            {
                if (string.IsNullOrEmpty(Data.Name) || string.IsNullOrEmpty(Data.Version))
                {
                    return "";
                }
                return $"{Data.Name}-{Data.Version}";
            }
        }
    }

    [Serializable]
    public class TemplateData : IEquatable<TemplateData>
    {
        //todo キーの値がいろいろ変わってるので見本のプロジェクト見ながら直す
        [SerializeField]
        private string name;
        [SerializeField]
        private string displayName;
        [SerializeField]
        private string description;
        [SerializeField]
        private string defaultScene;
        [SerializeField]
        private string version;

        [SerializeField] private string unity;
        private readonly string type = "template";
        public string Name => name;
        public string DisplayName => displayName;
        public string Description => description;
        public string DefaultScene => defaultScene;
        public string Version => version;

        public TemplateData(string name = "",string displayName = "",string defaultScene = "",string version = "",
            string unity = "",string description = "")
        {
            this.name = name;
            this.displayName = displayName;
            this.defaultScene = defaultScene;
            this.version = version;
            this.unity = unity;
            this.description = description;
        }
        public bool Equals(TemplateData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return name == other.name && displayName == other.displayName && description == other.description && defaultScene == other.defaultScene && version == other.version;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TemplateData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, displayName, description, defaultScene, version);
        }
    }
}
