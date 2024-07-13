using CommunityToolkit.Mvvm.ComponentModel;

namespace 新警成长管理工具.Model
{
    internal partial class AppConfig : ObservableObject
    {
        public AppConfig()
        {
        }

        /// <summary>
        /// 配置文件存储路径
        /// </summary>
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}Config.json";

        [ObservableProperty]
        private string user = "Admin";

        [ObservableProperty]
        private string password = "123456";
    }
}
