using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCleaner : MonoBehaviour
{
    [ContextMenu("Clear Tilemap")]
    private void ClearTilemapContext()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap != null)
        {
            tilemap.ClearAllTiles();
            Debug.Log("Tilemap 已通过右键菜单清空");
        }
        else
        {
            Debug.LogWarning("此对象没有 Tilemap 组件");
        }
    }
}
