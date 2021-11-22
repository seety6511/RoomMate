using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DinoFracture.Editor
{
    [CustomEditor(typeof(PreFracturedGeometry))]
    [CanEditMultipleObjects()]
    public class PreFracturedGeometryEditor : UnityEditor.Editor
    {
        private bool _waitForClick = false;

        private GUIStyle _centerStyle;
        private GUIStyle _buttonStyle;

        private PreFracturedGeometryEditorFractureProgress _progress;
        private static PreFracturedGeometryEditorFractureData _fractureData;

        public override void OnInspectorGUI()
        {
            EnsureProgressData();

            base.OnInspectorGUI();

            if (!IsRunningFractures())
            {
                if (GUILayout.Button("Create Fractures"))
                {
                    CreateFractureData();
                    GenerateFractures();
                }

                EditorUtility.ClearProgressBar();
            }
            else
            {
                _progress.DisplayGui(_fractureData);
            }

            if (_waitForClick)
            {
                if (_buttonStyle == null)
                {
                    _buttonStyle = new GUIStyle(GUI.skin.button);
                    _buttonStyle.normal.textColor = Color.white;
                }

                Color color = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Click on the Object", _buttonStyle))
                {
                    _waitForClick = false;
                }
                GUI.backgroundColor = color;
            }
            else
            {
                if (!IsRunningFractures())
                {
                    if (GUILayout.Button("Create Fractures at Point"))
                    {
                        CreateFractureData();
                        _waitForClick = true;
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Crumble"))
                {
                    CreateFractureData();
                    GenerateFractures();
                }
            }
        }

        private void EnsureProgressData()
        {
            if (_progress == null)
            {
                _progress = new PreFracturedGeometryEditorFractureProgress();
                _progress.OnCanceled += StopRunningFractures;
            }
        }

        private void CreateFractureData()
        {
            _fractureData = new PreFracturedGeometryEditorFractureData();

            foreach (PreFracturedGeometry geom in targets)
            {
                _fractureData.GeomList.Add(geom);
            }

            _fractureData.FinalizeList();
        }

        private void ClearFractureData()
        {
            _fractureData = null;
        }

        private bool IsRunningFractures()
        {
            if (_fractureData == null)
            {
                return false;
            }

            foreach (PreFracturedGeometry geom in _fractureData.GeomList)
            {
                if (geom.RunningFracture != null)
                {
                    return true;
                }
            }

            return false;
        }

        private void GenerateFractures(Vector3 localPoint = default)
        {
            if (Application.isPlaying)
            {
                foreach (PreFracturedGeometry geom in _fractureData.GeomList)
                {
                    geom.Fracture();
                }
            }
            else
            {
                _progress.OnFracturesStarted();

                foreach (PreFracturedGeometry geom in _fractureData.GeomList)
                {
                    geom.GenerateFractureMeshes(localPoint, (g) => { SaveToDisk(_fractureData); });
                }
            }
        }

        private void StopRunningFractures()
        {
            if (_fractureData != null)
            {
                foreach (PreFracturedGeometry geom in _fractureData.GeomList)
                {
                    geom.StopRunningFracture();
                }

                ClearFractureData();
            }
        }

        private void OnSceneGUI()
        {
            if (_waitForClick)
            {
                Vector2 mousePos = Event.current.mousePosition;

                if (_centerStyle == null)
                {
                    _centerStyle = new GUIStyle(GUI.skin.label);
                    _centerStyle.alignment = TextAnchor.UpperCenter;
                    _centerStyle.normal.textColor = Color.white;
                    _centerStyle.active.textColor = Color.white;
                    _centerStyle.hover.textColor = Color.white;
                }

                Handles.BeginGUI();
                GUI.Label(new Rect(mousePos.x - 80.0f, mousePos.y - 45.0f, 160.0f, 17.0f),
                    "Click on the object to", _centerStyle);
                GUI.Label(new Rect(mousePos.x - 80.0f, mousePos.y - 28.0f, 160.0f, 17.0f),
                    "create the fracture pieces.", _centerStyle);
                Handles.EndGUI();

                if (Event.current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(0);
                }

                foreach (PreFracturedGeometry geom in _fractureData.GeomList)
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        _waitForClick = false;
                        Collider collider = geom.GetComponent<Collider>();
                        if (collider != null)
                        {
                            RaycastHit hit;
                            if (collider.Raycast(ray, out hit, 1000000000.0f))
                            {
                                Vector3 localPoint = geom.transform.worldToLocalMatrix.MultiplyPoint(hit.point);

                                GenerateFractures(localPoint);

                                break;
                            }
                        }
                    }
                    else if (Event.current.type == EventType.MouseMove)
                    {
                        SceneView.RepaintAll();
                    }
                }
            }
        }

        private void SaveToDisk(PreFracturedGeometryEditorFractureData data)
        {
            bool complete = data.OnComplete();

            _progress.OnFractureComplete(data);

            if (!complete)
            {
                return;
            }
            else
            {
                _progress.Hide();
            }

            int choice = EditorUtility.DisplayDialogComplex("Save fractured meshes to disk?",
                "Would you like to save the fractured meshes to disk?  This is necessary to be part of a prefab.",
                "Clear Folder and Save", "Save, no Clear", "Don't Save");

            if (choice < 2)
            {
                string folder = EditorUtility.SaveFolderPanel("Asset Folder", "Assets", "FracturedMeshes");
                if (!String.IsNullOrEmpty(folder) && folder.StartsWith(Application.dataPath))
                {
                    // Delete all the assets first in case multiple objects have the same name
                    if (choice == 0)
                    {
                        foreach (PreFracturedGeometry geom in data.GeomList)
                        {
                            string saveFolder = Path.Combine(folder, geom.gameObject.name);

                            // Delete the contents of the folder
                            DirectoryInfo dir = new DirectoryInfo(saveFolder);
                            if (dir.Exists)
                            {
                                foreach (FileInfo file in dir.GetFiles())
                                {
                                    string baseName = "";
                                    if (file.Name.EndsWith(".asset"))
                                    {
                                        baseName = file.Name.Substring(0, file.Name.Length - ".asset".Length);
                                    }
                                    if (!String.IsNullOrEmpty(baseName))
                                    {
                                        try
                                        {
                                            new Guid(baseName);

                                            // Guid resolves, delete the file since it is probably one we created.
                                            AssetDatabase.DeleteAsset("Assets" +
                                                                      file.FullName.Substring(Application.dataPath.Length));
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (PreFracturedGeometry geom in data.GeomList)
                    {
                        string saveFolder = Path.Combine(folder, geom.gameObject.name);

                        // Make sure the directory exists
                        {
                            DirectoryInfo dir = new DirectoryInfo(saveFolder);
                            if (!dir.Exists)
                            {
                                dir.Create();
                            }
                        }

                        // Create a new asset for each mesh
                        saveFolder = saveFolder.Substring(Application.dataPath.Length + 1);
                        for (int i = 0; i < geom.GeneratedPieces.transform.childCount; i++)
                        {
                            MeshFilter mf = geom.GeneratedPieces.transform.GetChild(i).GetComponent<MeshFilter>();
                            if (mf != null && mf.sharedMesh != null)
                            {
                                string assetPath = Path.Combine("Assets", saveFolder, String.Format("{0}.asset", Guid.NewGuid().ToString("B")));
                                assetPath = assetPath.Replace('\\', '/');

                                AssetDatabase.CreateAsset(mf.sharedMesh, assetPath);
                            }
                        }

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            ClearFractureData();
        }
    }

    class PreFracturedGeometryEditorFractureData
    {
        public readonly List<PreFracturedGeometry> GeomList = new List<PreFracturedGeometry>();

        private int _countLeft;

        public int ActiveCount
        {
            get { return _countLeft; }
        }

        public void FinalizeList()
        {
            _countLeft = GeomList.Count;
        }

        public bool OnComplete()
        {
            return System.Threading.Interlocked.Decrement(ref _countLeft) == 0;
        }
    }

    class PreFracturedGeometryEditorFractureProgress
    {
        public event Action OnCanceled;

#if UNITY_2020_1_OR_NEWER
        public int _progressId;
#endif

        private void Cancel()
        {
            if (OnCanceled != null)
            {
                OnCanceled();
            }

            Hide();
        }

        public void DisplayGui(PreFracturedGeometryEditorFractureData data)
        {
#if UNITY_2020_1_OR_NEWER
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("Stop Fracturing"))
            {
                Cancel();
            }
            GUI.backgroundColor = color;
#else
            int totalCount = data.GeomList.Count;
            int completedCount = totalCount - data.ActiveCount;
            if (EditorUtility.DisplayCancelableProgressBar("Fracturing Objects", String.Format("Completed ({0} / {1})", completedCount, totalCount), (float)completedCount / (float)totalCount))
            {
                Cancel();
            }
#endif
        }

        public void OnFracturesStarted()
        {
#if UNITY_2020_1_OR_NEWER
            _progressId = Progress.Start("Fracturing Objects", null, Progress.Options.Synchronous);
#endif
        }

        public void OnFractureComplete(PreFracturedGeometryEditorFractureData data)
        {
#if UNITY_2020_1_OR_NEWER
            if (data != null)
            {
                int totalCount = data.GeomList.Count;
                int completedCount = totalCount - data.ActiveCount;
                Progress.Report(_progressId, completedCount, totalCount, String.Format("Completed ({0} / {1})", completedCount, totalCount));
            }
#endif
        }

        public void Hide()
        {
#if UNITY_2020_1_OR_NEWER
            Progress.Remove(_progressId);
#endif
        }
    }
}
