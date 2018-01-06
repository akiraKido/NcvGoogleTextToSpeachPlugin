using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using Plugin;

namespace NcvPluginSample
{
    public class Main : IPlugin
    {
        private readonly Queue<string> _comments = new Queue<string>();

        private IWebDriver _driver;

        public void AutoRun() => Run();

        public void Run()
        {
            if(_driver != null) return;

            var driver = new ChromeDriver
            {
                Url = "https://translate.google.co.jp/#auto/ja/"
            };
            _driver = driver;

            Host.ReceivedComment += Host_ReceivedComment;
            Host.BroadcastDisConnected += Host_BroadcastDisConnected;

            Task.Run( async () =>
            {
                while (true)
                {
                    if (_comments.Any())
                    {
                        var comment = _comments.Dequeue();
                        await MakeGoogleSpeak(comment);
                    }

                    await Task.Delay(2000);
                }
            });
        }

        private void Host_BroadcastDisConnected(object sender, System.EventArgs e)
        {
            _driver.Dispose();
        }

        private void Host_ReceivedComment(object sender, ReceivedCommentEventArgs e)
        {
            var comment = e.CommentDataList.First().Comment;
            _comments.Enqueue(comment);
        }

        private async Task MakeGoogleSpeak(string message)
        {
            var textbox = _driver.FindElement(By.Id("source"));
            textbox.Clear();
            textbox.SendKeys(message);
            await Task.Delay(1000);
            var gtspeech = _driver.FindElement(By.XPath("//*[@id=\"gt-src-listen\"]"));
            var action = new Actions(_driver).Click(gtspeech).Build();
            action.Perform();
        }

        public IPluginHost Host { get; set; }
        public bool IsAutoRun { get; } = true;
        public string Description { get; } = "グーグルがコメントを読み上げてくれます。";
        public string Version { get; } = "0.1";
        public string Name { get; } = "グーグル読み上げさん";
    }
}
