using System;
using UnityEditor;
using UnityEngine;

namespace Gentome
{
    
    public class ProjectTemplateWindow : EditorWindow
    {
        private string targetPath;
        private string templateName;
        private string templateDisplayName;
        private string templateDescription;
        private string templateDefaultScene;
        private string templateVersion;
        private string unity;
        private bool isAllAssets;
        private SceneAsset templateDefaultSceneAsset;
        private bool replaceTemplate;
        [MenuItem("Gentome/ProjectTemplate")]
        public static void ShowWindow()
        {
            ProjectTemplateWindow window = GetWindow<ProjectTemplateWindow>();
        }
        
        private void OnGUI()
        {
            
            isAllAssets = EditorGUILayout.Toggle("AllAsset", isAllAssets);
            templateName = EditorGUILayout.TextField("Name:", templateName);
            templateDisplayName = EditorGUILayout.TextField("Display name:", templateDisplayName);
            templateVersion = EditorGUILayout.TextField("Version:", templateVersion);
            unity = EditorGUILayout.TextField("Unity Version", unity);
            templateDescription = EditorGUILayout.TextField("Description:", templateDescription);
    
            if (GUILayout.Button("CreateTemplate"))
            {
                PackageFolderCreator packageFolderCreator = new PackageFolderCreator();
                Extractor extractor = new Extractor(packageFolderCreator.ProjectDataPath);
                extractor.AllAssets = isAllAssets;
                try
                {
                    PackageJsonCreator packageJsonCreator = new PackageJsonCreator(packageFolderCreator.PackagePath);
                    packageJsonCreator.Data = new TemplateData(
                        templateName,
                        version: templateVersion,
                        displayName: templateDisplayName,
                        defaultScene: templateDefaultScene,
                        unity: unity,
                        description: templateDescription
                        );
                    packageJsonCreator.CreatePackageFile();
                    extractor.AssetsExtract();
                    extractor.LibraryExtract();
                    extractor.PackagesExtract();
                    extractor.ProjectSettingsExtract();
            
                    // TarGz.TemplateName_Version = packageJsonCreator.TemplateName_Version;
                    // var task = TarGz.Archive(packageFolderCreator.CachePath);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                // extractor.RemoveCache();
            }
        }
    }
}
