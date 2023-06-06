using AdmissionCampaign.Commands.Base;
using System;

namespace AdmissionCampaign.Commands
{
    public class SimpleCallbackCommand : BaseCommand
    {
        private readonly Action execute;

        public SimpleCallbackCommand(Action execute)
        {
            this.execute = execute;
        }

        public override void Execute(object parameter)
        {
            execute();
        }
    }
}
