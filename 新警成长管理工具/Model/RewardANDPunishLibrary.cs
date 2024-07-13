using System.ComponentModel;

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
        public BindingList<RewardItem> RewardItems { get; set; } = [];

        /// <summary>
        /// 惩罚库
        /// </summary>
        public BindingList<PunishItem> PunishItems { get; set; } = [];
    }
}
