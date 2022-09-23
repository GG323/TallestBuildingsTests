using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace TallestBuildingsTests;

[TestFixture("MicrosoftEdge")]
[TestFixture("chrome")]
[Parallelizable(ParallelScope.All)]
public class TallestBuildingsList100Tests
{
    private ThreadLocal<IWebDriver> driver;
    private string browser;

    public TallestBuildingsList100Tests(string browser)
    {
        this.browser = browser;
        driver = new ThreadLocal<IWebDriver>();
    }

    [SetUp]
    public void Setup()
    {
        switch (browser)
        {
            case "MicrosoftEdge":
            {
                var options = new EdgeOptions();
                driver.Value = new EdgeDriver("../../../Drivers", options, TimeSpan.FromSeconds(30));
                break;
            }
            case "chrome":
            {
                var options = new ChromeOptions();
                driver.Value = new ChromeDriver("../../../Drivers", options, TimeSpan.FromSeconds(30));
                break;
            }
        }

        Thread.Sleep(2000);
    }


    [TearDown]
    public void Teardown()
    {
        driver.Value?.Close();
    }

    [Test]
    public void BuildingsList_ShouldBe100RowsLong()
    {
        const int expectedTableRowCount = 100;

        var page = new TallestBuildingsListPage(driver.Value);
        page.EnterPage();

        Assert.AreEqual(expectedTableRowCount, page.BuildingTableRowCount);
    }

    [Test]
    public void LotteWorldTower_ShouldHave123Floors()
    {
        const int expectedLotteWorldTowerFloorCount = 123;
        const string buildingsName = "Lotte World Tower";

        var page = new TallestBuildingsListPage(driver.Value);
        page.EnterPage();

        Assert.AreEqual(expectedLotteWorldTowerFloorCount, page.FindBuildingsFloorCountByName(buildingsName));
    }

    [Test]
    public void FindTheBuildingWithMostFloors()
    {
        var page = new TallestBuildingsListPage(driver.Value);
        page.EnterPage();

        var floorCellValues = page.MaxFloorCellValue();

        Assert.Pass(page.FindBuildingsNameByFloorCount(floorCellValues));
    }
}
