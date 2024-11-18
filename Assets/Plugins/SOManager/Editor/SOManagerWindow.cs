using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SOManagerWindow : EditorWindow
{
    private Vector2 _leftPanelScroll;
    private List<ScriptableObject> _soInstances = new();
    private string _selectedSOType;
    private Vector2 _rightPanelScroll;

    private TreeNode rootNode = new("Root");

    private TreeNode _selectedNode = null;

    private GUIStyle _selectedStyle;


    // Function registration
    private List<FunctionWrapper> _functions = new();
    private FunctionWrapper _selectedFunction = null;
    private Dictionary<string, object> _functionParameters = new();
   
    private FunctionExecutor _functionExecutor;

    [MenuItem("Window/Bin/SO Manager")]
    public static void ShowWindow()
    {
        var window = GetWindow<SOManagerWindow>("SO Manager");
        window.minSize = new Vector2(800, 600);
    }

    private readonly float[]
        columnWidths = { 150 }; // Adjust column widths

    private List<Type> _scriptableObjectTypes = new List<Type>();

    private void LoadAllScriptableObjectTypes()
    {
        _scriptableObjectTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ScriptableObject)) &&
                           type.GetCustomAttribute<CreateAssetMenuAttribute>() != null)
            .ToList();
    }

    private float getColumnWidth(int i)
    {
        if (i < columnWidths.Length)
        {
            return columnWidths[i];
        }

        return columnWidths[0];
    }


    private void OnEnable()
    {
        LoadAllScriptableObjectTypes();
        BuildTree();
        RegisterFunctions();
    }

    private void RegisterFunctions()
    {
        // 初始化 FunctionExecutor
        _functionExecutor = new FunctionExecutor(_soInstances);
    }
    


    private void OnGUI()
    {
        // Initialize selected style
        _selectedStyle = new GUIStyle(EditorStyles.label)
        {
            normal =
            {
                background = CreateHighlightTexture(new Color(0.2f, 0.6f, 1f, 0.5f)), // Light blue background
                textColor = Color.white
            }
        };
        EditorGUILayout.BeginHorizontal();
        DrawLeftPanel();
        DrawRightPanel();
        EditorGUILayout.EndHorizontal();

        DrawBottomPanel();
    }

    private void DrawBottomPanel()
    { 
        _functionExecutor.DrawBottomPanel();
    }

    private void DrawLeftPanel()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Scriptable Object Types", EditorStyles.boldLabel);

        _leftPanelScroll = EditorGUILayout.BeginScrollView(_leftPanelScroll);

        DrawTree(rootNode);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawRightPanel()
    {
        if (string.IsNullOrEmpty(_selectedSOType))
        {
            EditorGUILayout.LabelField("Select a ScriptableObject from the left panel.");
            return;
        }

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField($"Data Grid: {_selectedSOType}", EditorStyles.boldLabel);

        _rightPanelScroll = EditorGUILayout.BeginScrollView(_rightPanelScroll);

        if (_soInstances.Count > 0)
        {
            // Group instances by their actual type
            var instancesByType = _soInstances.GroupBy(instance => instance.GetType());

            foreach (var typeGroup in instancesByType)
            {
                var instanceType = typeGroup.Key;
                var instances = typeGroup.ToList();

                // Get fields for this specific type
                var fields =
                    instanceType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var baseType = instanceType.BaseType;
                while (baseType != null && baseType != typeof(ScriptableObject))
                {
                    fields = fields
                        .Concat(baseType.GetFields(BindingFlags.Public | BindingFlags.Instance |
                                                   BindingFlags.DeclaredOnly)).ToArray();
                    baseType = baseType.BaseType;
                }

                // Draw section header for this type
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField($"Type: {instanceType.Name}", EditorStyles.boldLabel);

                DrawTableHeader(fields);

                foreach (var instance in instances)
                {
                    EditorGUILayout.BeginHorizontal();
                    DrawTableRow(instance, fields);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("No instances found for the selected type.");
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawTableHeader(FieldInfo[] fields)
    {
        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < fields.Length; i++)
        {
            EditorGUILayout.LabelField(fields[i].Name, EditorStyles.boldLabel, GUILayout.Width(getColumnWidth(i)));
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTableRow(ScriptableObject instance, FieldInfo[] fields)
    {
        EditorGUILayout.BeginHorizontal();

        var serializedObject = new SerializedObject(instance);

        // 在这行中应用任何修改
        bool modified = false;

        foreach (var field in fields)
        {
            var property = serializedObject.FindProperty(field.Name);
            if (property != null)
            {
                EditorGUILayout.PropertyField(property, GUIContent.none,
                    GUILayout.Width(getColumnWidth(Array.IndexOf(fields, field))));
                modified = true; // 标记已经修改
            }
            else
            {
                EditorGUILayout.LabelField($"Unsupported ({field.FieldType.Name})",
                    GUILayout.Width(getColumnWidth(Array.IndexOf(fields, field))));
            }
        }

        if (modified)
        {
            // 如果数据被修改，应用更改
            serializedObject.ApplyModifiedProperties();

            // 保存修改
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.EndHorizontal();
    }

    #region Draw Tree

    private void BuildTree()
    {
        rootNode.Children.Clear();

        foreach (var soType in _scriptableObjectTypes)
        {
            var menuName = soType.GetCustomAttribute<CreateAssetMenuAttribute>()?.menuName;
            if (string.IsNullOrEmpty(menuName)) continue;

            var parts = menuName.Split('/');
            TreeNode currentNode = rootNode;

            foreach (var part in parts)
            {
                var child = currentNode.Children.Find(n => n.Name == part);
                if (child == null)
                {
                    child = new TreeNode(part);
                    currentNode.Children.Add(child);
                }

                currentNode = child;
            }

            // Store ScriptableObject type in the leaf node
            currentNode.Instances.AddRange(AssetDatabase.FindAssets($"t:{soType.Name}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(instance => instance != null));
        }
    }

    private Texture2D CreateHighlightTexture(Color color)
    {
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private void DrawTree(TreeNode node)
    {
        if (node == rootNode)
        {
            foreach (var child in node.Children)
            {
                DrawTree(child);
            }

            return;
        }

        EditorGUILayout.BeginHorizontal();

        if (node.Children.Count > 0)
        {
            // Intermediate node: display foldout button
            node.IsExpanded = EditorGUILayout.Foldout(node.IsExpanded, node.Name, true);
        }
        else
        {
            // Leaf node: draw button
            if (_selectedNode == node)
            {
                if (GUILayout.Button(node.Name, _selectedStyle))
                {
                    _selectedNode = node;
                    _selectedSOType = node.Name;
                    _soInstances = node.Instances;
                }
            }
            else
            {
                if (GUILayout.Button(node.Name, EditorStyles.label))
                {
                    _selectedNode = node;
                    _selectedSOType = node.Name;
                    _soInstances = node.Instances;
                }
            }
        }

        EditorGUILayout.EndHorizontal();

        if (node.IsExpanded)
        {
            EditorGUI.indentLevel++;
            foreach (var child in node.Children)
            {
                DrawTree(child);
            }

            EditorGUI.indentLevel--;
        }
    }

    #endregion

    private void ExecuteFunction()
    {
        if (_selectedFunction != null)
        {
            // Collect parameters
            var parameterValues = _functionParameters.Values.ToArray();
            _selectedFunction.Execute(parameterValues);
        }
    }


    #region Function Usage

    private void GetAllSOInstances(params object[] parameters)
    {
        // Return all instances of ScriptableObject
        return ;
    }

    private void FilterSOInstancesByType(params object[] parameters)
    {
        var type = (string)parameters[0]; // Get the type from the parameters
        // return _soInstances.Where(so => so.GetType().Name.Contains(type)).ToList();
    }

    #endregion
}