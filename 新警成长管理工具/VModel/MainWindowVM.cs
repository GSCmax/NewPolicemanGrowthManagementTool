using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using 新警成长管理工具.Model;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.VModel
{
    internal partial class MainWindowVM : ObservableObject
    {
        #region 登录页
        [ObservableProperty]
        private string userName = "";

        [ObservableProperty]
        private string userPassword = "";

        [ObservableProperty]
        private bool loginSuccess = false;

        [RelayCommand]
        private void Login()
        {
            if (UserName == GlobalDataHelper.appConfig!.User && UserPassword == GlobalDataHelper.appConfig!.Password)
                LoginSuccess = true;
        }
        #endregion

        #region 奖惩库
        [RelayCommand]
        private void RDel(RewardItem r)
        {
            foreach (var t in GlobalDataHelper.policemanLibrary!.PolicemanList)
            {
                var temp = t.PolicemanReward.FirstOrDefault(a => a.RewardOrPunishName == r.RewardName);
                if (temp != null)
                    t.PolicemanReward.Remove(temp);
            }

            GlobalDataHelper.rewardANDPunishLibrary!.RewardItems.Remove(r);
        }

        [RelayCommand]
        private void PDel(PunishItem p)
        {
            foreach (var t in GlobalDataHelper.policemanLibrary!.PolicemanList)
            {
                var temp = t.PolicemanPunish.FirstOrDefault(a => a.RewardOrPunishName == p.PunishName);
                if (temp != null)
                    t.PolicemanPunish.Remove(temp);
            }

            GlobalDataHelper.rewardANDPunishLibrary!.PunishItems.Remove(p);
        }
        #endregion

        #region 新警库
        [ObservableProperty]
        private SinglePoliceman? sp = null;
        partial void OnSpChanged(SinglePoliceman? value)
        {
            UpdateTree();
        }

        [RelayCommand]
        private void UpdateTree()
        {
            if (Sp != null)
            {
                Branches.Clear();
                DrawTree(200, 380, -90, 100, Sp!.PolicemanScore * 0.1, Sp!.PolicemanReward.Count);
            }
        }

        [ObservableProperty]
        private RewardItem? selectR = null;

        [RelayCommand]
        private void RAdd()
        {
            if (Sp != null && SelectR != null)
            {
                var temp = Sp.PolicemanReward.FirstOrDefault(a => a.RewardOrPunishName == SelectR.RewardName);
                if (temp == null)
                {
                    Sp.PolicemanReward.Add(new SingleRewardOrPunish4Policeman()
                    {
                        RewardOrPunishName = SelectR.RewardName,
                        AddAdmin = UserName,
                        AddTime = DateTime.Now,
                    });
                }
            }
        }

        [ObservableProperty]
        private PunishItem? selectP = null;

        [RelayCommand]
        private void PAdd()
        {
            if (Sp != null && SelectP != null)
            {
                var temp = Sp.PolicemanPunish.FirstOrDefault(a => a.RewardOrPunishName == SelectP.PunishName);
                if (temp == null)
                {
                    Sp.PolicemanPunish.Add(new SingleRewardOrPunish4Policeman()
                    {
                        RewardOrPunishName = SelectP.PunishName,
                        AddAdmin = UserName,
                        AddTime = DateTime.Now,
                    });
                }
            }
        }

        public BindingList<Branch> Branches { get; set; } = [];

        private void DrawTree(double x1, double y1, double angle, double length, double thickness, int depth)
        {
            if (depth == 0)
                return;

            double x2 = x1 + (Math.Cos(angle * Math.PI / 180) * length);
            double y2 = y1 + (Math.Sin(angle * Math.PI / 180) * length);

            Branches.Add(new Branch
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Thickness = thickness
            });

            DrawTree(x2, y2, angle - 25, length * 0.8, thickness * 0.8, depth - 1);
            DrawTree(x2, y2, angle + 25, length * 0.8, thickness * 0.8, depth - 1);
        }
        #endregion
    }
}
