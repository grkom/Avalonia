﻿using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Input;
using Avalonia.Utilities;

namespace Avalonia.Data.Converters
{
    class AlwaysEnabledDelegateCommand : ICommand
    {
        private readonly Delegate action;

        private ParameterInfo parameterInfo;

        public AlwaysEnabledDelegateCommand(Delegate action)
        {
            this.action = action;
            var parameters = action.Method.GetParameters();
            parameterInfo = parameters.Length == 0 ? null : parameters[0];
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameterInfo == null)
            {
                action.DynamicInvoke();
            }
            else
            {
                TypeUtilities.TryConvert(parameterInfo.ParameterType, parameter, CultureInfo.CurrentCulture, out object convertedParameter);
                action.DynamicInvoke(convertedParameter); 
            }
        }
    }
}
