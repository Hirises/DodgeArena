using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
    public ItemType type;
    public int amount;

    public ItemStack(ItemType type, int amount) {
        this.type = type;
        this.amount = amount;
    }
}
