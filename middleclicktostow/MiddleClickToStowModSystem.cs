using HarmonyLib;
using System;
using Vintagestory.API.Common;

namespace middleclicktostow
{
    public class MiddleClickToStowModSystem : ModSystem
    {
        public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

        private static ICoreAPI? _api;
        public static ICoreAPI Api => _api ?? throw new ArgumentNullException(nameof(Api));
        public Harmony? harmony;

        public override void Start(ICoreAPI api)
        {
            _api = api;

            if (!Harmony.HasAnyPatches(Mod.Info.ModID)) {
                harmony = new Harmony(Mod.Info.ModID);
                harmony.PatchAll(); // Applies all harmony patches
            }
        }

        public override void Dispose()
        {
            harmony?.UnpatchAll(Mod.Info.ModID);
            harmony = null;
        }
    }
}