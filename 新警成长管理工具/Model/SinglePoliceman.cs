using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.Model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal partial class SinglePoliceman : ObservableValidator
    {
        public SinglePoliceman()
        {
            PolicemanReward.ListChanged += (s, e) => OnPropertyChanged(nameof(PolicemanScore));
            PolicemanPunish.ListChanged += (s, e) => OnPropertyChanged(nameof(PolicemanScore));
        }

        [ObservableProperty]
        [property: JsonProperty]
        private string policemanName = "新警姓名";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PolicemanAge))]
        [NotifyPropertyChangedFor(nameof(PolicemanSex))]
        [RegularExpression("^[1-9]\\d{5}(18|19|20)\\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\\d{3}[0-9Xx]$")]
        [property: JsonProperty]
        private string policemanIDNo = "新警证件";
        partial void OnPolicemanIDNoChanged(string value)
        {
            ValidateAllProperties();
        }

        [ObservableProperty]
        [property: JsonProperty]
        private string policemanAddr = "新警住址";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PolicemanAge))]
        [NotifyPropertyChangedFor(nameof(PolicemanSex))]
        [RegularExpression("^\\d{6}$")]
        [property: JsonProperty]
        private string policemanNo = "000000";
        partial void OnPolicemanNoChanged(string value)
        {
            ValidateAllProperties();
        }

        public string PolicemanSex => UpdateSex();
        private string UpdateSex()
        {
            if (HasErrors)
                return "未知";
            else
                return (int.Parse(PolicemanIDNo.Substring(16, 1)) % 2 == 1) ? "男" : "女";
        }

        public int PolicemanAge => UpdateAge();
        private int UpdateAge()
        {
            if (HasErrors)
                return 0;
            else
                return DateTime.Now.Year - int.Parse(PolicemanIDNo.Substring(6, 4));
        }

        public double PolicemanScore => UpdateScore();
        private double UpdateScore()
        {
            double a = 0;
            double b = 0;
            foreach (string r in PolicemanReward)
                a += GlobalDataHelper.rewardANDPunishLibrary!.RewardItems.FirstOrDefault(i => i.RewardName == r)!.RewardScore;
            foreach (string p in PolicemanPunish)
                b += GlobalDataHelper.rewardANDPunishLibrary!.PunishItems.FirstOrDefault(i => i.PunishName == p)!.PunishScore;
            return a - b;
        }

        [JsonProperty]
        public BindingList<string> PolicemanReward { get; set; } = [];

        [JsonProperty]
        public BindingList<string> PolicemanPunish { get; set; } = [];
    }
}
