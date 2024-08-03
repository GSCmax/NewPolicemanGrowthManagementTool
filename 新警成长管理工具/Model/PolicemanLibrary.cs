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
        public BindingList<SinglePoliceman> PolicemanList { get; set; } = [
            new SinglePoliceman(){
                PolicemanName = "示例警员",
                PolicemanDegree = "本科生",
                PolicemanNo = "123456",
                PolicemanIDNo = "320481200001011234",
                PolicemanYear = "2024",
                PolicemanSource = "警校",
                PolicemanAddr = "江苏省溧阳市xxx小区",
                PolicemanReward = [
                    new SingleRewardOrPunish4Policeman(){
                        RewardOrPunishID = GlobalDataHelper.appConfig!.BePolicemanRewardID,
                        AddAdmin = "SYSTEM",
                        AddTime = DateTime.Now
                    }
                ],
            }
        ];

        [JsonIgnore]
        public BindingList<string> PolicemanMasters { get; set; } = [];
    }
}
