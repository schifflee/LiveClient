using LiveClientDesktop.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace LiveClientDesktop.WindowViews
{
    /// <summary>
    /// PreviewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PreviewWindow : MetroWindow
    {
        public PreviewWindow()
        {
            InitializeComponent();
        }
        private readonly PreviewWindowViewModel _previewWindowViewModel;
        public PreviewWindow(PreviewWindowViewModel previewWindowViewModel)
            : this()
        {
            this.DataContext = _previewWindowViewModel = previewWindowViewModel;
            _previewWindowViewModel.LoadPreviewWindowImages();
        }

        private void SelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            _previewWindowViewModel.PublishSelectedDemonstrationWindowEvent();
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            _previewWindowViewModel.ClearPreviewImage();
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            _previewWindowViewModel.LoadPreviewWindowImages();
        }
    }
}
