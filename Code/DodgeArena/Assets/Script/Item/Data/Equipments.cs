using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 장비들
/// </summary>
public class Equipments {
    public delegate void EquipmentsChange(Equipments self);
    public event EquipmentsChange changeEvent;

    public enum Slot {
        Helmet, 
        Chestplate,
        Boots,
        Hand,
        Jewel1,
        Jewel2,
        Quickbar1,
        Quickbar2,
        Quickbar3,
        Quickbar4
    }

    private ItemStack helmet;
    private ItemStack chestplate;
    private ItemStack boots;
    private ItemStack hand;
    private ItemStack jewel1;
    private ItemStack jewel2;
    private ItemStack[] quickbar;

    public Equipments() {
        this.helmet = ItemStack.Empty;
        this.chestplate = ItemStack.Empty;
        this.boots = ItemStack.Empty;
        this.hand = ItemStack.Empty;
        this.jewel1 = ItemStack.Empty;
        this.jewel2 = ItemStack.Empty;
        this.quickbar = new ItemStack[4];
        this.quickbar[0] = ItemStack.Empty;
        this.quickbar[1] = ItemStack.Empty;
        this.quickbar[2] = ItemStack.Empty;
        this.quickbar[3] = ItemStack.Empty;
    }
        
    public ItemStack GetEquipment(Slot slot) {
        switch(slot) {
            case Slot.Helmet:
                return helmet;
            case Slot.Chestplate:
                return chestplate;
            case Slot.Boots:
                return boots;
            case Slot.Hand:
                return hand;
            case Slot.Jewel1:
                return jewel1;
            case Slot.Jewel2:
                return jewel2;
            case Slot.Quickbar1:
                return quickbar[0];
            case Slot.Quickbar2:
                return quickbar[1];
            case Slot.Quickbar3:
                return quickbar[2];
            case Slot.Quickbar4:
                return quickbar[3];
        }
        return ItemStack.Empty;
    }

    public void Equip(Slot slot, ItemStack item) {
        switch(slot) {
            case Slot.Helmet:
                helmet = item;
                break;
            case Slot.Chestplate:
                chestplate = item;
                break;
            case Slot.Boots:
                boots = item;
                break;
            case Slot.Hand:
                hand = item;
                break;
            case Slot.Jewel1:
                jewel1 = item;
                break;
            case Slot.Jewel2:
                jewel2 = item;
                break;
            case Slot.Quickbar1:
                quickbar[0] = item;
                break;
            case Slot.Quickbar2:
                quickbar[1] = item;
                break;
            case Slot.Quickbar3:
                quickbar[2] = item;
                break;
            case Slot.Quickbar4:
                quickbar[3] = item;
                break;
        }
        if(changeEvent != null) {
            changeEvent(this);
        }
    }

    public bool IsEquiped(ItemStack item) {
        return IsEquiped(item, out Slot _);
    }

    public bool IsEquiped(ItemStack item, out Slot outSlot) {
        foreach(Slot slot in Enum.GetValues(typeof(Slot))) {
            if(GetEquipment(slot) == item) {
                outSlot = slot;
                return true;
            }
        }
        outSlot = Slot.Quickbar1;
        return false;
    }

    public void Unequip(Slot slot, ItemStack item) {
        if(GetEquipment(slot) == item) {
            Equip(slot, ItemStack.Empty);
        }
    }

    public ItemStack[] GetQuickbarItems() {
        return quickbar;
    }

    public ItemStack GetQuickbarItem(int slot) {
        return quickbar[slot];
    }

    public ItemStack AddItem(ItemStack item) {
        ItemStack copy = item.Clone();
        for(int i = 0; i < 4; i++) {
            if(GetQuickbarItem(i).StackableRestrict(copy)) {
                int count = GetQuickbarItem(i).AddItem(copy);
                copy.OperateAmount(-count);
                if(copy.IsEmpty()) {
                    HUDManager.instance.UpdateQuickBar();
                    return copy;
                }
            }
        }
        HUDManager.instance.UpdateQuickBar();
        return copy;
    }
}