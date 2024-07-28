﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExcelDataReader;
using HandyControl.Controls;
using Microsoft.Win32;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using 新警成长管理工具.Model;
using 新警成长管理工具.Tools;

namespace 新警成长管理工具.VModel
{
    internal partial class MainWindowVM : ObservableValidator
    {
        #region 首页
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Line1))]
        [NotifyPropertyChangedFor(nameof(Line2))]
        [NotifyPropertyChangedFor(nameof(Line3))]
        [NotifyPropertyChangedFor(nameof(Line4))]
        [NotifyPropertyChangedFor(nameof(Line5))]
        private string? selectedYear = "";

        public string? Line1 => $"共计新警{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Count()}人，其中男性{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanSex == "男").Count()}人，女性{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanSex == "女").Count()}人；";
        public string? Line2 => $"最大年龄{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Max(b => b.PolicemanAge)}岁，最小年龄{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Min(b => b.PolicemanAge)}岁，平均年龄{Math.Round(GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Average(b => b.PolicemanAge))}岁；";
        public string? Line3 => $"中共党员{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.IfCommunist == "是").Count()}人，研究生{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanDegree == "研究生").Count()}人，本科生{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanDegree == "本科生").Count()}人；";
        public string? Line4 => $"积分排名前三位是{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderByDescending(b => b.PolicemanScore).ElementAt(0).PolicemanName}，{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderByDescending(b => b.PolicemanScore).ElementAt(1).PolicemanName}，{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderByDescending(b => b.PolicemanScore).ElementAt(2).PolicemanName}；";
        public string? Line5 => $"　　　　末三位是{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderBy(b => b.PolicemanScore).ElementAt(0).PolicemanName}，{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderBy(b => b.PolicemanScore).ElementAt(1).PolicemanName}，{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderBy(b => b.PolicemanScore).ElementAt(2).PolicemanName}。";

        [ObservableProperty]
        [RegularExpression("^\\d{4}$")]
        private string? needAddYear;

        [RelayCommand]
        private void AddYear()
        {
            ValidateAllProperties();
            if (!HasErrors)
                if (NeedAddYear != null)
                    if (!GlobalDataHelper.appConfig!.PolicemanYear.Contains(NeedAddYear))
                        GlobalDataHelper.appConfig!.PolicemanYear.Add(NeedAddYear);
        }
        #endregion

        #region 登录页
        [ObservableProperty]
        private string? userName = "";

        [ObservableProperty]
        private string? userPassword = "";

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
            foreach (var pm in GlobalDataHelper.policemanLibrary!.PolicemanList)
            {
                var temp = pm.PolicemanReward.FirstOrDefault(t => t.RewardOrPunishID == r.RewardID);
                if (temp != null)
                    pm.PolicemanReward.Remove(temp);
            }

            GlobalDataHelper.rewardANDPunishLibrary!.RewardItems.Remove(r);
        }

        [RelayCommand]
        private void PDel(PunishItem p)
        {
            foreach (var pm in GlobalDataHelper.policemanLibrary!.PolicemanList)
            {
                var temp = pm.PolicemanPunish.FirstOrDefault(t => t.RewardOrPunishID == p.PunishID);
                if (temp != null)
                    pm.PolicemanPunish.Remove(temp);
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
                var temp = Sp.PolicemanReward.FirstOrDefault(t => t.RewardOrPunishID == SelectR.RewardID);
                if (temp == null)
                {
                    Sp.PolicemanReward.Add(new SingleRewardOrPunish4Policeman()
                    {
                        RewardOrPunishID = SelectR.RewardID,
                        AddAdmin = UserName!,
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
                var temp = Sp.PolicemanPunish.FirstOrDefault(t => t.RewardOrPunishID == SelectP.PunishID);
                if (temp == null)
                {
                    Sp.PolicemanPunish.Add(new SingleRewardOrPunish4Policeman()
                    {
                        RewardOrPunishID = SelectP.PunishID,
                        AddAdmin = UserName!,
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

        /// <summary>
        /// 刷新警师库
        /// </summary>
        [RelayCommand]
        private void UpdatePolicemanMasters()
        {
            GlobalDataHelper.UpdatePolicemanMasters();
        }
        #endregion

        #region 设置
        [RelayCommand]
        private void SaveImportTemplateFile()
        {
            var resourceStream = Application.GetResourceStream(new Uri("/新警成长管理工具;component/Resources/警员数据导入模板.xlsx", UriKind.Relative)).Stream;

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = "警员数据导入模板.xlsx",
                Filter = "Excel 工作簿(*.xlsx)|*.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string destinationPath = saveFileDialog.FileName;
                using (var fileStream = File.Create(destinationPath))
                {
                    resourceStream.Seek(0, SeekOrigin.Begin);
                    resourceStream.CopyTo(fileStream);
                }
            }
        }

        [RelayCommand]
        private void UploadPolicemanDataFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel 工作簿(*.xlsx)|*.xlsx"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string destinationPath = openFileDialog.FileName;
                int importCount = 0;
                try
                {
                    using (var stream = File.Open(destinationPath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetValue(0).ToString()! == "姓名*")
                                        continue;

                                    SinglePoliceman sp = new SinglePoliceman()
                                    {
                                        PolicemanName = reader.GetValue(0).ToString()!,
                                        PolicemanDegree = (string)reader.GetValue(1),
                                        PolicemanNo = reader.GetValue(2).ToString()!,
                                        PolicemanReward = [
                                            new SingleRewardOrPunish4Policeman(){
                                            RewardOrPunishID = GlobalDataHelper.appConfig!.BePolicemanRewardID,
                                            AddAdmin = UserName!,
                                            AddTime = DateTime.Now
                                        }
                                        ],
                                        PolicemanYear = (string)reader.GetValue(4),
                                        PolicemanSource = (string)reader.GetValue(5),
                                        PolicemanIDNo = reader.GetValue(6).ToString()!,
                                        PolicemanAddr = (string)reader.GetValue(7),
                                    };
                                    if ((string)reader.GetValue(3) == "是")
                                    {
                                        sp.PolicemanReward.Add(new SingleRewardOrPunish4Policeman()
                                        {
                                            RewardOrPunishID = GlobalDataHelper.appConfig!.CommunistRewardID,
                                            AddAdmin = UserName!,
                                            AddTime = DateTime.Now
                                        });
                                    }
                                    GlobalDataHelper.policemanLibrary!.PolicemanList.Add(sp);
                                    importCount++;
                                }
                            } while (reader.NextResult());
                        }
                    }
                    Growl.Success($"成功导入{importCount}条数据。");
                }
                catch (IOException ex)
                {
                    Growl.Error($"未能打开文件，请检查文件是否被占用。");
                }
                catch
                {
                    Growl.Error($"未能完整导入，已导入{importCount}条数据，其后发现错误，请检查Excel文件第{importCount + 2}行是否正确填写。");
                }
            }
        }

        [RelayCommand]
        private void LoginOut()
        {
            LoginSuccess = false;
        }
        #endregion
    }
}
