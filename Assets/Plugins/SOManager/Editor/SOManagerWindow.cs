using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class SOManagerWindow : EditorWindow
{
    private Vector2 _leftPanelScroll;
    private SOManager.MenuNode _menuRoot;
    private List<ScriptableObject> _soInstances = new();
    private string _selectedSOType;
    private Vector2 _rightPanelScroll;

    private float DEFAULT_LENGTH = 150;

    private TreeNode rootNode = new("Root");

    private TreeNode _selectedNode = null;

    private GUIStyle _selectedStyle;

    [MenuItem("Window/SO Manager")]
    public static void ShowWindow()
    {
        var window = GetWindow<SOManagerWindow>("SO Manager");
        window.minSize = new Vector2(800, 600);
    }

    private readonly float[] columnWidths = { 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, }; // Adjust column widths

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

        return 150;
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

    private void OnEnable()
    {
        LoadAllScriptableObjectTypes();
        BuildTree();

      
    }

    private void DrawBottomPanel()
    {
        EditorGUILayout.LabelField("SQL-like Command Panel", GUILayout.Height(50));
        // TODO: Implement text box and execution functionality
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
            var instanceType = _soInstances[0].GetType();
            var fields = instanceType.GetFields();

            DrawTableHeader(fields);

            foreach (var instance in _soInstances)
            {
                EditorGUILayout.BeginHorizontal();
                DrawTableRow(instance, fields);
                EditorGUILayout.EndHorizontal();
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

        Debug.Log("fields.Length" + fields.Length);
        for (int i = 0; i < fields.Length; i++)
        {
            EditorGUILayout.LabelField(fields[i].Name, EditorStyles.boldLabel, GUILayout.Width(getColumnWidth(i)));
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTableRow(ScriptableObject instance, FieldInfo[] fields)
    {
        EditorGUILayout.BeginHorizontal();

        foreach (var field in fields)
        {
            DrawField(instance, field, GUILayout.Width(getColumnWidth(Array.IndexOf(fields, field))));
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawField(ScriptableObject instance, FieldInfo field, GUILayoutOption width = null)
    {
        var value = field.GetValue(instance);

        if (field.FieldType == typeof(string))
        {
            var newValue = EditorGUILayout.TextField((string)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (newValue != (string)value)
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType == typeof(int))
        {
            var newValue = EditorGUILayout.IntField((int)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (newValue != (int)value)
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType == typeof(float))
        {
            var newValue = EditorGUILayout.FloatField((float)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (newValue != (float)value)
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType == typeof(bool))
        {
            var newValue = EditorGUILayout.Toggle((bool)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (newValue != (bool)value)
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType.IsEnum)
        {
            var newValue = EditorGUILayout.EnumPopup((Enum)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (!newValue.Equals(value))
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType == typeof(Vector2))
        {
            var newValue = EditorGUILayout.Vector2Field("", (Vector2)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (newValue != (Vector2)value)
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType == typeof(Vector3))
        {
            var newValue = EditorGUILayout.Vector3Field("", (Vector3)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (newValue != (Vector3)value)
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType == typeof(Color))
        {
            var newValue = EditorGUILayout.ColorField((Color)value, width ?? GUILayout.Width(DEFAULT_LENGTH));
            if (newValue != (Color)value)
            {
                field.SetValue(instance, newValue);
                EditorUtility.SetDirty(instance);
            }
        }
        else if (field.FieldType.IsArray ||
                 (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)))
        {
            // Get array or list value
            var elementType = field.FieldType.IsArray
                ? field.FieldType.GetElementType()
                : field.FieldType.GetGenericArguments()[0];

            if (value == null)
            {
                // Initialize empty list
                if (field.FieldType.IsArray)
                {
                    value = Array.CreateInstance(elementType, 0);
                }
                else
                {
                    value = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                }

                field.SetValue(instance, value);
                EditorUtility.SetDirty(instance);
            }

            // Convert to list operation (compatible with arrays and lists)
            var list = value as IList;

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField($"List<{elementType.Name}> ({list.Count} items)", EditorStyles.boldLabel);

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                // Element editing
                var item = list[i];
                var newItem = DrawListElement(item, elementType, width);
                if (!Equals(item, newItem))
                {
                    list[i] = newItem;
                    EditorUtility.SetDirty(instance);
                }

                // Delete button
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    list.RemoveAt(i);
                    EditorUtility.SetDirty(instance);
                }

                EditorGUILayout.EndHorizontal();
            }

            // Add button
            if (GUILayout.Button("+ Add Item", GUILayout.Width(100)))
            {
                object newItem = null;
                if (elementType == typeof(string))
                {
                    newItem = "";
                }
                else if (elementType.IsValueType)
                {
                    newItem = Activator.CreateInstance(elementType);
                }

                list.Add(newItem);
                EditorUtility.SetDirty(instance);
            }

            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.LabelField($"Unsupported ({field.FieldType.Name})",
                width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
    }

    private object DrawListElement(object item, Type elementType, GUILayoutOption width = null)
    {
        if (elementType == typeof(string))
        {
            return EditorGUILayout.TextField(item as string, width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (elementType == typeof(int))
        {
            return EditorGUILayout.IntField(item != null ? (int)item : 0, width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (elementType == typeof(float))
        {
            return EditorGUILayout.FloatField(item != null ? (float)item : 0f,
                width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (elementType == typeof(bool))
        {
            return EditorGUILayout.Toggle(item != null && (bool)item, width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (elementType.IsEnum)
        {
            return EditorGUILayout.EnumPopup(item as Enum, width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (elementType == typeof(Vector2))
        {
            return EditorGUILayout.Vector2Field("", item != null ? (Vector2)item : Vector2.zero,
                width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (elementType == typeof(Vector3))
        {
            return EditorGUILayout.Vector3Field("", item != null ? (Vector3)item : Vector3.zero,
                width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (elementType == typeof(Color))
        {
            return EditorGUILayout.ColorField(item != null ? (Color)item : Color.white,
                width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else if (typeof(Object).IsAssignableFrom(elementType))
        {
            return EditorGUILayout.ObjectField(item as Object, elementType, true,
                width ?? GUILayout.Width(DEFAULT_LENGTH));
        }
        else
        {
            EditorGUILayout.LabelField($"Unsupported ({elementType.Name})", width ?? GUILayout.Width(DEFAULT_LENGTH));
            return item;
        }
    }

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
}
