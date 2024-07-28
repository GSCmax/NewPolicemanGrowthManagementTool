using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using 新警成长管理工具.Model;

namespace 新警成长管理工具.Tools
{
    static class GlobalDataHelper
    {
        /// <summary>
        /// 存储当前App实例的版本信息
        /// </summary>
        public static string appVersion = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();

        /// <summary>
        /// 存储当前App实例的配置信息
        /// </summary>
        public static AppConfig? appConfig;

        /// <summary>
        /// 存储当前App实例的新警库
        /// </summary>
        public static PolicemanLibrary? policemanLibrary;

        /// <summary>
        /// 存储当前App实例的奖惩库
        /// </summary>
        public static RewardANDPunishLibrary? rewardANDPunishLibrary;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if (File.Exists(AppConfig.SavePath))
                try
                {
                    var json = File.ReadAllText(AppConfig.SavePath);
                    appConfig = (string.IsNullOrEmpty(json) ? new AppConfig() : JsonConvert.DeserializeObject<AppConfig>(json)) ?? new AppConfig();
                }
                catch
                {
                    appConfig = new AppConfig();
                }
            else
                appConfig = new AppConfig();

            if (File.Exists(RewardANDPunishLibrary.SavePath))
                try
                {
                    var json = File.ReadAllText(RewardANDPunishLibrary.SavePath);
                    rewardANDPunishLibrary = (string.IsNullOrEmpty(json) ? new RewardANDPunishLibrary() : JsonConvert.DeserializeObject<RewardANDPunishLibrary>(json)) ?? new RewardANDPunishLibrary();
                }
                catch
                {
                    rewardANDPunishLibrary = new RewardANDPunishLibrary();
                }
            else
                rewardANDPunishLibrary = new RewardANDPunishLibrary();

            if (File.Exists(PolicemanLibrary.SavePath))
                try
                {
                    var json = File.ReadAllText(PolicemanLibrary.SavePath);
                    policemanLibrary = (string.IsNullOrEmpty(json) ? new PolicemanLibrary() : JsonConvert.DeserializeObject<PolicemanLibrary>(json)) ?? new PolicemanLibrary();
                }
                catch
                {
                    policemanLibrary = new PolicemanLibrary();
                }
            else
                policemanLibrary = new PolicemanLibrary();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void Save()
        {
            var json1 = JsonConvert.SerializeObject(appConfig, Formatting.Indented);
            File.WriteAllText(AppConfig.SavePath, json1);

            var json2 = JsonConvert.SerializeObject(policemanLibrary, Formatting.Indented);
            File.WriteAllText(PolicemanLibrary.SavePath, json2);

            var json3 = JsonConvert.SerializeObject(rewardANDPunishLibrary, Formatting.Indented);
            File.WriteAllText(RewardANDPunishLibrary.SavePath, json3);
        }

        /// <summary>
        /// 更新警师列表
        /// </summary>
        public static void UpdatePolicemanMasters()
        {
            var temp1 = policemanLibrary!.PolicemanList.Where(a => a.CanBePolicemanMaster == true);
            List<string> temp2 = [];
            foreach (var a in temp1)
            {
                string PolicemanMasterItem = $"{a.PolicemanName}（{a.PolicemanNo}）";
                temp2.Add(PolicemanMasterItem);

                //计算得分
                double s = 0;
                foreach (var pm in policemanLibrary!.PolicemanList.Where(b => b.PolicemanMaster == PolicemanMasterItem))
                    s += pm.PolicemanScore;
                a.ScoreFromApprentice = s;
            }

            //添加警师
            foreach (var a in temp2)
                if (!(policemanLibrary!.PolicemanMasters.Contains(a)))
                    policemanLibrary!.PolicemanMasters.Add(a);

            //删除警师
            List<string> temp3 = [];
            foreach (var a in policemanLibrary!.PolicemanMasters)
                if (!(temp2.Contains(a)))
                    temp3.Add(a);
            foreach (var a in temp3)
                policemanLibrary!.PolicemanMasters.Remove(a);

            //添加一个空项
            policemanLibrary!.PolicemanMasters.Add("");
        }
    }
}
