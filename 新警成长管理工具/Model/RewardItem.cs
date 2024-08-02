using CommunityToolkit.Mvvm.ComponentModel;

namespace 新警成长管理工具.Model
{
    internal partial class RewardItem : ObservableObject
    {
        [ObservableProperty]
        private Guid rewardID = Guid.NewGuid();

        [ObservableProperty]
        private string rewardName = "新奖励";

        [ObservableProperty]
        private double rewardScore = 1.0;

        [ObservableProperty]
        private string rewardANDPunishCategory = "基础";
    }
}
