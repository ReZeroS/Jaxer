using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ReZeroS.Jaxer.Plugins
{
    public class ScriptableObjectManager : EditorWindow
    {
        // 布局相关变量
        private float navAreaWidth = 200f;
        private float executionAreaHeight = 200f;
        private float filterAreaWidth = 300f;
        private bool resizingNavArea = false;
        private bool resizingExecutionArea = false;
        private bool resizingFilterArea = false;

        // 导航区相关变量
        private Vector2 navScrollPos;
        private Type[] soTypes;
        private Type selectedType;

        // 工作区相关变量
        private Vector2 workScrollPos;
        private List<ScriptableObject> instances;
        private List<ScriptableObject> filteredInstances;
        private Dictionary<ScriptableObject, Editor> cachedEditors = new Dictionary<ScriptableObject, Editor>();

        // 工作区布局
        private int columnsPerRow = 3;
        private float columnWidth = 300f;
        private float rowSpacing = 10f;

        // 执行区相关变量
        private Vector2 executionFilterScrollPos;
        private Vector2 executionUpdateScrollPos;
        private Dictionary<string, bool> fieldToggles = new Dictionary<string, bool>();
        private Dictionary<string, string> fieldFilters = new Dictionary<string, string>();

        // 虚拟SO相关变量
        private ScriptableObject virtualSO;
        private Editor virtualSOEditor;
        private bool isDragging = false;
        private object draggedValue;
        private string draggedFieldName;

        [MenuItem("Tools/Scriptable Object Manager V2")]
        public static void OpenWindow()
        {
            GetWindow<ScriptableObjectManager>("SO Manager");
        }

        private void OnEnable()
        {
            soTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ScriptableObject))
                            && !t.IsAbstract
                            && t.GetCustomAttributes(typeof(CreateAssetMenuAttribute), false).Length > 0)
                .ToArray();
        }

        private void OnGUI()
        {
            DrawLayout();
        }

        private void DrawLayout()
        {
            Rect totalRect = position;

            // 导航区
            Rect navRect = new Rect(0, 0, navAreaWidth, totalRect.height - executionAreaHeight);
            GUILayout.BeginArea(navRect, EditorStyles.helpBox);
            DrawNavigationArea();
            GUILayout.EndArea();

            // 导航区调整手柄
            Rect navResizeRect = new Rect(navAreaWidth - 5, 0, 10, navRect.height);
            EditorGUIUtility.AddCursorRect(navResizeRect, MouseCursor.ResizeHorizontal);
            HandleNavAreaResize(navResizeRect);

            // 工作区
            Rect workspaceRect = new Rect(navAreaWidth, 0, totalRect.width - navAreaWidth,
                totalRect.height - executionAreaHeight);
            GUILayout.BeginArea(workspaceRect);
            DrawWorkspace();
            GUILayout.EndArea();

            // 执行区
            Rect executionRect = new Rect(0, totalRect.height - executionAreaHeight, totalRect.width,
                executionAreaHeight);
            GUILayout.BeginArea(executionRect, EditorStyles.helpBox);
            DrawExecutionArea();
            GUILayout.EndArea();

            // 执行区调整手柄
            Rect executionResizeRect = new Rect(0, totalRect.height - executionAreaHeight - 5, totalRect.width, 10);
            EditorGUIUtility.AddCursorRect(executionResizeRect, MouseCursor.ResizeVertical);
            // HandleExecutionAreaResize(executionResizeRect);
        }

        private void DrawNavigationArea()
        {
            EditorGUILayout.LabelField("Scriptable Object Types", EditorStyles.boldLabel);
            navScrollPos = EditorGUILayout.BeginScrollView(navScrollPos);

            foreach (var type in soTypes)
            {
                bool isSelected = selectedType == type;
                GUI.backgroundColor = isSelected ? Color.cyan : Color.white;

                if (GUILayout.Button(type.Name, EditorStyles.toolbarButton))
                {
                    selectedType = type;
                    LoadInstances();
                    CreateVirtualSO();
                }

                GUI.backgroundColor = Color.white;
            }

            EditorGUILayout.EndScrollView();
        }

        private void LoadInstances()
        {
            instances = AssetDatabase.FindAssets($"t:{selectedType.Name}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();

            filteredInstances = new List<ScriptableObject>(instances);

            // 初始化字段筛选器
            fieldToggles.Clear();
            fieldFilters.Clear();

            foreach (var field in selectedType.GetFields())
            {
                fieldToggles[field.Name] = true;
                fieldFilters[field.Name] = "";
            }
        }

        private void CreateVirtualSO()
        {
            if (selectedType == null) return;

            // 创建虚拟SO实例
            virtualSO = CreateInstance(selectedType);

            // 如果有筛选后的实例，使用第一个实例的值初始化虚拟SO
            if (filteredInstances != null && filteredInstances.Count > 0)
            {
                var sourceInstance = filteredInstances[0];
                foreach (var field in selectedType.GetFields())
                {
                    field.SetValue(virtualSO, field.GetValue(sourceInstance));
                }
            }

            virtualSOEditor = Editor.CreateEditor(virtualSO);
        }

        private void HandleDragAndDrop()
        {
            Event evt = Event.current;

            switch (evt.type)
            {
                case EventType.MouseDown when evt.button == 0:
                    if (selectedType != null && filteredInstances != null)
                    {
                        draggedFieldName = GetFieldUnderMouse(evt.mousePosition);
                        if (!string.IsNullOrEmpty(draggedFieldName))
                        {
                            isDragging = true;
                            evt.Use();
                        }
                    }

                    break;

                case EventType.MouseDrag when isDragging:
                    DragAndDrop.PrepareStartDrag();
                    evt.Use();
                    break;

                case EventType.MouseUp:
                    if (isDragging)
                    {
                        ApplyDraggedValueToVirtualSO();
                        isDragging = false;
                        DragAndDrop.AcceptDrag();
                        evt.Use();
                    }
                    else
                    {
                        isDragging = false; // 确保鼠标抬起时清理状态
                    }

                    break;
            }
        }

        private string GetFieldUnderMouse(Vector2 mousePosition)
        {
            if (filteredInstances == null || filteredInstances.Count == 0) return null;

            foreach (var instance in filteredInstances)
            {
                var fields = selectedType.GetFields();
                foreach (var field in fields)
                {
                    // 计算字段在界面上的矩形区域
                    // 这里需要根据你的具体布局来计算
                    // 如果鼠标在字段区域内，返回字段名
                    if (fieldToggles.ContainsKey(field.Name) && fieldToggles[field.Name])
                    {
                        return field.Name;
                    }
                }
            }

            return null;
        }

        private void ApplyDraggedValueToVirtualSO()
        {
            if (virtualSO == null || string.IsNullOrEmpty(draggedFieldName)) return;

            var field = selectedType.GetField(draggedFieldName);
            if (field != null)
            {
                try
                {
                    field.SetValue(virtualSO, draggedValue);
                    EditorUtility.SetDirty(virtualSO);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to set value: {e.Message}");
                }
            }
        }

        private void DrawWorkspace()
        {
            if (selectedType == null || filteredInstances == null) return;

            workScrollPos = EditorGUILayout.BeginScrollView(workScrollPos);

            int currentColumn = 0;
            EditorGUILayout.BeginHorizontal();

            foreach (var instance in filteredInstances)
            {
                if (!cachedEditors.ContainsKey(instance))
                {
                    cachedEditors[instance] = Editor.CreateEditor(instance);
                }

                EditorGUILayout.BeginVertical("box", GUILayout.Width(columnWidth));

                // 实例标题
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(instance.name, EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                // 绘制过滤后的Inspector
                DrawFilteredInspector(cachedEditors[instance]);

                EditorGUILayout.EndVertical();

                currentColumn++;
                if (currentColumn >= columnsPerRow)
                {
                    EditorGUILayout.EndHorizontal();
                    DrawHorizontalLine();
                    EditorGUILayout.BeginHorizontal();
                    currentColumn = 0;
                }
            }

            if (currentColumn > 0)
            {
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawFilteredInspector(Editor editor)
        {
            EditorGUI.BeginChangeCheck();

            // 获取序列化对象
            SerializedObject serializedObject = editor.serializedObject;
            SerializedProperty iterator = serializedObject.GetIterator();

            // 跳过脚本字段
            iterator.NextVisible(true);

            while (iterator.NextVisible(false))
            {
                // 检查是否勾选了该字段的显示
                if (!fieldToggles.ContainsKey(iterator.name) || !fieldToggles[iterator.name])
                    continue;

                // 绘制字段
                EditorGUILayout.PropertyField(iterator, true);
            }

            // 应用修改
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawExecutionArea()
        {
            if (selectedType == null) return;

            EditorGUILayout.BeginHorizontal();
            // 筛选区
            executionFilterScrollPos = EditorGUILayout.BeginScrollView(executionFilterScrollPos);
            EditorGUILayout.BeginVertical("box", GUILayout.Width(filterAreaWidth));
            DrawFilterArea();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            

            // 分隔线和调整手柄
            Rect filterResizeRect = GUILayoutUtility.GetRect(5, 0);
            EditorGUIUtility.AddCursorRect(filterResizeRect, MouseCursor.ResizeHorizontal);
         
            // 更新区（虚拟SO Inspector）
            executionUpdateScrollPos = EditorGUILayout.BeginScrollView(executionUpdateScrollPos);
            EditorGUILayout.BeginVertical("box");
            DrawVirtualInspector();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndHorizontal();

            // 按钮区
            DrawButtons();
        }

        private void DrawVirtualInspector()
        {
            if (!virtualSO || !virtualSOEditor) return;

            EditorGUILayout.LabelField("Virtual Inspector (Drag values here)", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            virtualSOEditor.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(virtualSO);
            }
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Import", GUILayout.Width(100)))
            {
                ImportInstances();
            }

            if (GUILayout.Button("Export", GUILayout.Width(100)))
            {
                ExportInstances();
            }

            if (GUILayout.Button("Apply Updates", GUILayout.Width(100)))
            {
                ApplyVirtualSOToInstances();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void ApplyVirtualSOToInstances()
        {
            if (virtualSO == null || filteredInstances == null) return;

            Undo.RecordObjects(filteredInstances.ToArray(), "Update SO Instances");

            foreach (var instance in filteredInstances)
            {
                foreach (var field in selectedType.GetFields())
                {
                    if (!fieldToggles.ContainsKey(field.Name) || !fieldToggles[field.Name]) continue;

                    try
                    {
                        field.SetValue(instance, field.GetValue(virtualSO));
                        EditorUtility.SetDirty(instance);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to update {instance.name}.{field.Name}: {e.Message}");
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }

        private void ImportInstances()
        {
            string path = EditorUtility.OpenFilePanel("Import Instances", "", "json");
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                string json = File.ReadAllText(path);
                var importedData = JsonUtility.FromJson<SOImportData>(json);

                // 实现导入逻辑...
            }
            catch (Exception e)
            {
                Debug.LogError($"Import failed: {e.Message}");
            }
        }

        private void ExportInstances()
        {
            string path =
                EditorUtility.SaveFilePanel("Export Instances", "", $"{selectedType.Name}_Instances.json", "json");
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                var exportData = new SOExportData
                {
                    typeName = selectedType.FullName,
                    instances = filteredInstances.Select(instance =>
                    {
                        var instanceData = new Dictionary<string, object>();
                        foreach (var field in selectedType.GetFields())
                        {
                            instanceData[field.Name] = field.GetValue(instance);
                        }

                        return instanceData;
                    }).ToList()
                };

                string json = JsonUtility.ToJson(exportData, true);
                File.WriteAllText(path, json);
                Debug.Log($"Exported {filteredInstances.Count} instances to {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Export failed: {e.Message}");
            }
        }

        // 辅助类用于JSON序列化
        [Serializable]
        private class SOExportData
        {
            public string typeName;
            public List<Dictionary<string, object>> instances;
        }

        [Serializable]
        private class SOImportData
        {
            public string typeName;
            public List<Dictionary<string, object>> instances;
        }

        // 其他辅助方法和处理程序...
        // HandleNavAreaResize, HandleExecutionAreaResize, HandleFilterAreaResize 等保持不变
        // 绘制分隔线
        private void DrawHorizontalLine()
        {
            Color lineColor = new Color(0.4f, 1f, 0.66f); // More visible gray color
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, lineColor);
        }


        private void DrawFilterArea()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width / 2 - 100));

            EditorGUILayout.LabelField("Filter Options", EditorStyles.boldLabel);

            foreach (var field in selectedType.GetFields())
            {
                EditorGUILayout.BeginHorizontal();
                fieldToggles[field.Name] =
                    EditorGUILayout.ToggleLeft(field.Name, fieldToggles[field.Name], GUILayout.Width(100));

                // 只读文本框，不响应实时输入
                string newFilter = EditorGUILayout.TextField(fieldFilters[field.Name],
                    GUILayout.Width(position.width / 4));

                // 只在应用按钮点击时更新过滤条件
                if (GUI.changed)
                {
                    fieldFilters[field.Name] = newFilter;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Apply Filters"))
            {
                ApplyFilters();
            }

            EditorGUILayout.EndVertical();
        }

        private void ApplyFilters()
        {
            filteredInstances = instances.Where(instance =>
            {
                foreach (var field in selectedType.GetFields())
                {
                    // 只检查有筛选条件的字段
                    string filter = fieldFilters[field.Name];
                    if (string.IsNullOrEmpty(filter)) continue;

                    object value = field.GetValue(instance);

                    // 使用更精确的匹配方法
                    try
                    {
                        // 对于字符串类型，使用包含匹配
                        if (value == null && !string.IsNullOrEmpty(filter)) return false;

                        string stringValue = value?.ToString() ?? "";
                        if (!stringValue.Contains(filter, StringComparison.OrdinalIgnoreCase))
                            return false;
                    }
                    catch
                    {
                        // 如果转换失败，不匹配
                        return false;
                    }
                }

                return true;
            }).ToList();

            // 触发重绘
            Repaint();
        }

        private void HandleNavAreaResize(Rect resizeRect)
        {
            Event e = Event.current;

            if (e.type == EventType.MouseDown && resizeRect.Contains(e.mousePosition))
            {
                resizingNavArea = true;
            }

            if (resizingNavArea && e.type == EventType.MouseDrag)
            {
                navAreaWidth = Mathf.Clamp(e.mousePosition.x, 100f, position.width - 200f);
                Repaint();
            }

            if (e.type == EventType.MouseUp)
            {
                resizingNavArea = false;
            }
        }
    }
}