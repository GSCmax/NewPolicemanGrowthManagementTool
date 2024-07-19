using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel;

namespace 新警成长管理工具.Model
{
    internal partial class AppConfig : ObservableObject
    {
        /// <summary>
        /// 配置文件存储路径
        /// </summary>
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}Config.json";

        /// <summary>
        /// 默认用户名
        /// </summary>
        [ObservableProperty]
        private string user = "Admin";

        /// <summary>
        /// 默认密码
        /// </summary>
        [ObservableProperty]
        private string password = "123456";

        /// <summary>
        /// 默认入警途径
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BindingList<string> PolicemanSource { get; set; } = ["警校", "社招", "军转"];
    }
}
