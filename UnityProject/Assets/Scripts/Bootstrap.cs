using UnityEngine;
public static class Bootstrap {
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
 static void Start() {
  if(Object.FindObjectOfType<MainControl>() == null) {
   var camera = new GameObject("Background Camera").AddComponent<Camera>();
   camera.clearFlags=CameraClearFlags.SolidColor; camera.backgroundColor=Color.black;
   new GameObject("LIWA Floor Game").AddComponent<MainControl>();
  }
 }
}