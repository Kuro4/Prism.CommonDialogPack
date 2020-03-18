using Prism.Common;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Prism.CommonDialogPack
{
    public class StyleableDialogService : IDialogService
    {
        private readonly IContainerExtension _containerExtension;

        public StyleableDialogService(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, false);
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, true);
        }

        void ShowDialogInternal(string name, IDialogParameters parameters, Action<IDialogResult> callback, bool isModal)
        {
            IDialogWindow dialogWindow = CreateDialogWindow();
            ConfigureDialogWindowEvents(dialogWindow, callback);
            ConfigureDialogWindowContent(name, dialogWindow, parameters);

            if (isModal)
                dialogWindow.ShowDialog();
            else
                dialogWindow.Show();
        }

        IDialogWindow CreateDialogWindow()
        {
            return _containerExtension.Resolve<IDialogWindow>();
        }

        void ConfigureDialogWindowContent(string dialogName, IDialogWindow window, IDialogParameters parameters)
        {
            var content = _containerExtension.Resolve<object>(dialogName);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (!(dialogContent.DataContext is IDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            ConfigureDialogWindowProperties(window, dialogContent, viewModel);

            MvvmHelpers.ViewAndViewModelAction<IDialogAware>(viewModel, d => d.OnDialogOpened(parameters));
        }

        void ConfigureDialogWindowEvents(IDialogWindow dialogWindow, Action<IDialogResult> callback)
        {
            Action<IDialogResult> requestCloseHandler = null;
            requestCloseHandler = (o) =>
            {
                dialogWindow.Result = o;
                dialogWindow.Close();
            };

            RoutedEventHandler loadedHandler = null;
            loadedHandler = (o, e) =>
            {
                dialogWindow.Loaded -= loadedHandler;
                ((IDialogAware)dialogWindow.DataContext).RequestClose += requestCloseHandler;
            };
            dialogWindow.Loaded += loadedHandler;

            CancelEventHandler closingHandler = null;
            closingHandler = (o, e) =>
            {
                if (!((IDialogAware)dialogWindow.DataContext).CanCloseDialog())
                    e.Cancel = true;
            };
            dialogWindow.Closing += closingHandler;

            EventHandler closedHandler = null;
            closedHandler = (o, e) =>
            {
                dialogWindow.Closed -= closedHandler;
                dialogWindow.Closing -= closingHandler;
                var vm = (IDialogAware)dialogWindow.DataContext;
                vm.RequestClose -= requestCloseHandler;

                vm.OnDialogClosed();

                if (dialogWindow.Result == null)
                    dialogWindow.Result = new DialogResult();

                callback?.Invoke(dialogWindow.Result);

                dialogWindow.DataContext = null;
                dialogWindow.Content = null;
            };
            dialogWindow.Closed += closedHandler;
        }

        void ConfigureDialogWindowProperties(IDialogWindow window, FrameworkElement dialogContent, IDialogAware viewModel)
        {
            var windowStyle = Dialog.GetWindowStyle(dialogContent);
            if (window is Window hostWindow && viewModel is IStyleableDialogAware styleableVM)
            {
                hostWindow.SetBinding(Window.StyleProperty, new Binding("WindowStyle"));
                hostWindow.Width = styleableVM.Width;
                hostWindow.Height = styleableVM.Height;
            }
            else if (windowStyle != null)
                window.Style = windowStyle;

            window.Content = dialogContent;
            window.DataContext = viewModel; //we want the host window and the dialog to share the same data contex

            if (window.Owner == null)
                window.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
        }
    }
}
