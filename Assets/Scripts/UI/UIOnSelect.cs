using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class UIOnSelect : Selectable
{
    [System.Serializable]
    public class SelectEvent : UnityEvent { }

    // 创建一个可在 Inspector 中编辑的事件
    public SelectEvent onSelect;
    public SelectEvent onDeselect;

    // 当被选择时调用
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);  // 调用基类的方法 主要是颜色变化
        onSelect.Invoke();
    }

    // 当取消选择时调用
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);  // 调用基类的方法 主要是颜色变化
        onDeselect.Invoke();
    }
}

