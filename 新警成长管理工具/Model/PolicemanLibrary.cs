using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.Model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal partial class PolicemanLibrary : ObservableObject
    {
        public PolicemanLibrary()
        {
            _policemanList.ListChanged += PolicemanList_ListChanged;
        }

        /// <summary>
        /// 新警库文件存储路径
        /// </summary>
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}PolicemanLibrary.json";

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BindingList<SinglePoliceman> PolicemanList
        {
            get => _policemanList;
            set
            {
                if (_policemanList != null)
                    _policemanList.ListChanged -= PolicemanList_ListChanged;
                _policemanList = value;
                if (_policemanList != null)
                    _policemanList.ListChanged += PolicemanList_ListChanged;
            }
        }

        private BindingList<SinglePoliceman> _policemanList = [new SinglePoliceman()];

        private void PolicemanList_ListChanged(object? sender, ListChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PolicemanMasters));
        }

        public BindingList<string> PolicemanMasters => UpdatePolicemanMasters();

        private BindingList<string> UpdatePolicemanMasters()
        {
            var temp1 = PolicemanList.Where(a => a.CanBePolicemanMaster == true);
            BindingList<string> temp2 = [];
            foreach (var a in temp1)
            {
                string PolicemanMasterItem = a.PolicemanNameWithNo;
                temp2.Add(PolicemanMasterItem);

                //计算得分
                double s = 0;
                foreach (var pm in PolicemanList.Where(b => b.PolicemanMaster == PolicemanMasterItem))
                    s += pm.PolicemanScore;
                a.ScoreFromApprentice = s * GlobalDataHelper.appConfig!.ScoreComeByApprenticeCoefficient;
            }
            temp2.Add("");
            return temp2;
        }
    }
}
