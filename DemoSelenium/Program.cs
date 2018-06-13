using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.IO;

namespace DemoSelenium
{
    class Program
    {
        private static string username = "ki114-17-9";
        private static string password = "Suxrobbek0729#";
        private static string LoginSite = @"http://moodle.samtuit.uz/login/index.php";
        private static List<KeyValuePair<string,string>> QA_List = new List<KeyValuePair<string, string>>();

        static void Main(string[] args)
        {
            Console.WriteLine("Test Selenium Framework");
            /*Console.WriteLine("Please enter the login: ");
            username = Console.ReadLine();
            Console.WriteLine("Please enter the password: ");
            password = Console.ReadLine();
            */
         
            try
            {
                var ChromePath = Microsoft
                .Win32
                .Registry
                .GetValue(@"HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command", null, null) as string;

                if (ChromePath != null)
                {
                    var split = ChromePath.Split('\"');
                    ChromePath = split.Length >= 2 ? split[1] : null;
                    GetPhysicsQA(new ChromeDriver());
                }
                else
                {
                    GetPhysicsQA(new EdgeDriver());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: Main() " + ex.Message);
            }                 
            

            if (!File.Exists("PhysicsQA.txt"))
            {
                File.Create("PhysicsQA.txt").Close();
            }           
            WriteToFile(QA_List, "PhysicsQA.txt");

            Console.WriteLine("All operations has successfully done and all Questions Answers have copied to \"PhysicsQA.txt\"");
            Console.ReadLine();
        }

        private static void GetPhysicsQA(IWebDriver driver)
        {
            try
            {
                Login(driver, username, password);
                Thread.Sleep(4000); //4c

                driver.Navigate().GoToUrl("http://moodle.samtuit.uz/mod/quiz/attempt.php?attempt=20765");
                Thread.Sleep(4000); //4c

                var button = driver.FindElement(By.ClassName("continuebutton"));
                button.Click();
                Thread.Sleep(4000); //4c
                button = driver.FindElement(By.XPath("//button[@type='submit']"));
                button.Click();
                Thread.Sleep(4000); //4c
                button = driver.FindElement(By.ClassName("endtestlink"));
                button.Click();
                Thread.Sleep(4000); //4c
                button = driver.FindElement(By.XPath("//button[@type='submit'][text()='Отправить всё и завершить тест']"));
                button.Click();
                Thread.Sleep(4000); //4c
                button = driver.FindElement(By.XPath("//input[@type='button'][@value='Отправить всё и завершить тест']"));
                //button = driver.FindElementByXPath("//input[contains(@id, 'id_yuiconfirmyes')]");
                button.Click();
                Thread.Sleep(4000); //4c

                driver.Navigate().GoToUrl("http://moodle.samtuit.uz/mod/quiz/review.php?attempt=20779&showall=1");
                Thread.Sleep(4000); //4c

                var questions = driver.FindElements(By.ClassName("qtext"));
                var answers = driver.FindElements(By.ClassName("rightanswer"));

                for (int i = 0; i < questions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {questions[i].Text} \n{answers[i].Text} \n");
                    QA_List.Add(new KeyValuePair<string, string>(questions[i].Text, answers[i].Text));
                }

                Thread.Sleep(10000); //10c 
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: GetPhysicsQA() " + ex.Message);
            }
        }

        private static void Login(IWebDriver webDriver, string username, string password)
        {
            try
            {
                webDriver.Navigate().GoToUrl(LoginSite);
                var userNameField = webDriver.FindElement(By.Id("username"));
                var userPasswordField = webDriver.FindElement(By.Id("password"));
                var loginButton = webDriver.FindElement(By.Id("loginbtn"));

                userNameField.SendKeys(username);
                userPasswordField.SendKeys(password);
                loginButton.Click();
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: Login() " + ex.Message);
            }            
        }

        private static void WriteToFile(List<KeyValuePair<string, string>> QAPairs, string fileName)
        {     
            List<string> buffer = new List<string>();
            int count = 0;
            foreach (var item in QAPairs)
            {
                count++;               
                buffer.Add($"{count}. {item.Key} - {item.Value}\n");
            }
            File.WriteAllLines(fileName, buffer.ToArray());         
        }
    }
}
