using Newtonsoft.Json;
using System.ComponentModel;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.Model
{
    internal class RewardANDPunishLibrary
    {
        /// <summary>
        /// 奖惩库文件存储路径
        /// </summary>
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}RewardANDPunishLibrary.json";

        /// <summary>
        /// 奖励库
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BindingList<RewardItem> RewardItems { get; set; } = [
            new RewardItem() {
                RewardANDPunishCategory = GlobalDataHelper.appConfig!.RewardANDPunishCategory.FirstOrDefault(),
                RewardName = "光荣从警",
                RewardScore = 50,
                RewardID = GlobalDataHelper.appConfig!.BePolicemanRewardID
            },
            new RewardItem() {
                RewardANDPunishCategory = GlobalDataHelper.appConfig!.RewardANDPunishCategory.FirstOrDefault(),
                RewardName = "中共党员",
                RewardScore = 30,
                RewardID = GlobalDataHelper.appConfig!.CommunistRewardID
            }
        ];

        /// <summary>
        /// 惩罚库
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BindingList<PunishItem> PunishItems { get; set; } = [];
    }
}
