using UnityEngine;

namespace PajamaNinja.Scripts.Common
{
    public class Analytics : MonoBehaviour
    {
        private const string GAME_START = "game_started";
        private const string TUTORIAL_START = "tutorial_started";
        private const string TUTORIAL_COMPLETE = "tutorial_completed";
        private const string PURCHASE_BARBERCHAIR = "purchase_barberChair";
        
        // private const string TUTORIAL_GYM_COMPLETE = "tutorial_gym_complete";
        // private const string TUTORIAL_DEFIBRILLATOR_COMPLETE = "tutorial_defibrillator_complete";
        // private const string TUTORIAL_WATERPACK_COMPLETE = "tutorial_waterpack_complete";
        // private const string PURCHASE_WARD = "purchase_ward";
        // private const string PURCHASE_NURSE = "purchase_nurse";
        // private const string PURCHASE_COACH = "purchase_coach";
        // private const string PURCHASE_RECEPTIONIST = "purchase_receptionist";
        // private const string PURCHASE_GYM = "purchase_gym";
        // private const string PURCHASE_OPENSPACE = "purchase_openspace";
        //
        // private const string UPGRADE_WARD = "upgrade_ward";
        // private const string UPGRADE_NURSE = "upgrade_nurse";
        // private const string UPGRADE_RECEPTIONIST = "upgrade_receptionist";
        //
        // private const string PURCHASE_VIP_WARD = "purchase_vip_ward";
        // private const string PURCHASE_VIP_WARD_FURNITURE = "purchase_vip_ward_furniture";
        //
        // private const string PURCHASE_OPENSPACE_ZONE = "purchase_openspace_zone";
        //
        // private const string COMPASS_USE = "compass_use";
        //
        // private const string PATIENT_SCARE_RUN_START = "patient_scare_run_start";
        // private const string PATIENT_SCARE_RUN_COMPLETE = "patient_scare_run_complete";
        //
        // private const string ADS_REWARDED_POPUP_OPEN = "ads_rewarded_popup_open";
        // private const string ADS_REWARDED_VIP_PATIENT = "ads_rewarded_vip_patient";
        //
        // private const string RESORT_LEVEL_UP = "resort_level_up";
        //
        // private const string PURCHASE_PHARMACY = "purchase_pharmacy";
        // private const string UPGRADE_PHARMACY = "upgrade_pharmacy";
        // private const string PURCHASE_PHARMACIST = "purchase_pharmacist";
        // private const string PURCHASE_MASSAGE = "purchase_massage";
        // private const string PURCHASE_MASSEUR = "purchase_masseur";
        // private const string UPGRADE_MASSEUR = "upgrade_masseur";
        // private const string PURCHASE_DENTAL = "purchase_dental";
        // private const string PURCHASE_DENTIST = "purchase_dentist";
        // private const string UPGRADE_DENTIST = "upgrade_dentist";
        //
        // private const string AD_INTERSTITIAL_START = "ad_interstitial_start";
        // private const string AD_INTERSTITIAL_COMPLETE = "ad_interstitial_complete";
        // private const string AD_INTERSTITIAL_CLOSE = "ad_interstitial_close";
        // private const string AD_INTERSTITIAL_PAY = "ad_interstitial_pay";
        // private const string AD_INTERSTITIAL_FAIL = "ad_interstitial_fail";
        // private const string AD_REWARDED_START = "ad_rewarded_start";
        // private const string AD_REWARDED_CLOSE = "ad_rewarded_close";
        // private const string AD_REWARDED_COMPLETE = "ad_rewarded_complete";
        // private const string AD_REWARDED_FAIL = "ad_rewarded_fail";

        // private static bool _isFreeplayInitialized;
        // private static AnalyticsEventsBuffer _eventsBuffer = new AnalyticsEventsBuffer(10);

        // private void OnEnable()
        // {
        //     Freeplay.OnInitialized += OnFreeplayInitialized;
        // }
        //
        // private void OnDisable()
        // {
        //     Freeplay.OnInitialized -= OnFreeplayInitialized;
        // }

        // private void OnFreeplayInitialized()
        // {
        //     _isFreeplayInitialized = true;
        //     
        //     while (_eventsBuffer.CurrentSize > 0)
        //     {
        //         var item = _eventsBuffer.RemoveItem();
        //         if (item.Properties == null)
        //             SendReportEvent(item.Message);
        //         else
        //             SendReportEvent(item.Message, item.Properties);
        //     }
        // }

        // public static void GameStart() => SendReportEvent(GAME_START);
        // public static void TutorialStart() => SendReportEvent(TUTORIAL_START);
        // public static void TutorialComlete() => SendReportEvent(TUTORIAL_COMPLETE);
        // public static void PurchaseBarberChair(string barberChairName) => SendReportEvent(PURCHASE_BARBERCHAIR, new Dictionary<string, object>() { { "barberChair_name", barberChairName } });

        // public static void TutorialGymComlete() => SendReportEvent(TUTORIAL_GYM_COMPLETE);
        // public static void TutorialDefibrillatorComlete() => SendReportEvent(TUTORIAL_DEFIBRILLATOR_COMPLETE);
        // public static void TutorialWaterPackComplete() => SendReportEvent(TUTORIAL_WATERPACK_COMPLETE);
        // public static void PurchaseWard(string wardNumber) => SendReportEvent(PURCHASE_WARD, new Dictionary<string, object>() { { "ward_number", wardNumber } });
        // public static void PurchaseVipWard(string wardNumber) => SendReportEvent(PURCHASE_VIP_WARD, new Dictionary<string, object>() { { "ward_number", wardNumber } });
        //
        //
        // public static void PurchaseNurse(string nurseNumber) => SendReportEvent(PURCHASE_NURSE, new Dictionary<string, object>() { { "nurse_number", nurseNumber } });
        // public static void PurchaseCoach() => SendReportEvent(PURCHASE_COACH);
        // public static void PurchaseReceptionist() => SendReportEvent(PURCHASE_RECEPTIONIST);
        // public static void PurchaseGym() => SendReportEvent(PURCHASE_GYM);
        // public static void PurchaseOpenspace() => SendReportEvent(PURCHASE_OPENSPACE);
        // public static void UpgradeWard(string wardNumber, int upgrade, int skin) => SendReportEvent(UPGRADE_WARD, new Dictionary<string, object>() { { "ward_number", wardNumber }, { "upgrade", upgrade }, { "skin", skin } });
        // public static void UpgradeNurse(string nurseNumber, int upgrade) => SendReportEvent(UPGRADE_NURSE, new Dictionary<string, object>() { { "nurse_number", nurseNumber }, { "upgrade", upgrade } });
        // public static void UpgradeReceptionist(int upgrade) => SendReportEvent(UPGRADE_RECEPTIONIST, new Dictionary<string, object>() { { "upgrade", upgrade } });
        // public static void PurchaseOpenspaceZone(string zoneNumber) => SendReportEvent(PURCHASE_OPENSPACE_ZONE, new Dictionary<string, object>() { { "zone_number", zoneNumber } });
        // public static void CompassUse() => SendReportEvent(COMPASS_USE);
        // public static void PatientScareRunStart() => SendReportEvent(PATIENT_SCARE_RUN_START);
        // public static void PatientScareRunComplete(bool caught) => SendReportEvent(PATIENT_SCARE_RUN_COMPLETE, new Dictionary<string, object>() { { "caught", caught ? 1 : 0 } });
        // public static void AdsRewardPopupOpen(string placement) => SendReportEvent(ADS_REWARDED_POPUP_OPEN, new Dictionary<string, object>() { { "placement", placement } });
        // public static void AdsRewardVipPatient() => SendReportEvent(ADS_REWARDED_VIP_PATIENT);
        // public static void ResortLevelUp(int level) => SendReportEvent(RESORT_LEVEL_UP, new Dictionary<string, object>() { { "level", level} });
        //
        // public static void PurchasePharmacy() => SendReportEvent(PURCHASE_PHARMACY);
        // public static void UpgradePharmacy(int upgrade) => SendReportEvent(UPGRADE_PHARMACY, new Dictionary<string, object>() { { "upgrade", upgrade } });
        // public static void PurchasePharmacist() => SendReportEvent(PURCHASE_PHARMACIST);
        //
        // public static void PurchaseMassage() => SendReportEvent(PURCHASE_MASSAGE);
        // public static void PurchaseMasseur() => SendReportEvent(PURCHASE_MASSEUR);
        // public static void UpgradeMasseur(int upgrade) => SendReportEvent(UPGRADE_MASSEUR, new Dictionary<string, object>() { { "upgrade", upgrade } });
        //
        // public static void PurchaseDental() => SendReportEvent(PURCHASE_DENTAL);
        // public static void PurchaseDentist() => SendReportEvent(PURCHASE_DENTIST);
        // public static void UpgradeDentist(int upgrade) => SendReportEvent(UPGRADE_DENTIST, new Dictionary<string, object>() { { "upgrade", upgrade } });
        //

        // private static void SendReportEvent(string message)
        // {
        //     if (!_isFreeplayInitialized)
        //     {
        //         _eventsBuffer.AddItem(message);
        //         return;
        //     }
        //
        //     // AppMetrica.Instance.ReportEvent(message);
        //     //
        //     // //FlyAnalytics.Instance.SendEvent(message);
        //     //
        //     // if (GameAnalytics.SettingsGA != null)
        //     //     GameAnalytics.NewDesignEvent(message);
        //     //
        //     // Freeplay.LogInfo($"Send Feeplay event: {message}");
        // }
        //
        // private static void SendReportEvent(string message, Dictionary<string, object> properties)
        // {
        //     if (!_isFreeplayInitialized)
        //     {
        //         _eventsBuffer.AddItem(message, properties);
        //         return;
        //     }
        //
        //     // if (GameAnalytics.SettingsGA != null)
        //     //     GameAnalytics.NewDesignEvent(message, properties);
        //     //
        //     // AppMetrica.Instance.ReportEvent(message, properties);
        //     //
        //     // //FlyAnalytics.Instance.SendEvent(message, properties);
        //     //
        //     // Freeplay.LogInfo($"Send Feeplay event: {message}\n{PropertiesToString(properties)}");
        // }
        //
        // private static string PropertiesToString(Dictionary<string, object> properties)
        // {
        //     var sb = new StringBuilder();
        //     foreach ( var prop in properties )
        //     {
        //         sb.AppendLine($"{prop.Key}: {prop.Value}");
        //     }
        //     return sb.ToString();
        // }

    }
}