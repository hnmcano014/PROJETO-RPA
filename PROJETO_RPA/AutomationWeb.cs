using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Npgsql;
using System.Runtime.InteropServices;

namespace SeleniumBot
{
    public class AutomationWeb
    {
        public IWebDriver driver;
        public AutomationWeb()
        {
            driver = new ChromeDriver();
        }
        public void TestWeb()
        {

            driver.Navigate().GoToUrl("https://10fastfingers.com/typing-test/portuguese");

            Thread.Sleep(3000);

            driver.FindElement(By.XPath("/html/body/div[10]/div/div/div/div/div[1]/div/div[2]")).Click();
            driver.FindElement(By.XPath("/html/body/div[11]/div/button")).Click();
            driver.FindElement(By.CssSelector("#CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll")).Click();

            for (int i = 1; ; i++)
            {
                IWebElement minutos = driver.FindElement(By.XPath("/html/body/div[5]/div/div[4]/div/div[1]/div[7]/div[2]/div/div[2]/div[1]"));

                string minutostext = minutos.Text;


                if (minutostext != "0:00")
                {
                    IWebElement elementotexto = driver.FindElement(By.XPath("/html/body/div[5]/div/div[4]/div/div[1]/div[7]/div[1]/div/span[" + i + "]"));

                    string TextoExtraido = elementotexto.Text;
                    List<string> palavras = new List<string>(TextoExtraido.Split(' '));

                    foreach (string x in palavras)
                    {
                        driver.FindElement(By.XPath("//*[@id=\"inputfield\"]")).SendKeys(x);
                        IWebElement elemento = driver.FindElement(By.XPath("//*[@id=\"inputfield\"]"));
                        Actions actions = new Actions(driver);
                        actions.SendKeys(elemento, Keys.Space).Build().Perform();
                    }
                }else
                {
                    break;
                }
            }

            Thread.Sleep(3000);

            IWebElement Keystrokes = driver.FindElement(By.CssSelector("#keystrokes > td.value"));
            IWebElement Accuracy = driver.FindElement(By.CssSelector("#accuracy > td.value"));
            IWebElement Correct_words = driver.FindElement(By.CssSelector("#correct > td.value"));
            IWebElement Wrong_words = driver.FindElement(By.CssSelector("#wrong > td.value"));

            string connectionString = "Host=localhost;Port='5433';Username=postgres;Password=hnmcano;Database=postgres";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();


                string createTableSql = "CREATE TABLE IF NOT EXISTS BD (" +
                                        "id SERIAL PRIMARY KEY," +
                                        "Keystrokes CHARACTER(50)," +
                                        "Accuracy CHARACTER(50)," +
                                        "Correct_words CHARACTER(50)," +
                                        "Wrong_words CHARACTER(50))";

                string inserirDado = $"INSERT INTO BD (Keystrokes,Accuracy,Correct_words,Wrong_words) VALUES ('{Keystrokes.Text}','{Accuracy.Text}','{Correct_words.Text}','{Wrong_words.Text}')";

                using (NpgsqlCommand command = new NpgsqlCommand(createTableSql, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (NpgsqlCommand command = new NpgsqlCommand(inserirDado, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            driver.Quit();
        }
    }
}
