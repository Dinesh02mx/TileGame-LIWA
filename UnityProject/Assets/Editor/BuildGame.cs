using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine;
public static class BuildGame {
 [MenuItem("LIWA/Create Boot Scene")]
 public static void CreateScene() {
  Directory.CreateDirectory("Assets/Scenes");
  var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
  EditorSceneManager.SaveScene(scene,"Assets/Scenes/Boot.unity");
  EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/Boot.unity",true)};
  AssetDatabase.Refresh();
 }
 [MenuItem("LIWA/Build Windows Player")]
 public static void Build() {
  CreateScene();
  string root=Path.GetFullPath(Path.Combine(Application.dataPath,"../.."));
  string destination=Path.Combine(root,"Builds/RebuiltWindows");
  Directory.CreateDirectory(destination);
  var report=BuildPipeline.BuildPlayer(new[]{"Assets/Scenes/Boot.unity"},Path.Combine(destination,"Light.exe"),BuildTarget.StandaloneWindows64,BuildOptions.None);
  if(report.summary.result!=BuildResult.Succeeded) throw new Exception("Unity build failed: "+report.summary.result);
  File.Copy(Path.Combine(root,"Config/Simulation.json"),Path.Combine(destination,"FloorConfig.json"),true);
  File.Copy(Path.Combine(root,"Builds/Windows/TileMap33x12.csv"),Path.Combine(destination,"TileMap33x12.csv"),true);
 }
}