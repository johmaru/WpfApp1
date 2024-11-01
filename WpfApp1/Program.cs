using System.Windows;
using Velopack;

namespace WpfApp1;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            //velopackの初期化&起動 .WithFirstRunで初めて起動したときの制御
            VelopackApp.Build().WithBeforeUninstallFastCallback((v) => { }).WithFirstRun((v) =>
            {
                MessageBox.Show("Thanks for installing my application!");
            }).Run();
                
            //velopackの初期化と起動そしてアップデートチェック時にもアプリケーションを動作させたいので、別スレッドで起動
            Thread thread = new Thread(() =>
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}