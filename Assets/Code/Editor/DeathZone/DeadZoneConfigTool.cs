using Code.StaticData.Input;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.DeathZone
{
    public class DeadZoneConfigTool : EditorWindow
    {
        private DeadZoneStaticData _deadZoneConfig;
        private Vector2 _scrollPosition;
        private bool _showPreview = true;
        private Color _workingAreaColor = new Color(0f, 1f, 0f, 0.3f);
        private Color _deadZoneColor = new Color(1f, 0f, 0f, 0.3f);
        private Vector2 _simulatedScreenSize = new Vector2(1920, 1080);
        private Vector2 _previewSize = new Vector2(400, 225);
        
        [MenuItem("Tools/Dead Zone Config")]
        public static void ShowWindow()
        {
            DeadZoneConfigTool window = GetWindow<DeadZoneConfigTool>();
            window.titleContent = new GUIContent("Dead Zone Config");
            window.minSize = new Vector2(450, 500);
            window.Show();
        }
        
        private void OnEnable()
        {
            LoadConfig();
        }
        
        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            DrawHeader();
            DrawConfigSelection();
            
            if (_deadZoneConfig != null)
            {
                DrawSliders();
                DrawPreview();
                DrawActions();
            }
            else
            {
                DrawCreateConfigButton();
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Dead Zone Configuration Tool", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            EditorGUILayout.HelpBox(
                "Configure the working area where input is allowed. " +
                "Everything outside this area will be a dead zone where input is ignored.",
                MessageType.Info);
            EditorGUILayout.Space();
        }
        
        private void DrawConfigSelection()
        {
            EditorGUILayout.LabelField("Configuration Asset", EditorStyles.boldLabel);
            
            DeadZoneStaticData newConfig = (DeadZoneStaticData)EditorGUILayout.ObjectField(
                "Dead Zone Config", 
                _deadZoneConfig, 
                typeof(DeadZoneStaticData), 
                false);
                
            if (newConfig != _deadZoneConfig)
            {
                _deadZoneConfig = newConfig;
                SaveConfig();
            }
            
            EditorGUILayout.Space();
        }
        
        private void DrawSliders()
        {
            EditorGUILayout.LabelField("Working Area Boundaries", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();
            
            _deadZoneConfig.WorkingAreaLeft = EditorGUILayout.Slider(
                "Left Boundary", 
                _deadZoneConfig.WorkingAreaLeft, 
                0f, 
                _deadZoneConfig.WorkingAreaRight - 0.01f);
            
            _deadZoneConfig.WorkingAreaRight = EditorGUILayout.Slider(
                "Right Boundary", 
                _deadZoneConfig.WorkingAreaRight, 
                _deadZoneConfig.WorkingAreaLeft + 0.01f, 
                1f);
            
            _deadZoneConfig.WorkingAreaBottom = EditorGUILayout.Slider(
                "Bottom Boundary", 
                _deadZoneConfig.WorkingAreaBottom, 
                0f, 
                _deadZoneConfig.WorkingAreaTop - 0.01f);
            
            _deadZoneConfig.WorkingAreaTop = EditorGUILayout.Slider(
                "Top Boundary", 
                _deadZoneConfig.WorkingAreaTop, 
                _deadZoneConfig.WorkingAreaBottom + 0.01f, 
                1f);
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_deadZoneConfig);
                Repaint();
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Area Information", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            float workingWidth = _deadZoneConfig.WorkingAreaRight - _deadZoneConfig.WorkingAreaLeft;
            float workingHeight = _deadZoneConfig.WorkingAreaTop - _deadZoneConfig.WorkingAreaBottom;
            
            EditorGUILayout.LabelField($"Working Area: {workingWidth:P1} × {workingHeight:P1}");
            EditorGUILayout.LabelField($"Dead Zone Top: {(1f - _deadZoneConfig.WorkingAreaTop):P1}");
            EditorGUILayout.LabelField($"Dead Zone Bottom: {_deadZoneConfig.WorkingAreaBottom:P1}");
            EditorGUILayout.LabelField($"Dead Zone Left: {_deadZoneConfig.WorkingAreaLeft:P1}");
            EditorGUILayout.LabelField($"Dead Zone Right: {(1f - _deadZoneConfig.WorkingAreaRight):P1}");
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }
        
        private void DrawPreview()
        {
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
            
            _showPreview = EditorGUILayout.Toggle("Show Preview", _showPreview);
            
            if (_showPreview)
            {
                _simulatedScreenSize = EditorGUILayout.Vector2Field("Simulated Screen Size", _simulatedScreenSize);
                _workingAreaColor = EditorGUILayout.ColorField("Working Area Color", _workingAreaColor);
                _deadZoneColor = EditorGUILayout.ColorField("Dead Zone Color", _deadZoneColor);
                
                EditorGUILayout.Space();
                
                Rect previewRect = GUILayoutUtility.GetRect(_previewSize.x, _previewSize.y);
                EditorGUI.DrawRect(previewRect, _deadZoneColor);
                
                Rect workingRect = new Rect(
                    previewRect.x + previewRect.width * _deadZoneConfig.WorkingAreaLeft,
                    previewRect.y + previewRect.height * (1f - _deadZoneConfig.WorkingAreaTop),
                    previewRect.width * (_deadZoneConfig.WorkingAreaRight - _deadZoneConfig.WorkingAreaLeft),
                    previewRect.height * (_deadZoneConfig.WorkingAreaTop - _deadZoneConfig.WorkingAreaBottom)
                );
                
                EditorGUI.DrawRect(workingRect, _workingAreaColor);
                GUI.Box(previewRect, "", EditorStyles.helpBox);
                
                GUI.Label(new Rect(previewRect.x + 5, previewRect.y + 5, 100, 20), "Dead Zone", EditorStyles.miniLabel);
                GUI.Label(new Rect(workingRect.x + 5, workingRect.y + 5, 100, 20), "Working Area", EditorStyles.miniLabel);
            }
            
            EditorGUILayout.Space();
        }
        
        private void DrawActions()
        {
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Reset to Default"))
            {
                _deadZoneConfig.WorkingAreaLeft = 0f;
                _deadZoneConfig.WorkingAreaRight = 1f;
                _deadZoneConfig.WorkingAreaBottom = 0f;
                _deadZoneConfig.WorkingAreaTop = 0.85f;
                EditorUtility.SetDirty(_deadZoneConfig);
            }
            
            if (GUILayout.Button("Common: Top UI"))
            {
                _deadZoneConfig.WorkingAreaLeft = 0f;
                _deadZoneConfig.WorkingAreaRight = 1f;
                _deadZoneConfig.WorkingAreaBottom = 0f;
                _deadZoneConfig.WorkingAreaTop = 0.85f;
                EditorUtility.SetDirty(_deadZoneConfig);
            }
            
            if (GUILayout.Button("Common: Full UI"))
            {
                _deadZoneConfig.WorkingAreaLeft = 0f;
                _deadZoneConfig.WorkingAreaRight = 1f;
                _deadZoneConfig.WorkingAreaBottom = 0.1f;
                _deadZoneConfig.WorkingAreaTop = 0.85f;
                EditorUtility.SetDirty(_deadZoneConfig);
            }
            
            EditorGUILayout.EndHorizontal();
            
            if (GUILayout.Button("Save Asset"))
            {
                AssetDatabase.SaveAssets();
                EditorGUILayout.HelpBox("Asset saved!", MessageType.Info);
            }
        }
        
        private void DrawCreateConfigButton()
        {
            EditorGUILayout.HelpBox("No Dead Zone Config selected. Create or select one.", MessageType.Warning);
            
            if (GUILayout.Button("Create New Dead Zone Config"))
            {
                CreateNewConfig();
            }
        }
        
        private void CreateNewConfig()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Create Dead Zone Config",
                "DeadZoneConfig",
                "asset",
                "Choose location for Dead Zone Config");
                
            if (!string.IsNullOrEmpty(path))
            {
                DeadZoneStaticData newConfig = CreateInstance<DeadZoneStaticData>();
                AssetDatabase.CreateAsset(newConfig, path);
                AssetDatabase.SaveAssets();
                
                _deadZoneConfig = newConfig;
                SaveConfig();
                
                EditorGUIUtility.PingObject(newConfig);
            }
        }
        
        private void LoadConfig()
        {
            string configGuid = EditorPrefs.GetString("DeadZoneConfigTool_LastConfig", "");
            if (!string.IsNullOrEmpty(configGuid))
            {
                string path = AssetDatabase.GUIDToAssetPath(configGuid);
                _deadZoneConfig = AssetDatabase.LoadAssetAtPath<DeadZoneStaticData>(path);
            }
        }
        
        private void SaveConfig()
        {
            if (_deadZoneConfig != null)
            {
                string path = AssetDatabase.GetAssetPath(_deadZoneConfig);
                string guid = AssetDatabase.AssetPathToGUID(path);
                EditorPrefs.SetString("DeadZoneConfigTool_LastConfig", guid);
            }
        }
    }
}
