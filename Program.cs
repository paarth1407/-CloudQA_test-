using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;

namespace CloudQATest
{
    [TestFixture]
    public class FormAutomationTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        
        [SetUp]
        public void Initialize()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-blink-features=AutomationControlled");
            options.AddArguments("--no-sandbox");
            
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TestFormFields()
        {
            driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");
            Thread.Sleep(3000);
            
            TestFirstNameField();
            TestEmailField(); 
            TestPhoneNumberField();
            
            Console.WriteLine("All tests completed successfully!");
        }

        private void TestFirstNameField()
        {
            Console.WriteLine("Testing first name field...");
            
            IWebElement nameField = FindFirstNameElement();
            
            if (nameField != null)
            {
                nameField.Clear();
                nameField.SendKeys("John Smith");
                
                string value = nameField.GetAttribute("value");
                Assert.AreEqual("John Smith", value);
                Console.WriteLine("First name field test passed");
            }
            else
            {
                Assert.Fail("Couldn't find the first name field");
            }
        }

        private void TestEmailField()
        {
            Console.WriteLine("Testing email field...");
            
            IWebElement emailField = FindEmailElement();
            
            if (emailField != null)
            {
                emailField.Clear();
                emailField.SendKeys("test@example.com");
                
                string value = emailField.GetAttribute("value");
                Assert.AreEqual("test@example.com", value);
                Console.WriteLine("Email field test passed");
            }
            else
            {
                Assert.Fail("Couldn't find the email field");
            }
        }

        private void TestPhoneNumberField()
        {
            Console.WriteLine("Testing phone field...");
            
            IWebElement phoneField = FindPhoneElement();
            
            if (phoneField != null)
            {
                phoneField.Clear();
                phoneField.SendKeys("9876543210");
                
                string value = phoneField.GetAttribute("value");
                Assert.AreEqual("9876543210", value);
                Console.WriteLine("Phone field test passed");
            }
            else
            {
                Assert.Fail("Couldn't find the phone field");
            }
        }

        private IWebElement FindFirstNameElement()
        {
            List<By> strategies = new List<By>
            {
                By.Id("firstName"),
                By.Id("fname"), 
                By.Name("firstName"),
                By.XPath("//input[@placeholder='First Name']"),
                By.XPath("//input[@placeholder='Name']"),
                By.CssSelector("input[placeholder*='First']"),
                By.CssSelector("input[placeholder*='Name']"),
                By.XPath("//input[contains(@class,'form-control')][@type='text'][1]")
            };

            return TryFindElement(strategies);
        }

        private IWebElement FindEmailElement()
        {
            List<By> strategies = new List<By>
            {
                By.Id("userEmail"),
                By.Id("email"),
                By.Name("email"),
                By.XPath("//input[@type='email']"),
                By.XPath("//input[@placeholder='Email']"),
                By.CssSelector("input[type='email']"),
                By.CssSelector("input[placeholder*='email']"),
                By.XPath("//input[contains(@placeholder,'Email')]")
            };

            return TryFindElement(strategies);
        }

        private IWebElement FindPhoneElement()
        {
            List<By> strategies = new List<By>
            {
                By.Id("userNumber"),
                By.Id("mobile"),
                By.Id("phone"),
                By.Name("mobile"),
                By.XPath("//input[@type='tel']"),
                By.XPath("//input[@placeholder='Mobile Number']"),
                By.XPath("//input[contains(@placeholder,'Mobile')]"),
                By.XPath("//input[contains(@placeholder,'Phone')]"),
                By.CssSelector("input[placeholder*='Mobile']")
            };

            return TryFindElement(strategies);
        }

        private IWebElement TryFindElement(List<By> locators)
        {
            foreach (By locator in locators)
            {
                try
                {
                    var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                    if (element.Displayed && element.Enabled)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                        Thread.Sleep(500);
                        return element;
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    continue;
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }
            return null;
        }

        [TearDown]
        public void Cleanup()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}