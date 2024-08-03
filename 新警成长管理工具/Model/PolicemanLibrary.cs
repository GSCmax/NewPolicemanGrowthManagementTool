using Newtonsoft.Json;
using System.ComponentModel;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.Model
{
    internal class PolicemanLibrary
    {
        /// <summary>
        /// 新警库文件存储路径
        /// </summary>
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}PolicemanLibrary.json";

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BindingList<SinglePoliceman> PolicemanList { get; set; } = [new SinglePoliceman() {
            PolicemanReward = {
                new SingleRewardOrPunish4Policeman() {
                    RewardOrPunishID = GlobalDataHelper.appConfig!.BePolicemanRewardID,
                    AddAdmin = "SYSTEM",
                    AddTime = DateTime.Now
                }
            }
        }];

        [JsonIgnore]
        public BindingList<string> PolicemanMasters { get; set; } = [];
    }
}
