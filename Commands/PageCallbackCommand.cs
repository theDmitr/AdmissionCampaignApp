﻿using AdmissionCampaign.Commands.Base;
using System;
using System.Windows.Controls;

namespace AdmissionCampaign.Commands
{
    /// <summary>
    /// Команда, вызывающая Callback-функцию, принимающую один параметр - Page
    /// </summary>
    public class PageCallbackCommand : BaseCommand
    {
        private readonly Action<Page> execute;

        public PageCallbackCommand(Action<Page> execute)
        {
            this.execute = execute;
        }

        public override void Execute(object parameter)
        {
            Page page = parameter as Page;
            execute(page);
        }
    }
}
