using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;
using System.Windows.Documents;

namespace OpenInTerminal
{
    /// <summary>
    /// Command1 handler.
    /// </summary>
    [VisualStudioContribution]
    internal class OpenInTerminalCommand : Command
    {
        private readonly TraceSource logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenInTerminalCommand"/> class.
        /// </summary>
        /// <param name="traceSource">Trace source instance to utilize.</param>
        public OpenInTerminalCommand(TraceSource traceSource)
        {
            // This optional TraceSource can be used for logging in the command. You can use dependency injection to access
            // other services here as well.
            this.logger = Requires.NotNull(traceSource, nameof(traceSource));
        }

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new("%OpenInTerminal.OpenInTerminalCommand.DisplayName%")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.OpenFileDialog, IconSettings.IconAndText),
            Placements =
            [
                // File in project context menu
                CommandPlacement.VsctParent(new Guid("{d309f791-903f-11d0-9efc-00a0c911004f}"), id: 0x0209, priority: 0x1600),

                // Project context menu
                CommandPlacement.VsctParent(new Guid("{d309f791-903f-11d0-9efc-00a0c911004f}"), id: 0x0266, priority: 0x1600), 

                // Folder context menu
                CommandPlacement.VsctParent(new Guid("{d309f791-903f-11d0-9efc-00a0c911004f}"), id: 0x02A3, priority: 0x1600),
            ],
        };

        /// <inheritdoc />
        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Use InitializeAsync for any one-time setup or initialization.
            return base.InitializeAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            string dir = "";
            try
            {
                dir = Path.GetDirectoryName(context.GetSelectedPathAsync(cancellationToken).Result.LocalPath);
            }
            catch (Exception)
            {
            }
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "wt.exe",
                Arguments = $"wt -d \"{dir}\"",
                UseShellExecute = true,
                CreateNoWindow = true  // 不显示命令窗口
            };
            Process.Start(startInfo);
        }
    }
}
