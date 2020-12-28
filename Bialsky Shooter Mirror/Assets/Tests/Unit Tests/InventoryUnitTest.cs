using NUnit.Framework;
using BialskyShooter.InventoryModule;
using Zenject;
using UnityEngine;

[TestFixture]
public class InventoryUnitTest : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        //Container.Bind<Inventory>().FromComponentInNewPrefab(playerPrefab);
        //Container.Inject(this);
    }

    //[Inject] Inventory inventory;

    [Test]
    public void RunPickupItemTest()
    {
        Assert.AreEqual(1, 1);
    }
}