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
        Undefind,
        Helmet, 
        Chestplate,
        Boots,
        Hand,
        Quickbar1,
        Quickbar2,
        Quickbar3,
        Quickbar4
    }

    private ItemStack helmet;
    private ItemStack chestplate;
    private ItemStack boots;
    private ItemStack hand;
    private ItemStack[] quickbar;

    public Equipments() {
        this.helmet = ItemStack.Empty;
        this.chestplate = ItemStack.Empty;
        this.boots = ItemStack.Empty;
        this.hand = ItemStack.Empty;
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
        outSlot = Slot.Undefind;
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
}