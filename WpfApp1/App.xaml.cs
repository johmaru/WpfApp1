using System.Configuration;
using System.Data;
using System.Windows;
using Velopack;
using Velopack.Sources;

namespace WpfApp1;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    
    
    private static async Task UpdateCheck()
    {
        try
        {
            /*httpsやgitlabなども設定できます。
今回は、githubで設定しました。ダウングレードやStable、Betaのチャンネル切り替えも出来ますが、今回はシンプルな構造にしました。
                                        */
            var mgr = new UpdateManager(new GithubSource(@"https://github.com/johmaru/WpfApp1", null, false),
                new UpdateOptions
                {
                    AllowVersionDowngrade = true
                });

            if (!mgr.IsInstalled)
            {
                Console.WriteLine("Application is not installed. Update check cannot proceed.");
                return;
            }

            var newVersionCheck = await mgr.CheckForUpdatesAsync();
            if (newVersionCheck == null) return;

            var result = MessageBox.Show("New Version Available", "Update", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await mgr.DownloadUpdatesAsync(newVersionCheck);
                mgr.ApplyUpdatesAndRestart(newVersionCheck);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
        MainWindow = new MainWindow();
        MainWindow.Show();
        await UpdateCheck();
    }
}