using CommunityToolkit.Mvvm.ComponentModel;

namespace 新警成长管理工具.Model
{
    internal partial class PunishItem : ObservableObject
    {
        [ObservableProperty]
        private Guid punishID = Guid.NewGuid();

        [ObservableProperty]
        private string punishName = "新惩罚";

        [ObservableProperty]
        private double punishScore = 1.0;

        [ObservableProperty]
        private string rewardANDPunishCategory = "基础";

        public override string ToString() => $"{PunishName} [{RewardANDPunishCategory}] (-{PunishScore})";
    }
}
