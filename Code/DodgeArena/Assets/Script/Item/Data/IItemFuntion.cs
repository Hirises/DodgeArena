using System.Collections;
using UnityEngine;

public interface IItemFuntion {
    public void CanUse(BackpackSlotHUD slot);

    public bool OnUse(BackpackSlotHUD slot);

    public void CanEquip(BackpackSlotHUD slot);

    public bool OnEquip(BackpackSlotHUD slot);

    public void CanUnequip(BackpackSlotHUD slot);

    public bool OnUnequip(BackpackSlotHUD slot);

    public void CanDiscard(BackpackSlotHUD slot);

    public bool OnDiscard(BackpackSlotHUD slot);
}