﻿using System;
using System.Windows.Input;

namespace AdmissionCampaign.Commands.Base
{
    /// <summary>
    /// Абстрактный (базовый) класс команды
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);
    }
}
