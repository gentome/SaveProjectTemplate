using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Gentome.Test
{
    public class Is : UnityEngine.TestTools.Constraints.Is
    {
        public static SameFolderConstaraint SameFolder(string expectDirPath)
        {
            return new SameFolderConstaraint(expectDirPath);
        }
    }

    public class SameFolderConstaraint : Constraint
    {
        private string _expectFolder;
        public SameFolderConstaraint(string expectFolder)
        {
            _expectFolder = expectFolder;
            Assert.True(Directory.Exists(expectFolder),$"Expect--{expectFolder}は存在しない");
        }
        public override ConstraintResult ApplyTo(object actual)
        {
            if (actual is not string)
            {
                return new ConstraintResult(this, actual, ConstraintStatus.Error);
            }
            string path = (string)actual;
            Assert.True(Directory.Exists(path),$"{path}フォルダは存在しない");
            DirectoryInfo actualInfo = new DirectoryInfo(path);
            DirectoryInfo expectInfo = new DirectoryInfo(_expectFolder);
            var actualfiles = actualInfo.GetFiles("*.*",SearchOption.AllDirectories);
            var expectFiles = expectInfo.GetFiles("*.*", SearchOption.AllDirectories);
            Assert.True(actualfiles.SequenceEqual(expectFiles, new FileCompare()),"一致していないファイルがあった");
            return new ConstraintResult(this, path, ConstraintStatus.Success);
        }
    }

    public class FileCompare : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return  x.Name == y.Name;
        }

        public int GetHashCode(FileInfo obj)
        {
            return HashCode.Combine(obj.Directory, obj.DirectoryName, obj.Exists, obj.IsReadOnly, obj.Length, obj.Name);
        }
    }
}