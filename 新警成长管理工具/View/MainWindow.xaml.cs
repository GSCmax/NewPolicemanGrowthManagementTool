using HandyControl.Controls;
using 新警成长管理工具.Tools;
using 新警成长管理工具.VModel;

namespace 新警成长管理工具.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as MainWindowVM)!.UserPassword = (sender as PasswordBox)!.Password;
        }

        private void ButtonShiftOut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bd.Visibility = System.Windows.Visibility.Visible;
            ButtonShiftOut.Visibility = System.Windows.Visibility.Collapsed;
            ButtonShiftIn.Visibility = System.Windows.Visibility.Visible;
        }

        private void ButtonShiftIn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bd.Visibility = System.Windows.Visibility.Collapsed;
            ButtonShiftOut.Visibility = System.Windows.Visibility.Visible;
            ButtonShiftIn.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Tag_Closed(object sender, EventArgs e)
        {
            foreach (var item in GlobalDataHelper.rewardANDPunishLibrary!.RewardItems)
                if (item.RewardANDPunishCategory == (sender as Tag)!.Content.ToString())
                    item.RewardANDPunishCategory = "基础";

            foreach (var item in GlobalDataHelper.rewardANDPunishLibrary.PunishItems)
                if (item.RewardANDPunishCategory == (sender as Tag)!.Content.ToString())
                    item.RewardANDPunishCategory = "基础";
        }
    }
}
