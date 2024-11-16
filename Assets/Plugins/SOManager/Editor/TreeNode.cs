using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public string Name { get; set; }
    public List<TreeNode> Children { get; set; } = new List<TreeNode>();
    public List<ScriptableObject> Instances { get; set; } = new List<ScriptableObject>();
    public bool IsExpanded { get; set; } = false; // 是否展开

    public TreeNode(string name)
    {
        Name = name;
    }
}