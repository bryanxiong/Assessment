using NUnit.Framework;
using System;
using System.Threading;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Assessment;

public class Main
{
    const string url = "http://127.0.0.1:4723/wd/hub";
    AppiumDriver<AndroidElement> driver;

    [SetUp]
    public void Setup()
    {
        AppiumOptions appCapabilities = new AppiumOptions();
        appCapabilities.AddAdditionalCapability("deviceName", "R5CT448SESJ");
        appCapabilities.AddAdditionalCapability("platformName", "Android");
        appCapabilities.AddAdditionalCapability("platformVersion", "12.0");
        appCapabilities.AddAdditionalCapability("appPackage", "com.cie.qatest");
        appCapabilities.AddAdditionalCapability("appActivity", "crc64107c44d0e5f5b2d9.MainActivity");

        driver = new AndroidDriver<AndroidElement>(new Uri(url), appCapabilities);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(3000); // wait before quiting
        driver.Quit();
    }

    [Test]
    public void AddMovieToFavorites()
    {
        var view = driver.FindElementByClassName("androidx.recyclerview.widget.RecyclerView");      // Change view to be all elements within current element 
        var getMovies = view.FindElementsByClassName("android.view.ViewGroup");                     // get all elements that are of classname ViewGroup

        // For debugging and troubleshooting
        /*
        int i = 0;
        foreach (var g in getMovies)
        {
            TestContext.Progress.WriteLine(i + " index contains " + g.FindElementsByClassName("android.widget.TextView")[1].Text);
            i++;
        }
        */

        var getTitle = getMovies[0].FindElementsByClassName("android.widget.TextView")[1].Text;     // Get the title of the 1st Movie in list
        //var getTitle = getMovies[22].FindElementsByClassName("android.widget.TextView")[1].Text;    // Fail Case, since this checks the 22nd element's title

        getMovies[0].Click();                                                                       // Click on the 1st Movie in list

        driver.FindElementByAccessibilityId("Favorite").Click();                                    // Click on Favorite icon
        driver.FindElementById("android:id/button2").Click();                                       // Click OK on confirmation popup

        driver.FindElementById("com.cie.qatest:id/navigation_bar_item_small_label_view").Click();   // Click on Favorites tab-based navigation
        
        // TestContext.Progress.WriteLine("Title selected is: " + getTitle + " vs: " + driver.FindElementsByClassName("android.widget.TextView")[2].Text);
        
        Assert.AreEqual(getTitle, driver.FindElementsByClassName("android.widget.TextView")[2].Text);   // Check if both strings are equal, if so then test passes
    }
}