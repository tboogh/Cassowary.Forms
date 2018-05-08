using Cassoway.Forms.Layout;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms;

namespace Cassowary.Forms.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var layout = new CassowaryLayout();
            var view = new Label();
            layout.Children.Add(view);
            
            var rect = new Rectangle(0, 0, 400, 400);
            layout.Layout(rect);
            
            Assert.AreEqual(rect.Right, view.Bounds.Right);
        }
    }
}