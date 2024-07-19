using CommunityToolkit.Mvvm.ComponentModel;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.Model
{
    internal partial class RewardItem : ObservableObject
    {
        [ObservableProperty]
        private string rewardName = "新奖励";
        partial void OnRewardNameChanged(string? oldValue, string newValue)
        {
            if (oldValue != null && newValue != null)
            {
                foreach (var item1 in GlobalDataHelper.policemanLibrary!.PolicemanList)
                {
                    if (item1.PolicemanReward.Contains(oldValue))
                    {
                        item1.PolicemanReward.Remove(oldValue);
                        item1.PolicemanReward.Add(newValue);
                    }
                }
            }
        }

        [ObservableProperty]
        private double rewardScore = 1.0;
    }
}
