using CommunityToolkit.Mvvm.ComponentModel;

namespace 新警成长管理工具.Model
{
    internal partial class Branch : ObservableObject
    {
        [ObservableProperty]
        public double x1;

        [ObservableProperty]
        public double y1;

        [ObservableProperty]
        public double x2;

        [ObservableProperty]
        public double y2;

        [ObservableProperty]
        public double thickness;
    }
}
