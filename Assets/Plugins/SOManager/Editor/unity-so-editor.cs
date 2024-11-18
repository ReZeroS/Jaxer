using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

public class SOBrowserWindow : EditorWindow
{
    private Vector2 leftScrollPosition;
    private Vector2 rightScrollPosition;
    private Vector2 filterScrollPosition;
    private float leftPanelWidth = 200f;
    private float filterPanelHeight = 150f;
    private float itemWidth = 300f;

    // 类型和数据管理
    private List<Type> _scriptableObjectTypes;
    private List<ScriptableObject> allSOs = new List<ScriptableObject>();
    private List<ScriptableObject> filteredSOs = new List<ScriptableObject>();
    private Dictionary<ScriptableObject, Editor> soEditors = new Dictionary<ScriptableObject, Editor>();

    // 筛选相关
    private Type selectedType;
    private List<PropertyFilter> propertyFilters = new List<PropertyFilter>();
    private bool showFilterPanel = false;

    // 属性筛选器类
    [Serializable]
    private class PropertyFilter
    {
        public string propertyName;
        public string filterValue = "";
        public SerializedPropertyType propertyType;
        public bool enabled = true;
    }

    [MenuItem("Window/Bin/SO Browser")]
    public static void ShowWindow()
    {
        GetWindow<SOBrowserWindow>("SO Browser");
    }

    private void OnEnable()
    {
        LoadCustomScriptableObjectTypes();
        filteredSOs = allSOs; // 初始显示所有数据
    }

    private void LoadCustomScriptableObjectTypes()
    {
        _scriptableObjectTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ScriptableObject)) &&
                           type.GetCustomAttribute<CreateAssetMenuAttribute>() != null)
            .ToList();
    }

    private void RefreshSOList()
    {
        allSOs.Clear();
        soEditors.Clear();

        if (selectedType != null)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{selectedType.Name}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (so)
                {
                    allSOs.Add(so);
                    soEditors[so] = Editor.CreateEditor(so);
                }
            }
        }

        ApplyFilters();
    }

    private void OnGUI()
    {
        Debug.Log($"Event type: {Event.current.type}, Mouse position: {Event.current.mousePosition}");
        EditorGUILayout.BeginHorizontal();

        DrawLeftPanel();
        ResizeHandler();
        DrawRightSection();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawLeftPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(leftPanelWidth));

        if (GUILayout.Button("Flush List"))
        {
            RefreshSOList();
        }

        leftScrollPosition = EditorGUILayout.BeginScrollView(leftScrollPosition);

        foreach (var type in _scriptableObjectTypes)
        {
            if (GUILayout.Button(type.Name, selectedType == type ? EditorStyles.boldLabel : EditorStyles.miniButton))
            {
                if (selectedType != type)
                {
                    selectedType = type;
                    RefreshSOList();
                    GeneratePropertyFilters();
                    showFilterPanel = true;
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawRightSection()
    {
        EditorGUILayout.BeginVertical();

        // 主内容区域
        float mainContentHeight = position.height - filterPanelHeight;
        EditorGUILayout.BeginVertical(GUILayout.Height(mainContentHeight));
        DrawSOList();
        EditorGUILayout.EndVertical();

        // 筛选面板
        if (showFilterPanel && selectedType != null)
        {
            DrawFilterPanel();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawSOList()
    {
        rightScrollPosition = EditorGUILayout.BeginScrollView(rightScrollPosition, true, true);
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

        foreach (var so in filteredSOs)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(itemWidth));
            if (soEditors.ContainsKey(so))
            {
                soEditors[so].DrawHeader();
                soEditors[so].OnInspectorGUI();
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }

    private void DrawFilterPanel()
    {
        Debug.Log($"Current Event: {Event.current.type}");
        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(filterPanelHeight));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Attribute Filter", EditorStyles.boldLabel);
        if (GUILayout.Button("apply filter", GUILayout.Width(80)))
        {
            Debug.Log("apply filter");
            ApplyFilters();
        }

        EditorGUILayout.EndHorizontal();

        filterScrollPosition = EditorGUILayout.BeginScrollView(filterScrollPosition);

        foreach (var t in propertyFilters)
        {
            EditorGUILayout.BeginHorizontal();

            t.enabled = EditorGUILayout.Toggle(t.enabled, GUILayout.Width(20));
            EditorGUILayout.LabelField(t.propertyName, GUILayout.Width(150));

            t.filterValue = EditorGUILayout.TextField(t.filterValue);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void GeneratePropertyFilters()
    {
        propertyFilters.Clear();

        if (selectedType == null) return;

        var fields = selectedType.GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(f => !f.IsInitOnly && !f.IsLiteral);

        foreach (var field in fields)
        {
            propertyFilters.Add(new PropertyFilter
            {
                propertyName = field.Name,
                propertyType = GetSerializedPropertyType(field.FieldType)
            });
        }
    }

    private void ApplyFilters()
    {
        filteredSOs = new List<ScriptableObject>(allSOs);

        if (!propertyFilters.Any(f => f.enabled && !string.IsNullOrEmpty(f.filterValue)))
        {
            return;
        }

        filteredSOs = filteredSOs.Where(so =>
        {
            SerializedObject serializedObject = new SerializedObject(so);
            bool matchesAllFilters = true;

            foreach (var filter in propertyFilters.Where(f => f.enabled && !string.IsNullOrEmpty(f.filterValue)))
            {
                SerializedProperty property = serializedObject.FindProperty(filter.propertyName);
                if (property == null) continue;

                bool matchesFilter = MatchesFilter(property, filter.filterValue);
                if (!matchesFilter)
                {
                    matchesAllFilters = false;
                    break;
                }
            }

            return matchesAllFilters;
        }).ToList();
    }

    private bool MatchesFilter(SerializedProperty property, string filterValue)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                if (int.TryParse(filterValue, out int intValue))
                    return property.intValue == intValue;
                break;

            case SerializedPropertyType.Float:
                if (float.TryParse(filterValue, out float floatValue))
                    return Mathf.Approximately(property.floatValue, floatValue);
                break;

            case SerializedPropertyType.String:
                return property.stringValue.Contains(filterValue, StringComparison.OrdinalIgnoreCase);

            case SerializedPropertyType.Boolean:
                if (bool.TryParse(filterValue, out bool boolValue))
                    return property.boolValue == boolValue;
                break;
        }

        return false;
    }

    private SerializedPropertyType GetSerializedPropertyType(Type type)
    {
        if (type == typeof(int)) return SerializedPropertyType.Integer;
        if (type == typeof(float)) return SerializedPropertyType.Float;
        if (type == typeof(string)) return SerializedPropertyType.String;
        if (type == typeof(bool)) return SerializedPropertyType.Boolean;
        return SerializedPropertyType.Generic;
    }

    private void ResizeHandler()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(5));
        GUI.DrawTexture(GUILayoutUtility.GetRect(5f, 5f, GUILayout.ExpandHeight(true)), EditorGUIUtility.whiteTexture);
        EditorGUILayout.EndVertical();

        Rect resizeRect = GUILayoutUtility.GetLastRect();
        EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeHorizontal);

        // 仅在鼠标位于调整区域时处理事件
        if (resizeRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown)
            {
                Debug.Log("Mouse down on resize");
                EditorGUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                Event.current.Use(); // 消耗事件，避免传递给其他控件
            }
            else if (Event.current.type == EventType.MouseDrag && EditorGUIUtility.hotControl != 0)
            {
                leftPanelWidth += Event.current.delta.x;
                leftPanelWidth = Mathf.Clamp(leftPanelWidth, 100f, position.width / 2);
                Repaint();
                Event.current.Use(); // 同样消耗拖动事件
            }
            else if (Event.current.type == EventType.MouseUp && EditorGUIUtility.hotControl != 0)
            {
                EditorGUIUtility.hotControl = 0;
                Event.current.Use(); // 消耗鼠标释放事件
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var editor in soEditors.Values)
        {
            DestroyImmediate(editor);
        }
    }
}