using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public class ScriptableObjectManager : EditorWindow
{
    // 整体布局
    private float navAreaWidth = 150f;
    private float executionAreaHeight = 150f;
    private bool resizingNavArea = false;
    private bool resizingExecutionArea = false;

    // 导航区相关变量
    private Vector2 navScrollPos;
    private Type[] soTypes;
    private Type selectedType;

    // 工作区相关变量
    private Vector2 workScrollPos;
    private List<ScriptableObject> instances;
    private List<ScriptableObject> filteredInstances;

    // 工作区调整布局
    private int columnsPerRow = 3; // 默认每行显示 3 个实例
    private float columnWidth = 300; // 每列的宽度
    private float rowSpacing = 10f; // 行间间隙

    // 执行区相关变量
    // 在类的其他成员变量中添加
    private Vector2 executionScrollPos;
    private Dictionary<string, bool> fieldToggles = new Dictionary<string, bool>();
    private Dictionary<string, string> fieldFilters = new Dictionary<string, string>();
    private Dictionary<string, string> updateFields = new Dictionary<string, string>();
    private Dictionary<ScriptableObject, Editor> cachedEditors = new Dictionary<ScriptableObject, Editor>();


    [MenuItem("Window/Scriptable Object Manager")]
    public static void OpenWindow()
    {
        GetWindow<ScriptableObjectManager>("SO Manager");
    }

    private void OnEnable()
    {
        // 获取所有用户自定义的 ScriptableObject 类型
        soTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(ScriptableObject))
                        && !t.IsAbstract
                        && t.GetCustomAttributes(typeof(CreateAssetMenuAttribute), false).Length > 0)
            .ToArray();
    }


    private void OnGUI()
    {
        Rect totalRect = position;

        // Navigation and Workspace areas
        Rect navRect = new Rect(0, 0, navAreaWidth, totalRect.height - executionAreaHeight);
        Rect workspaceRect = new Rect(navAreaWidth, 0, totalRect.width - navAreaWidth,
            totalRect.height - executionAreaHeight);

        // Draw nav area
        GUILayout.BeginArea(navRect);
        DrawNavigationArea();
        GUILayout.EndArea();

        // Resize handle for nav area
        Rect navResizeRect = new Rect(navAreaWidth - 5, 0, 10, navRect.height);
        EditorGUIUtility.AddCursorRect(navResizeRect, MouseCursor.ResizeHorizontal);

        // Draw workspace
        GUILayout.BeginArea(workspaceRect);
        DrawWorkspace();
        GUILayout.EndArea();

        // Handle navigation area resizing
        HandleNavAreaResize(navResizeRect);

        // Execution area
        Rect executionRect = new Rect(0, totalRect.height - executionAreaHeight, totalRect.width, executionAreaHeight);
        GUILayout.BeginArea(executionRect);
        DrawExecutionArea();
        GUILayout.EndArea();

        // Resize handle for execution area
        Rect executionResizeRect = new Rect(0, totalRect.height - executionAreaHeight - 5, totalRect.width, 10);
        EditorGUIUtility.AddCursorRect(executionResizeRect, MouseCursor.ResizeVertical);

        // Handle execution area resizing
        HandleExecutionAreaResize(executionResizeRect);
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

    private void HandleExecutionAreaResize(Rect resizeRect)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && resizeRect.Contains(e.mousePosition))
        {
            resizingExecutionArea = true;
        }

        if (resizingExecutionArea && e.type == EventType.MouseDrag)
        {
            executionAreaHeight = Mathf.Clamp(position.height - e.mousePosition.y, 100f, position.height - 200f);
            Repaint();
        }

        if (e.type == EventType.MouseUp)
        {
            resizingExecutionArea = false;
        }
    }


    private void DrawNavigationArea()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(150));
        navScrollPos = EditorGUILayout.BeginScrollView(navScrollPos);

        foreach (var type in soTypes)
        {
            if (GUILayout.Button(type.Name, EditorStyles.toolbarButton))
            {
                selectedType = type;
                LoadInstances();
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
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
        updateFields.Clear();

        foreach (var field in selectedType.GetFields())
        {
            fieldToggles[field.Name] = true;
            fieldFilters[field.Name] = "";
            updateFields[field.Name] = "";
        }
    }

    private void DrawWorkspace()
    {
        if (selectedType == null || filteredInstances == null) return;
        EditorGUILayout.BeginVertical();

        // 调整列数的滑块
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Columns Per Row:", GUILayout.Width(120));
        columnsPerRow = EditorGUILayout.IntSlider(columnsPerRow, 1, 10);
        EditorGUILayout.EndHorizontal();

        // 开始滚动区域
        workScrollPos = EditorGUILayout.BeginScrollView(workScrollPos);

        bool isFirstRow = true; // 用于判断是否是第一行
        int currentColumn = 0; // 当前列数
        EditorGUILayout.BeginHorizontal(); // 开始行

        foreach (var instance in filteredInstances)
        {
            if (!cachedEditors.ContainsKey(instance))
            {
                cachedEditors[instance] = Editor.CreateEditor(instance);
            }

            // 每列绘制一个实例
            EditorGUILayout.BeginVertical(GUILayout.Width(columnWidth));
            EditorGUILayout.LabelField(instance.name, EditorStyles.boldLabel); // 实例名称
            cachedEditors[instance].OnInspectorGUI(); // 实例内容
            EditorGUILayout.EndVertical();

            currentColumn++;


            // 到达列数上限时换行
            if (currentColumn >= columnsPerRow)
            {
                EditorGUILayout.EndHorizontal(); // 结束当前行
                if (!isFirstRow)
                {
                    // 添加行间间隙或分隔线
                    GUILayout.Space(rowSpacing); // 行间间隙
                    DrawHorizontalLine(); // 分隔线
                }

                isFirstRow = false;

                EditorGUILayout.BeginHorizontal(); // 开始新行
                currentColumn = 0;
            }
        }

        EditorGUILayout.EndHorizontal(); // 结束最后一行
        EditorGUILayout.EndScrollView(); // 结束滚动区域
        EditorGUILayout.EndVertical();
    }

    private void DrawExecutionArea()
    {
        if (selectedType == null || filteredInstances == null) return;

        // 创建一个垂直滚动视图
        executionScrollPos = EditorGUILayout.BeginScrollView(
            executionScrollPos,
            GUILayout.Height(150),
            GUILayout.ExpandWidth(true)
        );

        EditorGUILayout.BeginHorizontal();
        DrawFilterArea();
        DrawUpdateArea();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        // 按钮区域
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Export", GUILayout.Width(100)))
        {
            ExportInstances();
        }

        if (GUILayout.Button("Apply Updates", GUILayout.Width(100)))
        {
            ApplyUpdates();
        }

        EditorGUILayout.EndHorizontal();
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
            fieldFilters[field.Name] = EditorGUILayout.TextField(fieldFilters[field.Name]);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Apply Filters"))
        {
            ApplyFilters();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawUpdateArea()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width / 2 - 100));

        EditorGUILayout.LabelField("Update Options", EditorStyles.boldLabel);

        foreach (var field in selectedType.GetFields())
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(field.Name, GUILayout.Width(100));
            updateFields[field.Name] = EditorGUILayout.TextField(updateFields[field.Name]);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private void ApplyFilters()
    {
        filteredInstances = instances.Where(instance =>
        {
            foreach (var field in selectedType.GetFields())
            {
                // 只检查被勾选的字段
                if (!fieldToggles[field.Name]) continue;

                object value = field.GetValue(instance);
                string filter = fieldFilters[field.Name];

                // 如果有筛选条件，则进行正则匹配
                if (!string.IsNullOrEmpty(filter) && !Regex.IsMatch(value?.ToString() ?? "", filter))
                    return false;
            }

            return true;
        }).ToList();
    }


    private void ApplyUpdates()
    {
        foreach (var instance in filteredInstances)
        {
            foreach (var field in selectedType.GetFields())
            {
                if (!updateFields.ContainsKey(field.Name)) continue;
                string value = updateFields[field.Name];
                if (string.IsNullOrEmpty(value)) continue;

                try
                {
                    object parsedValue = Convert.ChangeType(value, field.FieldType);
                    field.SetValue(instance, parsedValue);
                    EditorUtility.SetDirty(instance);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to update field {field.Name} with value {value}: {e.Message}");
                }
            }
        }

        AssetDatabase.SaveAssets();
    }

    private void ExportInstances()
    {
        // 选择保存路径
        string path =
            EditorUtility.SaveFilePanel("Export Instances", "", $"{selectedType.Name}_Instances.json", "json");
        if (string.IsNullOrEmpty(path)) return;

        // 构建数据
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        foreach (var instance in filteredInstances)
        {
            var dict = new Dictionary<string, object>();
            foreach (var field in selectedType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.Instance))
            {
                dict[field.Name] = field.GetValue(instance);
            }

            data.Add(dict);
        }

        // 调用用户实现的 JSON 生成方法
        string json = GenerateJson(data);

        // 写入文件
        if (!string.IsNullOrEmpty(json))
        {
            System.IO.File.WriteAllText(path, json);
            Debug.Log($"Exported {filteredInstances.Count} instances to {path}");
        }
        else
        {
            Debug.LogError("Failed to generate JSON. Ensure the GenerateJson method is implemented correctly.");
        }
    }

    /// <summary>
    /// 将 ScriptableObject 数据序列化为 JSON 字符串。
    /// </summary>
    /// <param name="data">包含每个实例字段及其值的字典列表。</param>
    /// <returns>序列化后的 JSON 字符串。</returns>
    private string GenerateJson(List<Dictionary<string, object>> data)
    {
        // 你来实现具体的 JSON 序列化逻辑，并返回生成的 JSON 字符串
        throw new NotImplementedException("Please implement the GenerateJson method.");
    }


    // 绘制分隔线
    private void DrawHorizontalLine()
    {
        Color lineColor = new Color(1f, 1f, 1f, 1f); // More visible gray color
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, lineColor);
    }
}