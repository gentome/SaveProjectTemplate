using System.IO;
using System.Text;
using SharpCompress.Common;
using SharpCompress.Writers;
using UnityEngine;
using CompressionType = SharpCompress.Common.CompressionType;

namespace Gentome
{
    public class TarGz : MonoBehaviour
    {
        public static string TemplateName_Version;

        public static string Archive(string packageDirPath)
        {
            if (!Directory.Exists(packageDirPath))
            {
                return "NON_DIRECTORY";
            }
            if (string.IsNullOrEmpty(TemplateName_Version))
            {
                TemplateName_Version = "XXXXXX";//note 変な名前にならないようにするだけ、未設定なら動かない
            }
            string outputPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + TemplateName_Version +
                                ".tgz";
            GZip(outputPath,packageDirPath);
            return outputPath;
        }

        public static void GZip(string outputTarFilePath, string sourceDirectory)
        {

            //todo 圧縮すると文字化けする
            //todo なぜかUnityハブで開けない
            ArchiveEncoding encoding = new ArchiveEncoding(Encoding.UTF8, Encoding.UTF8);
            using(var stream = File.OpenWrite(outputTarFilePath))
            using (var writer = WriterFactory.Open(stream, ArchiveType.Tar,
                       new WriterOptions(CompressionType.GZip){LeaveStreamOpen = true,ArchiveEncoding = encoding}))
            {
                
                writer.WriteAll(sourceDirectory,"*",SearchOption.AllDirectories);
            }
            
        }


    }
}
