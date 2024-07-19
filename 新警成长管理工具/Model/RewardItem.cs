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
                    var temp = item1.PolicemanReward.FirstOrDefault(a => a.RewardOrPunishName == oldValue);
                    if (temp != null)
                    {
                        item1.PolicemanReward.Remove(temp);
                        item1.PolicemanReward.Add(new SingleRewardOrPunish4Policeman()
                        {
                            RewardOrPunishName = newValue,
                            AddAdmin = temp.AddAdmin,
                            AddTime = temp.AddTime,
                        });
                    }
                }
            }
        }

        [ObservableProperty]
        private double rewardScore = 1.0;
    }
}
