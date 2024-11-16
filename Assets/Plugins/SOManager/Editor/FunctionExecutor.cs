using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

public class FunctionExecutor
{
    private string _selectedFunction = "GetAllSOInstances";
    private SerializedObject _serializedObject;
    private SerializedProperty _param1Property;
    private SerializedProperty _param2Property;
    private string _executionResult;

    private List<ScriptableObject> _soInstances;

    private string[] _functionNames = new string[] { "GetAllSOInstances", "FilterSOInstances" };

    // 存储函数参数的 ScriptableObject 类型
    private FunctionParameters _functionParameters;

    public FunctionExecutor(List<ScriptableObject> soInstances)
    {
        _soInstances = soInstances;
        _functionParameters = ScriptableObject.CreateInstance<FunctionParameters>(); // 创建 FunctionParameters 实例
        _serializedObject = new SerializedObject(_functionParameters); // 传入 SerializedObject
    }

    // 在窗口的底部面板绘制命令面板
    public void DrawBottomPanel()
    {
        EditorGUILayout.LabelField("Function Executor Command Panel", GUILayout.Height(50));

        // 创建下拉框供用户选择函数
        _selectedFunction = _functionNames[EditorGUILayout.Popup("Select Function", Array.IndexOf(_functionNames, _selectedFunction), _functionNames)];

        // 根据选择的函数，显示相应的参数输入框
        DrawFunctionParameters();

        // 执行按钮
        if (GUILayout.Button("Execute"))
        {
            ExecuteFunction(_selectedFunction);
        }

        // 显示执行结果
        if (!string.IsNullOrEmpty(_executionResult))
        {
            EditorGUILayout.LabelField("Execution Result:");
            EditorGUILayout.LabelField(_executionResult);
        }
    }

    private void DrawFunctionParameters()
    {
        // 创建 SerializedObject 和 SerializedProperty 用来显示输入框
        _serializedObject.Update(); // 先更新 SerializedObject

        if (_selectedFunction == "GetAllSOInstances")
        {
            _param1Property = _serializedObject.FindProperty("param1");
            EditorGUILayout.PropertyField(_param1Property, new GUIContent("ScriptableObject Type"));
        }
        else if (_selectedFunction == "FilterSOInstances")
        {
            _param1Property = _serializedObject.FindProperty("param1");
            _param2Property = _serializedObject.FindProperty("param2");
            EditorGUILayout.PropertyField(_param1Property, new GUIContent("Property Name"));
            EditorGUILayout.PropertyField(_param2Property, new GUIContent("Value"));
        }

        _serializedObject.ApplyModifiedProperties(); // 应用修改
    }

    private void ExecuteFunction(string functionName)
    {
        _executionResult = ""; // 清空先前的结果

        // 获取参数
        string param1 = _param1Property != null ? _param1Property.stringValue : "";
        string param2 = _param2Property != null ? _param2Property.stringValue : "";

        // 根据选择的函数来执行相应的逻辑
        if (functionName == "GetAllSOInstances")
        {
            GetAllSOInstances(param1);
        }
        else if (functionName == "FilterSOInstances")
        {
            FilterSOInstances(param1, param2);
        }
    }

    // 获取所有 ScriptableObject 实例
    private void GetAllSOInstances(string soType)
    {
        var instances = _soInstances.FindAll(so => so.GetType().Name == soType);

        if (instances.Count > 0)
        {
            _executionResult = $"Found {instances.Count} instances of {soType}.";
        }
        else
        {
            _executionResult = $"No instances found for type {soType}.";
        }
    }

    // 按照属性过滤 ScriptableObject 实例
    private void FilterSOInstances(string propertyName, string value)
    {
        var filteredInstances = new List<ScriptableObject>();

        foreach (var instance in _soInstances)
        {
            var property = instance.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var propertyValue = property.GetValue(instance)?.ToString();
                if (propertyValue == value)
                {
                    filteredInstances.Add(instance);
                }
            }
        }

        if (filteredInstances.Count > 0)
        {
            _executionResult = $"Found {filteredInstances.Count} matching instances.";
        }
        else
        {
            _executionResult = $"No instances match the condition ({propertyName} = {value}).";
        }
    }
}

// 辅助类，用于存储参数，这里继承自 ScriptableObject 以便能被 SerializedObject 序列化
public class FunctionParameters : ScriptableObject
{
    public string param1 = "";
    public string param2 = "";
}
