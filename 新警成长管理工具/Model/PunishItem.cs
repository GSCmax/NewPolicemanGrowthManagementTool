using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace 新警成长管理工具.Model
{
    internal partial class PunishItem : ObservableObject
    {
        [ObservableProperty]
        private Guid punishID = Guid.NewGuid();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomToString))]
        private string punishName = "新惩罚";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomToString))]
        private double punishScore = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomToString))]
        private string rewardANDPunishCategory = "基础";

        [JsonIgnore]
        public string CustomToString => $"{PunishName} [{RewardANDPunishCategory}] (-{PunishScore})";
    }
}
