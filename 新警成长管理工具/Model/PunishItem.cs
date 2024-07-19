using CommunityToolkit.Mvvm.ComponentModel;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.Model
{
    internal partial class PunishItem : ObservableObject
    {
        [ObservableProperty]
        private string punishName = "新惩罚";
        partial void OnPunishNameChanged(string? oldValue, string newValue)
        {
            if (oldValue != null && newValue != null)
            {
                foreach (var item1 in GlobalDataHelper.policemanLibrary!.PolicemanList)
                {
                    var temp = item1.PolicemanPunish.FirstOrDefault(a => a.RewardOrPunishName == oldValue);
                    if (temp != null)
                    {
                        item1.PolicemanPunish.Remove(temp);
                        item1.PolicemanPunish.Add(new SingleRewardOrPunish4Policeman()
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
        private double punishScore = 1.0;
    }
}
