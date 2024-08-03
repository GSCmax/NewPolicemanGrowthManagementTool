using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExcelDataReader;
using HandyControl.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Win32;
using SkiaSharp;
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
        [NotifyPropertyChangedFor(nameof(YearStr))]
        private string? selectedYear = "";

        public string? YearStr => $"""
            年度汇总：

            共计新警{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Count()}人，其中男性{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanSex == "男").Count()}人，女性{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanSex == "女").Count()}人；
            中共党员{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.IfCommunist == "是").Count()}人，研究生{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanDegree == "研究生").Count()}人，本科生{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Where(b => b.PolicemanDegree == "本科生").Count()}人；
            最大年龄{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Max(b => b.PolicemanAge)}岁，最小年龄{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Min(b => b.PolicemanAge)}岁，平均年龄{Math.Round(GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).Average(b => b.PolicemanAge))}岁；
            积分排名前三位是：{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderByDescending(b => b.PolicemanScore).ElementAt(0).PolicemanName}、{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderByDescending(b => b.PolicemanScore).ElementAt(1).PolicemanName}、{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderByDescending(b => b.PolicemanScore).ElementAt(2).PolicemanName}；
            　　　　末三位是：{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderBy(b => b.PolicemanScore).ElementAt(0).PolicemanName}、{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderBy(b => b.PolicemanScore).ElementAt(1).PolicemanName}、{GlobalDataHelper.policemanLibrary!.PolicemanList.Where(a => a.PolicemanYear == SelectedYear).OrderBy(b => b.PolicemanScore).ElementAt(2).PolicemanName}。
            """;
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

        #region 警员库
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
                #region 成长树相关
                Branches.Clear();
                DrawTree(193, 380, -90, 100, Sp!.PolicemanScore * 0.1, Sp!.PolicemanReward.Count);
                #endregion

                #region 雷达图相关
                Dictionary<string, double> categoryScore = new Dictionary<string, double>();
                foreach (var temp in GlobalDataHelper.appConfig!.RewardANDPunishCategory)
                    categoryScore.Add(temp, 0);

                foreach (var temp1 in Sp.PolicemanReward)
                {
                    var temp2 = GlobalDataHelper.rewardANDPunishLibrary!.RewardItems.FirstOrDefault(t => t.RewardID == temp1.RewardOrPunishID);
                    if (temp2 != null)
                        categoryScore[temp2.RewardANDPunishCategory] += temp2.RewardScore;
                }

                foreach (var temp1 in Sp.PolicemanPunish)
                {
                    var temp2 = GlobalDataHelper.rewardANDPunishLibrary!.PunishItems.FirstOrDefault(t => t.PunishID == temp1.RewardOrPunishID);
                    if (temp2 != null)
                        categoryScore[temp2.RewardANDPunishCategory] -= temp2.PunishScore;
                }

                AngleAxes = [new PolarAxis {
                    LabelsRotation = LiveCharts.TangentAngle,
                    Labels = GlobalDataHelper.appConfig!.RewardANDPunishCategory,
                    LabelsPaint = new SolidColorPaint {
                        Color = SKColors.Black,
                        SKTypeface = SKFontManager.Default.MatchCharacter('汉')
                    }
                }];

                Series = [new PolarLineSeries<double>{
                    Values = categoryScore.Values,
                    LineSmoothness = 0,
                    GeometrySize = 0,
                    Fill = new SolidColorPaint(new SKColor(50, 108, 243,204)),
                    Stroke = new SolidColorPaint(new SKColor(50, 108, 243)),
                    IsHoverable = false
                }];
                #endregion
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

        #region 雷达图相关
        /// <summary>
        /// 项目数据
        /// </summary>
        [ObservableProperty]
        private ISeries[] series = [new PolarLineSeries<double>()];

        /// <summary>
        /// 项目名称
        /// </summary>
        [ObservableProperty]
        private PolarAxis[] angleAxes = [new PolarAxis()];
        #endregion

        #region 成长树相关
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

        [ObservableProperty]
        private string? needAddCategory;

        [RelayCommand]
        private void AddCategory()
        {
            if (NeedAddCategory != null)
                if (!GlobalDataHelper.appConfig!.RewardANDPunishCategory.Contains(NeedAddCategory))
                    GlobalDataHelper.appConfig!.RewardANDPunishCategory.Add(NeedAddCategory);
        }

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
                catch (IOException)
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
