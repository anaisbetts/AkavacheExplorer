using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using AkavacheExplorer.ViewModels;
using ReactiveUI;

namespace AkavacheExplorer.Views
{
    /// <summary>
    /// Interaction logic for CacheView.xaml
    /// </summary>
    public partial class CacheView : UserControl, IViewFor<CacheViewModel>
    {
        public CacheView()
        {
            InitializeComponent();

            RxApp.MainThreadScheduler.Schedule(() => {
                new[] { textRadio, jsonRadio, imageRadio }
                    .Select(y => y.WhenAny(x => x.IsChecked, x => x).Where(x => x.Value == true).Select(x => x.Sender.Tag))
                    .Merge()
                    .Subscribe(x => ViewModel.SelectedViewer = (string)x);
            });
        }

        public CacheViewModel ViewModel {
            get { return (CacheViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CacheViewModel), typeof(CacheView), new UIPropertyMetadata(null));

        object IViewFor.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (CacheViewModel) value; } 
        }
    }
}
