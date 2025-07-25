using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationNameUpperCamelCase.ViewModels;
using Avalonia.Controls;
using Avalonia.Threading;

using Nullinside.Api.Common.Desktop;

#if !DEBUG
using Microsoft.Extensions.DependencyInjection;
#else
using Avalonia;
#endif
namespace ApplicationNameUpperCamelCase.Views;

/// <summary>
///   The main application window.
/// </summary>
public partial class MainWindow : Window {
  /// <summary>
  ///   Initializes a new instance of the <see cref="MainWindow" /> class.
  /// </summary>
  public MainWindow() {
    InitializeComponent();
#if DEBUG
    this.AttachDevTools();
#endif
  }
  
  #if !DEBUG
  /// <summary>
  ///   The service provider for DI.
  /// </summary>
  public IServiceProvider? ServiceProvider { get; set; }
  #endif

  /// <summary>
  ///   Checks for a new version number of the application.
  /// </summary>
  protected override void OnInitialized() {
    base.OnInitialized();

    var args = Environment.GetCommandLineArgs().ToList();
    if (args.Contains("--update")) {
      _ = GitHubUpdateManager.PerformUpdateAndRestart("nullinside-development-group", "ApplicationNameUpperCamelCase", args[2], "windows-x64.zip");
      return;
    }

    if (args.Contains("--justUpdated")) {
      _ = GitHubUpdateManager.CleanupUpdate();
    }

    Task.Factory.StartNew(async () => {
      GithubLatestReleaseJson? serverVersion =
        await GitHubUpdateManager.GetLatestVersion("nullinside-development-group", "ApplicationNameUpperCamelCase").ConfigureAwait(false);
      string? localVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
      if (null == serverVersion || string.IsNullOrWhiteSpace(serverVersion.name) ||
          string.IsNullOrWhiteSpace(localVersion)) {
        return;
      }

      localVersion = localVersion.Substring(0, localVersion.LastIndexOf('.'));
      if (string.IsNullOrWhiteSpace(localVersion)) {
        return;
      }

      if (serverVersion.name?.Equals(localVersion, StringComparison.InvariantCultureIgnoreCase) ?? true) {
// Had to add this because code clean up tools were removing the "redundant" return statement.
// which was causing the check to always be ignored.
#if !DEBUG
        return;
#endif
      }

#if !DEBUG
      var vm = ServiceProvider?.GetRequiredService<NewVersionWindowViewModel>();
      if (null == vm) {
        return;
      }

      vm.LocalVersion = localVersion;
      Dispatcher.UIThread.Post(async void () => {
        try {
          var versionWindow = new NewVersionWindow {
            DataContext = vm
          };

          await versionWindow.ShowDialog(this).ConfigureAwait(false);
        }
        catch {
          // do nothing, don't crash
        }
      });
#endif
    });
  }
}