/******************************************************************************/
/*
  Project   - MudBun
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.PackageManager.Requests;

using UnityEngine;

namespace MudBun
{
  [InitializeOnLoad]
  public class CompatibilityManager
  {
    public struct PackageGuidSet
    {
      public static readonly string[] Brp    = { s_brpShaders   , s_brpMaterials    , s_brpResources   };
      public static readonly string[] Urp    = { s_urpShaders   , s_urpMaterials    , s_urpResources   };
      public static readonly string[] Urp10  = { s_urp10Shaders , s_urp10Materials  , s_urp10Resources };
      public static readonly string[] Hdrp   = { s_hdrpShaders  , s_hdrpMaterials   , s_hdrpResources  };
      public static readonly string[] Hdrp10 = { s_hdrp10Shaders, s_hdrp10Materials , s_hdrp10Resources};

      public static readonly string[] BrpExamples    = { s_examplesCommon, s_brpExamples    };
      public static readonly string[] UrpExamples    = { s_examplesCommon, s_urpExamples    };
      public static readonly string[] Urp10Examples  = { s_examplesCommon, s_urp10Examples  };
      public static readonly string[] HdrpExamples   = { s_examplesCommon, s_hdrpExamples   };
      public static readonly string[] Hdrp10Examples = { s_examplesCommon, s_hdrp10Examples };

      private static string s_examplesCommon  => "6825e33867e702f4cb62940d403c8fc6";
      private static string s_brpShaders      => "4241d02d4946ab540b3b6d30b5055ada";
      private static string s_brpMaterials    => "f4708b8089db6bb4db8b7494f584fdf3";
      private static string s_brpResources    => "1141a916eb742b64dbcb20a8b043c653";
      private static string s_brpExamples     => "8c10d79f3f5ed4344bf46c6baa0b41f5";
      private static string s_urpShaders      => "cbc86b1ac9c6d70429669970d84b3ffe";
      private static string s_urpMaterials    => "4c59403f964c1db4a94f773eb0ffbb4a";
      private static string s_urpResources    => "d4d516d2ff095af43bbfe6ec4d56dbc6";
      private static string s_urpExamples     => "c7ec736ae0e7b58458bbd7eaf9449255";
      private static string s_urp10Shaders    => "3df17cbe40235b742be8abb0bc570a5d";
      private static string s_urp10Materials  => "fe5c70387bc3068468f573724aef9b21";
      private static string s_urp10Resources  => "40b1dfda8a9edad45960b010ffa351d3";
      private static string s_urp10Examples   => "da61268984de6b244900a39cb5099df7";
      private static string s_hdrpShaders     => "bf0e14d6a0017fe4b970aecd24e83f56";
      private static string s_hdrpMaterials   => "d221572e98548184e99c925edc8b2ba0";
      private static string s_hdrpResources   => "869d66266dacf404194ab2fc481a6661";
      private static string s_hdrpExamples    => "1517e5b20f6e52447aa362359f4f8eb5";
      private static string s_hdrp10Shaders   => "824e90dbe63a37240a93af572453f7e8";
      private static string s_hdrp10Materials => "0bab1af1775a3f24292e5b92bc6ca9a3";
      private static string s_hdrp10Resources => "ffd9e1e538392ef458458075ffeba998";
      private static string s_hdrp10Examples  => "4ec9626ef9e669c43ad743308b8aa3d8";
    }

    public enum PackageImportTarget
    {
      Required, 
      Examples, 
    }

    private static readonly string RequiredPackagesLoadedKey = "RequiredPacakgesLoaded";
    private static readonly string LastLoadedRequiredPackagesRevisionKey = "LastLoadedRequiredPackagesRevision";
    private static readonly string ExamplesPackagesLoadedKey = "ExamplesPacakgesLoaded";
    private static readonly string LastLoadedExamplesPackagesRevisionKey = "LastLoadedExamplesPackagesRevision";

    static CompatibilityManager()
    {
      TryLoadRequiredPackages();
    }

    public static void TryLoadRequiredPackages()
    {
      if (Application.isPlaying)
        return;

      if (ProjectPrefs.GetInt(LastLoadedRequiredPackagesRevisionKey, -1) == MudBun.Revision)
        return;

      KickCompatibilityScan(PackageImportTarget.Required);
    }

    private enum SRPVersion
    {
      SRP_3_0_0  = 030000, 
      SRP_3_1_0  = 030100, 
      SRP_3_3_0  = 030300, 
      SRP_4_1_0  = 040100, 
      SRP_4_2_0  = 040200, 
      SRP_4_3_0  = 040300, 
      SRP_4_6_0  = 040600, 
      SRP_4_8_0  = 040800, 
      SRP_4_9_0  = 040900, 
      SRP_4_10_0 = 041000, 
      SRP_5_7_2  = 050702, 
      SRP_5_8_2  = 050802, 
      SRP_5_9_0  = 050900, 
      SRP_5_10_0 = 051000, 
      SRP_5_13_0 = 051300, 
      SRP_5_16_1 = 051601, 
      SRP_6_9_0  = 060900, 
      SRP_6_9_1  = 060901, 
      SRP_6_9_2  = 060902, 
      SRP_7_0_1  = 070001, 
      SRP_7_1_1  = 070101, 
      SRP_7_1_2  = 070102, 
      SRP_7_1_5  = 070105, 
      SRP_7_1_6  = 070106, 
      SRP_7_1_7  = 070107, 
      SRP_7_1_8  = 070108, 
      SRP_7_2_0  = 070200, 
      SRP_7_2_1  = 070201, 
      SRP_7_3_1  = 070301, 
      SRP_7_4_1  = 070401, 
      SRP_7_4_2  = 070402, 
      SRP_7_4_3  = 070403, 
      SRP_7_5_1  = 070501, 
      SRP_7_5_2  = 070502, 
      SRP_8_2_0  = 080200, 
      SRP_8_3_1  = 080301, 
      SRP_9_0_0  = 090000, 
      SRP_10_0_0 = 100000, 
      SRP_10_1_0 = 100100, 
      SRP_10_2_2 = 100202, 
      SRP_RECENT = 999999,
    }

    private static Dictionary<string, SRPVersion> s_srpVersionTable = new Dictionary<string, SRPVersion>()
    {
      { "3.0.0-preview",     SRPVersion.SRP_3_0_0  }, 
      { "3.1.0-preview",     SRPVersion.SRP_3_1_0  }, 
      { "3.3.0-preview",     SRPVersion.SRP_3_3_0  }, 
      { "4.1.0-preview",     SRPVersion.SRP_4_1_0  }, 
      { "4.2.0-preview",     SRPVersion.SRP_4_2_0  }, 
      { "4.3.0-preview",     SRPVersion.SRP_4_3_0  }, 
      { "4.6.0-preview",     SRPVersion.SRP_4_6_0  }, 
      { "4.8.0-preview",     SRPVersion.SRP_4_8_0  }, 
      { "4.9.0-preview",     SRPVersion.SRP_4_9_0  }, 
      { "4.10.0-preview",    SRPVersion.SRP_4_10_0 }, 
      { "5.7.2-preview",     SRPVersion.SRP_5_7_2  }, 
      { "5.7.2",             SRPVersion.SRP_5_7_2  }, 
      { "5.8.2-preview",     SRPVersion.SRP_5_8_2  }, 
      { "5.8.2",             SRPVersion.SRP_5_8_2  }, 
      { "5.9.0-preview",     SRPVersion.SRP_5_9_0  }, 
      { "5.9.0",             SRPVersion.SRP_5_9_0  }, 
      { "5.10.0-preview",    SRPVersion.SRP_5_10_0 }, 
      { "5.10.0",            SRPVersion.SRP_5_10_0 }, 
      { "5.13.0-preview",    SRPVersion.SRP_5_13_0 }, 
      { "5.13.0",            SRPVersion.SRP_5_13_0 }, 
      { "5.16.1-preview",    SRPVersion.SRP_5_16_1 }, 
      { "5.16.1",            SRPVersion.SRP_5_16_1 }, 
      { "6.9.0",             SRPVersion.SRP_6_9_0  }, 
      { "6.9.0-preview",     SRPVersion.SRP_6_9_0  }, 
      { "6.9.1",             SRPVersion.SRP_6_9_1  }, 
      { "6.9.1-preview",     SRPVersion.SRP_6_9_1  }, 
      { "6.9.2",             SRPVersion.SRP_6_9_2  }, 
      { "6.9.2-preview",     SRPVersion.SRP_6_9_2  }, 
      { "7.0.1",             SRPVersion.SRP_7_0_1  }, 
      { "7.0.1-preview",     SRPVersion.SRP_7_0_1  }, 
      { "7.1.1",             SRPVersion.SRP_7_1_1  }, 
      { "7.1.1-preview",     SRPVersion.SRP_7_1_1  }, 
      { "7.1.2",             SRPVersion.SRP_7_1_2  }, 
      { "7.1.2-preview",     SRPVersion.SRP_7_1_2  }, 
      { "7.1.5",             SRPVersion.SRP_7_1_5  }, 
      { "7.1.5-preview",     SRPVersion.SRP_7_1_5  }, 
      { "7.1.6",             SRPVersion.SRP_7_1_6  }, 
      { "7.1.6-preview",     SRPVersion.SRP_7_1_6  }, 
      { "7.1.7",             SRPVersion.SRP_7_1_7  }, 
      { "7.1.7-preview",     SRPVersion.SRP_7_1_7  }, 
      { "7.1.8",             SRPVersion.SRP_7_1_8  }, 
      { "7.1.8-preview",     SRPVersion.SRP_7_1_8  }, 
      { "7.2.0",             SRPVersion.SRP_7_2_0  }, 
      { "7.2.0-preview",     SRPVersion.SRP_7_2_0  }, 
      { "7.2.1",             SRPVersion.SRP_7_2_1  }, 
      { "7.2.1-preview",     SRPVersion.SRP_7_2_1  }, 
      { "7.3.1",             SRPVersion.SRP_7_3_1  }, 
      { "7.3.1-preview",     SRPVersion.SRP_7_3_1  }, 
      { "7.4.1",             SRPVersion.SRP_7_4_1  }, 
      { "7.4.1-preview",     SRPVersion.SRP_7_4_1  }, 
      { "7.4.2",             SRPVersion.SRP_7_4_2  }, 
      { "7.4.2-preview",     SRPVersion.SRP_7_4_2  }, 
      { "7.4.3",             SRPVersion.SRP_7_4_3  }, 
      { "7.4.3-preview",     SRPVersion.SRP_7_4_3  }, 
      { "7.5.1",             SRPVersion.SRP_7_5_1  }, 
      { "7.5.1-preview",     SRPVersion.SRP_7_5_1  }, 
      { "7.5.2",             SRPVersion.SRP_7_5_2  }, 
      { "7.5.2-preview",     SRPVersion.SRP_7_5_2  }, 
      { "8.2.0",             SRPVersion.SRP_8_2_0  }, 
      { "8.2.0-preview",     SRPVersion.SRP_8_2_0  }, 
      { "8.3.1",             SRPVersion.SRP_8_3_1  }, 
      { "8.3.1-preview",     SRPVersion.SRP_8_3_1  }, 
      { "9.0.0",             SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.13",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.14",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.33",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.35",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.54",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.55",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.71",  SRPVersion.SRP_9_0_0  }, 
      { "10.0.0-preview.26", SRPVersion.SRP_10_0_0 }, 
      { "10.0.0-preview.27", SRPVersion.SRP_10_0_0 }, 
      { "10.1.0",            SRPVersion.SRP_10_1_0 }, 
      { "10.2.2",            SRPVersion.SRP_10_2_2 }, 
    };

    private class CompatibilitySpec
    {
      public ResourcesUtil.RenderPipelineEnum RenderPipeline;
      public string RenderPipelineRecordName;
      public string RenderPipelinePackageId;
      public string[] RequiredPackagesBase;
      public string[] ExamplesPackagesBase;
      public Dictionary<SRPVersion, string[]> RequiredPackagesUpgrade;
      public Dictionary<SRPVersion, string[]> ExamplesPackagesUpgrade;
    }

    private static CompatibilitySpec[] s_aCompatibilitySpec = 
    {
      new CompatibilitySpec()
      {
        RenderPipeline = ResourcesUtil.RenderPipelineEnum.BuiltIn, 
        RenderPipelineRecordName = "BRP", 
        RenderPipelinePackageId = "", 
        RequiredPackagesBase = PackageGuidSet.Brp, 
        ExamplesPackagesBase = PackageGuidSet.BrpExamples, 
        RequiredPackagesUpgrade = new Dictionary<SRPVersion, string[]>()
        {
          // empty
        },
        ExamplesPackagesUpgrade = new Dictionary<SRPVersion, string[]>()
        {
          // empty
        }, 
      }, 
      new CompatibilitySpec()
      {
        RenderPipeline = ResourcesUtil.RenderPipelineEnum.URP,
        RenderPipelineRecordName = "URP",
        RenderPipelinePackageId = "com.unity.render-pipelines.universal", 
        RequiredPackagesBase = PackageGuidSet.Urp, 
        ExamplesPackagesBase = PackageGuidSet.UrpExamples, 
        RequiredPackagesUpgrade = new Dictionary<SRPVersion, string[]>()
        {
          { SRPVersion.SRP_10_0_0, PackageGuidSet.Urp10 }, 
          { SRPVersion.SRP_10_1_0, PackageGuidSet.Urp10 }, 
          { SRPVersion.SRP_10_2_2, PackageGuidSet.Urp10 }, 
          { SRPVersion.SRP_RECENT, PackageGuidSet.Urp10 }, 
        },
        ExamplesPackagesUpgrade = new Dictionary<SRPVersion, string[]>()
        {
          { SRPVersion.SRP_10_0_0, PackageGuidSet.Urp10Examples }, 
          { SRPVersion.SRP_10_1_0, PackageGuidSet.Urp10Examples }, 
          { SRPVersion.SRP_10_2_2, PackageGuidSet.Urp10Examples }, 
          { SRPVersion.SRP_RECENT, PackageGuidSet.Urp10Examples }, 
        }, 
      }, 
      new CompatibilitySpec()
      {
        RenderPipeline = ResourcesUtil.RenderPipelineEnum.HDRP,
        RenderPipelineRecordName = "HDRP",
        RenderPipelinePackageId = "com.unity.render-pipelines.high-definition", 
        RequiredPackagesBase = PackageGuidSet.Hdrp, 
        ExamplesPackagesBase = PackageGuidSet.HdrpExamples, 
        RequiredPackagesUpgrade = new Dictionary<SRPVersion, string[]>()
        {
          { SRPVersion.SRP_10_0_0, PackageGuidSet.Hdrp10 }, 
          { SRPVersion.SRP_10_1_0, PackageGuidSet.Hdrp10 }, 
          { SRPVersion.SRP_10_2_2, PackageGuidSet.Hdrp10 }, 
          { SRPVersion.SRP_RECENT, PackageGuidSet.Hdrp10 }, 
        },
        ExamplesPackagesUpgrade = new Dictionary<SRPVersion, string[]>()
        {
          { SRPVersion.SRP_10_0_0, PackageGuidSet.Hdrp10Examples }, 
          { SRPVersion.SRP_10_1_0, PackageGuidSet.Hdrp10Examples }, 
          { SRPVersion.SRP_10_2_2, PackageGuidSet.Hdrp10Examples }, 
          { SRPVersion.SRP_RECENT, PackageGuidSet.Hdrp10Examples }, 
        }, 
      }, 
    };

    private static bool s_loadingPackageList = false;
    private static ListRequest s_packageListRequest;
    private static PackageImportTarget s_importTarget;

    // kick-starts the entire process of compatibility check
    public static void KickCompatibilityScan(PackageImportTarget target)
    {
      if (s_loadingPackageList)
        return;

      EditorApplication.update += CompatibilityScanLoop;
      s_loadingPackageList = true;
      s_importTarget = target;
    }

    private static void CompatibilityScanLoop()
    {
      if (s_packageListRequest == null)
      {
        s_packageListRequest = UnityEditor.PackageManager.Client.List(true);
      }

      if (s_packageListRequest == null)
      {
        EditorApplication.update -= CompatibilityScanLoop;
        s_loadingPackageList = false;
        return;
      }

      if (!s_packageListRequest.IsCompleted)
        return;

      EditorApplication.update -= CompatibilityScanLoop;
      s_loadingPackageList = false;

      if (!Application.isPlaying)
      {
        ImportPackages(s_importTarget);
      }
    }

    private static bool ImportPackages(PackageImportTarget target)
    {
      SRPVersion version = SRPVersion.SRP_RECENT;
      string versionString = "";

      ResourcesUtil.DetermineRenderPipeline();
      UpdateDefines();

      var spec = s_aCompatibilitySpec.FirstOrDefault(x => x.RenderPipeline == ResourcesUtil.RenderPipeline);
      if (spec == null)
        return false;

      if (spec.RenderPipeline != ResourcesUtil.RenderPipelineEnum.BuiltIn)
      {
        var rpPackageInfo = s_packageListRequest.Result.FirstOrDefault(x => x.name.Equals(spec.RenderPipelinePackageId));
        if (rpPackageInfo == null)
        {
          Debug.LogError($"MudBun: Detected {spec.RenderPipeline.ToString()} package \"{spec.RenderPipelinePackageId}\" but not {spec.RenderPipeline.ToString()} render asset. Has {spec.RenderPipeline.ToString()} been imported & set up correctly?");
          return false;
        }

        versionString = rpPackageInfo.version;
        if (s_srpVersionTable.ContainsKey(versionString))
          version = s_srpVersionTable[versionString];
      }

      string lastLoadedRevisionKey = "";
      string packagesLoadedKey = "";
      string packagesLoadedValue = "";
      switch (target)
      {
        case PackageImportTarget.Required:
          foreach (string packageGuid in spec.RequiredPackagesBase)
          {
            ImportPackage(spec, packageGuid);
          }
          if (spec.RequiredPackagesUpgrade.ContainsKey(version))
          {
            foreach (string packageGuid in spec.RequiredPackagesUpgrade[version])
            {
              ImportPackage(spec, packageGuid);
            }
          }
          lastLoadedRevisionKey = LastLoadedRequiredPackagesRevisionKey;
          packagesLoadedKey = RequiredPackagesLoadedKey;
          packagesLoadedValue = spec.RenderPipelineRecordName;
          break;

        case PackageImportTarget.Examples:
          foreach (string packageGuid in spec.ExamplesPackagesBase)
          {
            ImportPackage(spec, packageGuid);
          }
          if (spec.RequiredPackagesUpgrade.ContainsKey(version))
          {
            foreach (string packageGuid in spec.ExamplesPackagesUpgrade[version])
            {
              ImportPackage(spec, packageGuid);
            }
          }
          lastLoadedRevisionKey = LastLoadedExamplesPackagesRevisionKey;
          packagesLoadedKey = ExamplesPackagesLoadedKey;
          packagesLoadedValue = spec.RenderPipelineRecordName;
          break;
      }
      if (!string.IsNullOrEmpty(versionString))
        packagesLoadedValue += $"-{versionString}";
#if !MUDBUN_DEV
      if (!string.IsNullOrEmpty(lastLoadedRevisionKey))
        ProjectPrefs.SetInt(lastLoadedRevisionKey, MudBun.Revision);

      if (!string.IsNullOrEmpty(packagesLoadedKey))
        ProjectPrefs.AddToSet(packagesLoadedKey, packagesLoadedValue);
#endif

      return true;
    }

    private static void ImportPackage(CompatibilitySpec spec, string guid)
    {
      string packagePath = AssetDatabase.GUIDToAssetPath(guid);
      if (packagePath.Equals("") || !File.Exists(packagePath))
      {
        Debug.LogError($"Compatibility package \"{packagePath}\" not found for {spec.RenderPipeline.ToString()}.\n Did you forget to import the MudBun/Compatibility folder?");
        return;
      }

#if MUDBUN_DEV
      Debug.Log($"MudBun (DEV): Retail builds would have imported package \"{packagePath}\"");
#else
      AssetDatabase.ImportPackage(packagePath, false);
      MudRendererBase.ReloadAllShaders();
#endif
    }

    private static void UpdateDefines()
    {
      var currentRp = (int) ResourcesUtil.DetermineRenderPipeline();
      var aRpDef = new string[]
      {
        "MUDBUN_BUILTIN_RP", 
        "MUDBUN_URP", 
        "MUDUBN_HDRP", 
      };
      int numRps = (int) ResourcesUtil.RenderPipelineEnum.Count;
      Assert.True(aRpDef.Length == numRps, "Mismatched known render pipeline count");

      AddDefine("MUDBUN");

      for (int rp = 0; rp < numRps; ++rp)
      {
        if (rp == currentRp)
          AddDefine(aRpDef[rp]);
        else
          RemoveDefine(aRpDef[rp]);
      }
    }

    private static void AddDefine(string def)
    {
      string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      if (defines.Contains($"{def};"))
        return;

      PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, $"{def};{defines}");
    }

    private static void RemoveDefine(string def)
    {
      string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      if (defines.Contains($"{def};"))
        return;

      PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines.Replace($"{def};", ""));
    }
  }
}

