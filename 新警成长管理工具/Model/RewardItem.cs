using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace 新警成长管理工具.Model
{
    internal partial class RewardItem : ObservableObject
    {
        [ObservableProperty]
        private Guid rewardID = Guid.NewGuid();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomToString))]
        private string rewardName = "新奖励";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomToString))]
        private double rewardScore = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomToString))]
        private string rewardANDPunishCategory = "基础";

        [JsonIgnore]
        public string CustomToString => $"{RewardName} [{RewardANDPunishCategory}] ({RewardScore})";
    }
}
