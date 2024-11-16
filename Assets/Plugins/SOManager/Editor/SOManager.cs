using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SOManager
{
    
    // 获取所有 ScriptableObject 类型
    public static List<string> GetAllScriptableObjectTypes()
    {
        var types = new List<string>();
        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(ScriptableObject)))
                {
                    types.Add(type.Name);
                }
            }
        }
        return types;
    }

    // 加载指定类型的所有实例
    public static List<ScriptableObject> LoadAllInstances(string typeName)
    {
        var results = new List<ScriptableObject>();
        var guids = AssetDatabase.FindAssets("t:ScriptableObject");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset && asset.GetType().Name == typeName)
            {
                results.Add(asset);
            }
        }

        return results;
    }
    
    
    // 获取所有 ScriptableObject 及其路径（基于 CreateAssetMenu 的 MenuName）
    public static Dictionary<string, List<string>> GetScriptableObjectPaths()
    {
        var menuPaths = new Dictionary<string, List<string>>();

        foreach (var type in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var t in type.GetTypes())
            {
                if (t.IsSubclassOf(typeof(ScriptableObject)))
                {
                    // 获取 ScriptableObject 类型的 MenuName
                    var attr = t.GetCustomAttributes(typeof(CreateAssetMenuAttribute), false);
                    if (attr.Length > 0)
                    {
                        var menuName = ((CreateAssetMenuAttribute)attr[0]).menuName;

                        // 按路径分组
                        var root = menuName.Contains("/") ? menuName.Split('/')[0] : menuName;
                        if (!menuPaths.ContainsKey(root))
                        {
                            menuPaths[root] = new List<string>();
                        }
                        menuPaths[root].Add(menuName);
                    }
                }
            }
        }
        return menuPaths;
    }
    
    // 层级节点表示
    public class MenuNode
    {
        public string Name;
        public Dictionary<string, MenuNode> Children = new Dictionary<string, MenuNode>();
        public List<string> SOPaths = new List<string>(); // 对应 ScriptableObject 类型
    }
    
    
    // 构建层级树
    public static MenuNode BuildMenuTree()
    {
        var root = new MenuNode { Name = "Root" };

        foreach (var type in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var t in type.GetTypes())
            {
                if (t.IsSubclassOf(typeof(ScriptableObject)))
                {
                    var attr = t.GetCustomAttributes(typeof(CreateAssetMenuAttribute), false);
                    if (attr.Length > 0)
                    {
                        var menuName = ((CreateAssetMenuAttribute)attr[0]).menuName;
                        AddToMenuTree(root, menuName, t.Name);
                    }
                }
            }
        }

        return root;
    }

    private static void AddToMenuTree(MenuNode node, string path, string soTypeName)
    {
        var parts = path.Split('/');
        if (parts.Length == 0) return;

        if (!node.Children.ContainsKey(parts[0]))
        {
            node.Children[parts[0]] = new MenuNode { Name = parts[0] };
        }

        if (parts.Length > 1)
        {
            AddToMenuTree(node.Children[parts[0]], string.Join("/", parts, 1, parts.Length - 1), soTypeName);
        }
        else
        {
            node.Children[parts[0]].SOPaths.Add(soTypeName);
        }
    }
    
}
