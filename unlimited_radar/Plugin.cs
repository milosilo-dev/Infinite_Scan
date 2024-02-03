using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using unlimited_radar.Paches;

namespace unlimited_radar
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class UnlimitedRadarBase : BaseUnityPlugin
    {
        private const string modGUID = "TSD.unlimitedRadar";
        private const string modName = "Unlimited Radar";
        private const string modVersion = "1.1.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static UnlimitedRadarBase Instance;

        internal ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Unlimited Radar");

            harmony.PatchAll(typeof(UnlimitedRadarBase));
            harmony.PatchAll(typeof(AssignNewNodesPatch));
            harmony.PatchAll(typeof(AddNewScrapFoundToDisplayPatch));
            harmony.PatchAll(typeof(UpdateScanNodesPatch));
        }
    }
}
