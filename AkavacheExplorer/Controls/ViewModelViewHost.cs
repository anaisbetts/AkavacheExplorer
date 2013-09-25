using System;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;

// NB: This is to work around a glitched version of this in RxUI <= 3.1.2
namespace AkavacheExplorer.Controls
{
    /// <summary>
    /// This content control will automatically load the View associated with
    /// the ViewModel property and display it. This control is very useful
    /// inside a DataTemplate to display the View associated with a ViewModel.
    /// </summary>
    public class ViewModelViewHost : TransitioningContentControl
    {
        /// <summary>
        /// The ViewModel to display
        /// </summary>
        public IReactiveNotifyPropertyChanged ViewModel
        {
            get { return (IReactiveNotifyPropertyChanged)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IReactiveNotifyPropertyChanged), typeof(ViewModelViewHost), new PropertyMetadata(null));

        /// <summary>
        /// If no ViewModel is displayed, this content (i.e. a control) will be displayed.
        /// </summary>
        public object DefaultContent
        {
            get { return (object)GetValue(DefaultContentProperty); }
            set { SetValue(DefaultContentProperty, value); }
        }
        public static readonly DependencyProperty DefaultContentProperty =
            DependencyProperty.Register("DefaultContent", typeof(object), typeof(ViewModelViewHost), new PropertyMetadata(null));

        public ViewModelViewHost()
        {
            var latestViewModel = this.WhenAny(x => x.ViewModel, x => x.Value)
                .StartWith((IReactiveNotifyPropertyChanged) null);

            latestViewModel.Subscribe(vm =>
            {
                if (vm == null)
                {
                    Content = DefaultContent;
                    return;
                }

                var view = RxRouting.ResolveView(vm);
                view.ViewModel = vm;
                Content = view;
            });
        }
    }
}