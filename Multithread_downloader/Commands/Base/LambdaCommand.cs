#nullable enable
using System;
using System.Windows.Input;

namespace Multithread_downloader.Commands.Base
{
	internal class LambdaCommand : ICommand
	{
		private readonly Action<object?> _executeAction;
		private readonly Func<object?, bool>? _canExecuteFunc;

		public event EventHandler? CanExecuteChanged {
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public LambdaCommand(Action<object?> executeAction, Func<object?, bool>? canExecuteFunc = null)
		{
			_executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
			_canExecuteFunc = canExecuteFunc;
		}

		public bool CanExecute(object? parameter) => _canExecuteFunc?.Invoke(parameter) ?? true;

		public void Execute(object? parameter) => _executeAction.Invoke(parameter);
	}
}
