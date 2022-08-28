using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// 아이템을 보관합니다
/// </summary>
public class Container {
    public List<ItemStack> content;

    public Container(int amount) {
        content = new List<ItemStack>();
        for(int i = 0; i < amount; i++) {
            content.Add(ItemStack.Empty);
        }
    }

    public ItemStack this[int index] {
        get {
            return content[index];
        }
        set {
            content[index] = value;
        }
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
        foreach(ItemStack check in content) {
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
        foreach(ItemStack check in content) {
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
        foreach(ItemStack check in content) {
            check.RemoveItem(item);
            if(item.amount <= 0) {
                return;
            }
        }
    }

    public void RemoveItem(ItemType type, int count) {
        int left = count;
        foreach(ItemStack check in content) {
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
        foreach(ItemStack item in content) {
            output += item.ToString() + ", ";
        }
        return output.Substring(0, output.Length - 2);
    }
}