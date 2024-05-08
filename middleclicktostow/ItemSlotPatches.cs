using HarmonyLib;
using System.Text;
using Vintagestory.API.Common;

namespace middleclicktostow
{
    [HarmonyPatch(typeof(ItemSlot))]
    public static class ItemSlotPatches
    {
        public static ICoreAPI Api => MiddleClickToStowModSystem.Api;

        [HarmonyPostfix, HarmonyPatch("ActivateSlotMiddleClick")]
        public static void Before__ActivateSlotMiddleClick(ItemSlot __instance, ItemSlot sinkSlot, ref ItemStackMoveOperation op)
        {
            // from InventoryBase.ActivateSlot()
            ItemSlot sourceSlot = __instance;
            if (sourceSlot.Empty || op.MovedQuantity > 0) return; // if there's nothing to transfer, or if something already happened
            string? stackName = sourceSlot.Itemstack?.GetName();
            string? sourceInv = sourceSlot.Inventory?.InventoryID;

            StringBuilder shiftClickDebugText = new StringBuilder();

            op.RequestedQuantity = sourceSlot.StackSize;
            op.ActingPlayer.InventoryManager.TryTransferAway(sourceSlot, ref op, true, shiftClickDebugText);

            Api.World.Logger.Audit("{0} shift clicked slot in {1}. Moved {2}x{3} to ({4})", op.ActingPlayer?.PlayerName, sourceInv, op.MovedQuantity, stackName, shiftClickDebugText.ToString());
        }
    }
}