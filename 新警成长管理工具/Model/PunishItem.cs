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
                    if (item1.PolicemanPunish.Contains(oldValue))
                    {
                        item1.PolicemanPunish.Remove(oldValue);
                        item1.PolicemanPunish.Add(newValue);
                    }
                }
            }
        }

        [ObservableProperty]
        private double punishScore = 1.0;
    }
}
