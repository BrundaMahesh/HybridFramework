using Google.Selenium_Tests.Pages;
using Google.Test_Data_Classes;
using Google.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.Selenium_Tests.Tests
{
    internal class HomePageTests : BasePage
    {
        private HomePage homePage;
        string? testName;

        [SetUp]
        public void BeforeTest()
        {
            homePage = new HomePage(Driver);
        }

        [Test]
        public void GoogleSearchTest()
        {

            testName = "Google Search Test";
            Log.Information(testName);
            Log.Information("************************************************");
            Test = Extent.CreateTest(testName);


            string? excelFilePath = currdir + "/Test Data/Google Data.xlsx";
            string? sheetName = "Search Data";

            List<SearchData> excelDataList = SearchDataRead.ReadSearchText(excelFilePath, sheetName);

            foreach (var excelData in excelDataList)
            {
                string? searchText = excelData.SearchText;


                LogTestResult(testName, "Info", "Starting the Google search test");

                LogTestResult(testName, "Info", "Opened Google homepage");

                homePage.EnterSearchText(searchText);
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                TakeScreenShot();
                LogTestResult(testName, "Info", $"Entered search text: {searchText}");

                homePage.ClickSearchButton();
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                TakeScreenShot();
                LogTestResult(testName, "Info", "Clicked on the Search button");

                try
                {
                    // Assume the title contains the search text for simplicity
                    Assert.That(Driver.Title, Does.Contain(searchText), $"Search results page title does not contain '{searchText}'");
                    LogTestResult(testName, "Info", "Google search test completed");
                    LogTestResult(testName, "pass", testName + " - Passed");
                }
                catch (Exception ex)
                {
                    LogTestResult(testName, "fail", testName + " - Failed", ex.Message);
                }
                finally
                {
                    Driver.Navigate().GoToUrl(url);
                }
                // Additional assertions or actions if needed
            }
        }

        [TearDown]
        public void AfterTest()
        {
            Driver.Navigate().GoToUrl(url);
            Log.Information("************************************************");
        }
    }
}
///