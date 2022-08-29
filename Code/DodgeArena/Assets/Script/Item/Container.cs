using System;
using System.Collections.Generic;

/// <summary>
/// 아이템을 보관합니다
/// </summary>
public class Container {
    private List<ItemStack> _content;
    public IEnumerator<ItemStack> content {
        get => _content.GetEnumerator();
    }
    public int size {
        get => _content.Count;
    }
    public delegate void ContainerChange(Container self);
    public event ContainerChange changeEvent;

    public Container(int amount) {
        _content = new List<ItemStack>();
        for(int i = 0; i < amount; i++) {
            _content.Add(ItemStack.Empty);
        }
    }

    public ItemStack this[int index] {
        get {
            return _content[index];
        }
        set {
            _content[index] = value;
        }
    }

    public void Resize(int size) {
        _content = new List<ItemStack>();
        for(int i = 0; i < size; i++) {
            _content.Add(ItemStack.Empty);
        }
        UpdateChange();
    }

    public void UpdateChange() {
        changeEvent(this);
    }

    /// <summary>
    /// 입력된 아이템을 추가합니다.
    /// </summary>
    /// <param name="items">추가할 아이템</param>
    public void AddItems(List<ItemStack> items) {
        foreach(ItemStack item in items) {
            AddItem(item);
        }
    }

    /// <summary>
    /// 입력된 아이템을 추가합니다.
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    public ItemStack AddItem(ItemStack item) {
        ItemStack copy = item.Clone();
        foreach(ItemStack check in _content) {
            if(!check.IsEmpty()) {
                check.AddItem(copy);
                if(copy.amount <= 0) {
                    return ItemStack.Empty;
                }
            }
        }
        if(copy.amount <= 0) {
            return ItemStack.Empty;
        }
        foreach(ItemStack check in _content) {
            if(check.IsEmpty()) {
                check.AddItem(copy);
                if(copy.amount <= 0) {
                    return ItemStack.Empty;
                }
            }
        }
        return copy;
    }

    /// <summary>
    /// 입력된 아이템을 제거합니다.
    /// 파라미터를 수정합니다. (제거하고 남은 아이템임)
    /// </summary>
    /// <param name="items">제거할 아이템</param>
    public void RemoveItems(List<ItemStack> items) {
        foreach(ItemStack item in items) {
            RemoveItem(item);
        }
    }

    /// <summary>
    /// 입력된 아이템을 제거합니다.
    /// 파라미터를 수정합니다. (제거하고 남은 아이템임)
    /// </summary>
    /// <param name="item">제거할 아이템</param>
    public void RemoveItem(ItemStack item) {
        foreach(ItemStack check in _content) {
            check.RemoveItem(item);
            if(item.amount <= 0) {
                return;
            }
        }
    }

    public void RemoveItem(ItemType type, int count) {
        int left = count;
        foreach(ItemStack check in _content) {
            if(check.type.Equals(type)) {
                int amount = Math.Min(left, check.amount);
                check.OperateAmount(-amount);
                left -= amount;
                if(left <= 0) {
                    return;
                }
            }
        }
    }

    public override string ToString() {
        string output = "";
        foreach(ItemStack item in _content) {
            output += item.ToString() + ", ";
        }
        return output.Substring(0, output.Length - 2);
    }
}