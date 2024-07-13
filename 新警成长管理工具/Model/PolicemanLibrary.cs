using System.ComponentModel;

namespace 新警成长管理工具.Model
{
    internal class PolicemanLibrary
    {
        /// <summary>
        /// 新警库文件存储路径
        /// </summary>
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}PolicemanLibrary.json";

        public BindingList<SinglePoliceman> PolicemanList { get; set; } = [];
    }
}
