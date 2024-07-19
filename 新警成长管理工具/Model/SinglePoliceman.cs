﻿using CommunityToolkit.Mvvm.ComponentModel;
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

        /// <summary>
        /// 姓名
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string policemanName = "新警姓名";

        /// <summary>
        /// 身份证号
        /// </summary>
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

        /// <summary>
        /// 家庭住址
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string policemanAddr = "新警住址";

        /// <summary>
        /// 警号
        /// </summary>
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

        /// <summary>
        /// 入警时间
        /// </summary>
        [ObservableProperty]
        [RegularExpression("^\\d{4}$")]
        [property: JsonProperty]
        private string policemanYear = "2000";

        /// <summary>
        /// 入警途径
        /// </summary>
        [ObservableProperty]
        [property: JsonProperty]
        private string policemanSource = "";

        /// <summary>
        /// 性别（自动）
        /// </summary>
        public string PolicemanSex => UpdateSex();
        private string UpdateSex()
        {
            if (HasErrors)
                return "未知";
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
        /// 成长值（自动）
        /// </summary>
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

        /// <summary>
        /// 奖励列表
        /// </summary>
        [JsonProperty]
        public BindingList<string> PolicemanReward { get; set; } = [];

        /// <summary>
        /// 惩罚列表
        /// </summary>
        [JsonProperty]
        public BindingList<string> PolicemanPunish { get; set; } = [];
    }
}
