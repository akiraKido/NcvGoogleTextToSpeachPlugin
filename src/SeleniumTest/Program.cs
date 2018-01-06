using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new ChromeDriver
            {
                Url = "https://translate.google.co.jp/#ja/ja/"
            };
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            var textbox = driver.FindElement(By.Id("source"));
            textbox.SendKeys("hogehoge");
            Thread.Sleep(1000);
            var gtspeech = driver.FindElement(By.XPath("//*[@id=\"gt-src-listen\"]"));
            var action = new Actions(driver).Click(gtspeech).Build();
            action.Perform();
            //gtspeech.Click();
        }
    }
}
