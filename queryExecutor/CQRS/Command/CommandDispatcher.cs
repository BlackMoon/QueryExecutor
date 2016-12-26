using System;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;

namespace queryExecutor.CQRS.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IContainer _container;

        public CommandDispatcher(IContainer container)
        {
            _container = container;
        }

        public void Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _container.Resolve<ICommandHandler<TParameter>>();
            handler.Execute(command);
        }

        public TResult Dispatch<TParameter, TResult>(TParameter command) where TParameter : ICommand where TResult : ICommandResult
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _container.Resolve<ICommandHandlerWithResult<TParameter, TResult>>();
            return handler.Execute(command);
        }
    }
}