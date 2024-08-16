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
            _policemanReward.ListChanged += PolicemanReward_ListChanged;
            _policemanPunish.ListChanged += PolicemanPunish_ListChanged;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PolicemanNameWithNo))]
        [property: JsonProperty]
        private string policemanName = "警员姓名";

        /// <summary>
        /// 身份证号
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PolicemanAge))]
        [NotifyPropertyChangedFor(nameof(PolicemanSex))]
        [RegularExpression("^[1-9]\\d{5}(18|19|20)\\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\\d{3}[0-9Xx]$")]
        [property: JsonProperty]
        private string policemanIDNo = "320481200001011234";
        partial void OnPolicemanIDNoChanged(string value)
        {
            ValidateAllProperties();
        }

        /// <summary>
        /// 家庭住址
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string? policemanAddr = "警员住址";

        /// <summary>
        /// 警号
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PolicemanNameWithNo))]
        [RegularExpression("^\\d{6}$")]
        [property: JsonProperty]
        private string policemanNo = "123456";
        partial void OnPolicemanNoChanged(string value)
        {
            ValidateAllProperties();
        }

        /// <summary>
        /// 姓名（警号）（自动）
        /// </summary>
        public string PolicemanNameWithNo => $"{PolicemanName}（{PolicemanNo}）";

        /// <summary>
        /// 入警时间
        /// </summary>
        [ObservableProperty]
        [RegularExpression("^\\d{4}$")]
        [property: JsonProperty]
        private string? policemanYear = "2024";

        /// <summary>
        /// 入警途径
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string? policemanSource = "警校";

        /// <summary>
        /// 学历
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string? policemanDegree = "本科生";

        /// <summary>
        /// 性别（自动）
        /// </summary>
        public string PolicemanSex => UpdateSex();
        private string UpdateSex()
        {
            if (HasErrors)
                return "";
            else
                return (int.Parse(PolicemanIDNo.Substring(16, 1)) % 2 == 1) ? "男" : "女";
        }

        /// <summary>
        /// 年龄（自动）
        /// </summary>
        public int PolicemanAge => UpdateAge();
        private int UpdateAge()
        {
            if (HasErrors)
                return 0;
            else
                return DateTime.Now.Year - int.Parse(PolicemanIDNo.Substring(6, 4));
        }

        /// <summary>
        /// 来自徒弟的积分
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PolicemanScore))]
        private double scoreFromApprentice;

        /// <summary>
        /// 成长值（自动）
        /// </summary>
        public double PolicemanScore => UpdateScore();
        private double UpdateScore()
        {
            double a = 0;
            double b = 0;
            foreach (var r in PolicemanReward)
                a += GlobalDataHelper.rewardANDPunishLibrary!.RewardItems.FirstOrDefault(t => t.RewardID == r.RewardOrPunishID)!.RewardScore;
            foreach (var p in PolicemanPunish)
                b += GlobalDataHelper.rewardANDPunishLibrary!.PunishItems.FirstOrDefault(t => t.PunishID == p.RewardOrPunishID)!.PunishScore;
            //来自徒弟的积分
            return a - b + ScoreFromApprentice;
        }

        /// <summary>
        /// 中共党员（自动）
        /// </summary>
        public string? IfCommunist => UpdateCommunist();
        private string UpdateCommunist()
        {
            var temp = PolicemanReward.FirstOrDefault(a => a.RewardOrPunishID == GlobalDataHelper.appConfig!.CommunistRewardID);
            if (temp != null)
                return "是";
            else
                return "否";
        }

        /// <summary>
        /// 警师标记
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private bool canBePolicemanMaster = false;
        partial void OnCanBePolicemanMasterChanged(bool value)
        {
            if (!value)
                ScoreFromApprentice = 0;
        }

        /// <summary>
        /// 师承（所队干部）
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string policemanMaster_A = "";

        /// <summary>
        /// 师承（业务骨干）
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string policemanMaster_B = "";

        /// <summary>
        /// 任职单位
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string policemanWorkUnit = "警员单位";

        /// <summary>
        /// 奖励列表
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BindingList<SingleRewardOrPunish4Policeman> PolicemanReward
        {
            get => _policemanReward;
            set
            {
                if (_policemanReward != null)
                    _policemanReward.ListChanged -= PolicemanReward_ListChanged;
                _policemanReward = value;
                if (_policemanReward != null)
                    _policemanReward.ListChanged += PolicemanReward_ListChanged;
            }
        }
        private BindingList<SingleRewardOrPunish4Policeman> _policemanReward = [new SingleRewardOrPunish4Policeman() { RewardOrPunishID = GlobalDataHelper.appConfig!.BePolicemanRewardID, AddAdmin = "SYSTEM", AddTime = DateTime.Now }];

        private void PolicemanReward_ListChanged(object? sender, ListChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PolicemanScore));
            OnPropertyChanged(nameof(IfCommunist));
        }

        /// <summary>
        /// 惩罚列表
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BindingList<SingleRewardOrPunish4Policeman> PolicemanPunish
        {
            get => _policemanPunish;
            set
            {
                if (_policemanPunish != null)
                    _policemanPunish.ListChanged -= PolicemanPunish_ListChanged;
                _policemanPunish = value;
                if (_policemanPunish != null)
                    _policemanPunish.ListChanged += PolicemanPunish_ListChanged;
            }
        }
        private BindingList<SingleRewardOrPunish4Policeman> _policemanPunish = [];

        private void PolicemanPunish_ListChanged(object? sender, ListChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PolicemanScore));
        }
    }

    internal partial class SingleRewardOrPunish4Policeman : ObservableObject
    {
        [ObservableProperty]
        private Guid rewardOrPunishID;

        [ObservableProperty]
        private DateTime addTime;

        [ObservableProperty]
        private string addAdmin = "EMPTY";
    }
}
