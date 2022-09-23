using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace TallestBuildingsTests;

public class TallestBuildingsListPage
{
    const string url = "https://www.skyscrapercenter.com/buildings?list=tallest100-completed";
    private readonly IWebDriver driver;

    private const string tallestBuildingsListXpath = "/html/body/div[1]/div[2]/div[2]/div[1]/div[2]/table";

    public TallestBuildingsListPage(IWebDriver driver)
    {
        this.driver = driver;
        PageFactory.InitElements(driver, this);
    }

    public void EnterPage()
    {
        driver.Navigate().GoToUrl(url);
        WaitForLoadToComplete();
    }

    private void WaitForLoadToComplete()
    {
        new WebDriverWait(driver, TimeSpan.FromSeconds(30))
            .Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("buildingsTable")));
    }

    [FindsBy(How = How.XPath, Using = $"{tallestBuildingsListXpath}/tbody/tr")]
    [CacheLookup]
    private IList<IWebElement> buildingTableRows;

    [FindsBy(How = How.XPath, Using = "//th/div/p[contains(text(),'Floors')]/../../preceding-sibling::th")]
    [CacheLookup]
    private IList<IWebElement> columnsBeforeFloorsColumn;

    [FindsBy(How = How.XPath, Using = "//th/div/p[contains(text(),'Name')]/../../preceding-sibling::th")]
    [CacheLookup]
    private IList<IWebElement> columnsBeforeNameColumn;

    public int BuildingTableRowCount => 
        buildingTableRows.Count;

    public int FloorsColumnIndex =>
        columnsBeforeFloorsColumn.Count + 1;

    public int NameColumnIndex =>
        columnsBeforeNameColumn.Count + 1;

    public int FindBuildingsFloorCountByName(string name)
    {
        var buildingsFloorsCellXpath = $"//td/p/a[contains(text(),'{name}')]/../../../td[{FloorsColumnIndex}]";
        var buildingsFloorsCell = driver.FindElement(By.XPath(buildingsFloorsCellXpath));

        return int.Parse(buildingsFloorsCell.GetDomAttribute("data-order"));
    }

    public string FindBuildingsNameByFloorCount(int floors)
    {
        var buildingsNameCellXpath = $"//td[@data-order = '{floors}']/../td[{NameColumnIndex}]/p/a";
        var buildingsNameCell = driver.FindElement(By.XPath(buildingsNameCellXpath));

        return buildingsNameCell.Text;
    }

    public int MaxFloorCellValue()
    {
        var floorCells = driver.FindElements(By.XPath($"//td[{FloorsColumnIndex}]"));

        return floorCells.Select(c => int.Parse(c.GetDomAttribute("data-order")))
            .Max();
    }
}
