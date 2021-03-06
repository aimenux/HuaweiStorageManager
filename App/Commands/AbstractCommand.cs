using System;
using System.Reflection;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;
using OBS;

namespace App.Commands
{
    public abstract class AbstractCommand
    {
        protected IConsoleHelper ConsoleHelper;

        protected AbstractCommand(IConsoleHelper consoleHelper)
        {
            ConsoleHelper = consoleHelper;
        }

        public void OnExecute(CommandLineApplication app)
        {
            try
            {
                Initialize();

                if (!HasValidOptions() || !HasValidArguments())
                {
                    throw new Exception($"Invalid options/arguments for command {GetType().Name}");
                }

                Execute(app);
            }
            catch (ObsException ex)
            {
                ConsoleHelper.RenderException(ex);
            }
            catch (Exception ex)
            {
                ConsoleHelper.RenderException(ex);
            }
        }

        protected abstract void Execute(CommandLineApplication app);

        protected virtual void Initialize()
        {
            // any staff you need to run before command execution
            // like setting default values for options/arguments, etc.
        }

        protected virtual bool HasValidOptions() => true;

        protected virtual bool HasValidArguments() => true;

        protected static string GetVersion(Type type)
        {
            return type
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
        }
    }
}