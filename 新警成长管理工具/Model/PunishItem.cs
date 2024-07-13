using CommunityToolkit.Mvvm.ComponentModel;

namespace 新警成长管理工具.Model
{
    internal partial class PunishItem : ObservableObject
    {
        [ObservableProperty]
        private string punishName = "新惩罚";

        [ObservableProperty]
        private double punishScore = 1.0;
    }
}
