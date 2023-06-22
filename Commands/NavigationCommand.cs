using AdmissionCampaign.Commands.Base;
using AdmissionCampaign.ViewModels.Base;
using System;
using System.Windows.Controls;

namespace AdmissionCampaign.Commands
{
    /// <summary>
    /// Команда для перемещения по страницам
    /// </summary>
    public class NavigationCommand : BaseCommand
    {
        private readonly Uri uri;

        public NavigationCommand(Uri uri)
        {
            this.uri = uri;
        }

        public override void Execute(object parameter)
        {
            Page page = parameter as Page;
            ViewModel.NavigateToPage(page, uri);
        }
    }
}
