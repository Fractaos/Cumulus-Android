using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Cumulus.UnitTests
{
    public class UtilsTests
    {
        [Test]
        public void GetPositionOnScreenByPercent_CenterOfScreen_IsTheCenter()
        {
            const float x = .5f;
            const float y = .5f;

            Vector2 result = Utils.GetPositionOnScreenByPercent(x, y);

            Assert.That(result, Is.EqualTo(new Vector2(Utils.CENTER_SCREEN_HORIZONTAL, Utils.CENTER_SCREEN_VERTICAL)));
        }
    }
}