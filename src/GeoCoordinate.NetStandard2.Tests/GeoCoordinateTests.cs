using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTesting = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoCoordinate.NetStandard2.Tests
{
    [TestClass]
    public class GeoCoordinateTests
    {
        private GeoCoordinate _unitUnderTest;

        [TestMethod]
        public void GeoCoordinate_ConstructorWithDefaultValues_DoesNotThrow()
        {
            _unitUnderTest = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);
        }

        [TestMethod]
        public void GeoCoordinate_ConstructorWithParameters_ReturnsInstanceWithExpectedValues()
        {
            const double latitude = 42D;
            const double longitude = 44D;
            const double altitude = 46D;
            const double horizontalAccuracy = 48D;
            const double verticalAccuracy = 50D;
            const double speed = 52D;
            const double course = 54D;
            const bool isUnknown = false;
            _unitUnderTest = new GeoCoordinate(latitude, longitude, altitude, horizontalAccuracy, verticalAccuracy, speed, course);

            UnitTesting.Assert.AreEqual(latitude, _unitUnderTest.Latitude);
            UnitTesting.Assert.AreEqual(longitude, _unitUnderTest.Longitude);
            UnitTesting.Assert.AreEqual(altitude, _unitUnderTest.Altitude);
            UnitTesting.Assert.AreEqual(horizontalAccuracy, _unitUnderTest.HorizontalAccuracy);
            UnitTesting.Assert.AreEqual(verticalAccuracy, _unitUnderTest.VerticalAccuracy);
            UnitTesting.Assert.AreEqual(speed, _unitUnderTest.Speed);
            UnitTesting.Assert.AreEqual(course, _unitUnderTest.Course);
            UnitTesting.Assert.AreEqual(isUnknown, _unitUnderTest.IsUnknown);
        }

        [TestMethod]
        public void GeoCoordinate_DefaultConstructor_ReturnsInstanceWithDefaultValues()
        {
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.Altitude);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.Course);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.HorizontalAccuracy);
            UnitTesting.Assert.IsTrue(_unitUnderTest.IsUnknown);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.Latitude);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.Longitude);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.Speed);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.VerticalAccuracy);
        }

        [TestMethod]
        public void GeoCoordinate_EqualsOperatorWithNullParameters_DoesNotThrow()
        {
            GeoCoordinate first = null;
            GeoCoordinate second = null;
            UnitTesting.Assert.IsTrue(first == second);

            first = new GeoCoordinate();
            UnitTesting.Assert.IsFalse(first == second);

            first = null;
            second = new GeoCoordinate();
            UnitTesting.Assert.IsFalse(first == second);
        }

        [TestMethod]
        public void GeoCoordinate_EqualsTwoInstancesWithDifferentValuesExceptLongitudeAndLatitude_ReturnsTrue()
        {
            var first = new GeoCoordinate(11, 12, 13, 14, 15, 16, 17);
            var second = new GeoCoordinate(11, 12, 14, 15, 16, 17, 18);

            UnitTesting.Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void GeoCoordinate_EqualsTwoInstancesWithSameValues_ReturnsTrue()
        {
            var first = new GeoCoordinate(11, 12, 13, 14, 15, 16, 17);
            var second = new GeoCoordinate(11, 12, 13, 14, 15, 16, 17);

            UnitTesting.Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void GeoCoordinate_EqualsWithOtherTypes_ReturnsFalse()
        {
            dynamic something = 42;
            UnitTesting.Assert.IsFalse(_unitUnderTest.Equals(something));
        }

        [TestMethod]
        public void GeoCoordinate_GetDistanceTo_ReturnsExpectedDistance()
        {
            var start = new GeoCoordinate(1, 1);
            var end = new GeoCoordinate(5, 5);
            var distance = start.GetDistanceTo(end);
            const double expected = 629060.759879635;
            var delta = distance - expected;

            UnitTesting.Assert.IsTrue(delta < 1e-8);
        }

        [TestMethod]
        public void GeoCoordinate_GetDistanceToWithNaNCoordinates_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new GeoCoordinate(double.NaN, 1).GetDistanceTo(new GeoCoordinate(5, 5)));
            Assert.Throws<ArgumentException>(() => new GeoCoordinate(1, double.NaN).GetDistanceTo(new GeoCoordinate(5, 5)));
            Assert.Throws<ArgumentException>(() => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(double.NaN, 5)));
            Assert.Throws<ArgumentException>(() => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(5, double.NaN)));
        }

        [TestMethod]
        public void GeoCoordinate_GetHashCode_OnlyReactsOnLongitudeAndLatitude()
        {
            _unitUnderTest.Latitude = 2;
            _unitUnderTest.Longitude = 3;
            var firstHash = _unitUnderTest.GetHashCode();

            _unitUnderTest.Altitude = 4;
            _unitUnderTest.Course = 5;
            _unitUnderTest.HorizontalAccuracy = 6;
            _unitUnderTest.Speed = 7;
            _unitUnderTest.VerticalAccuracy = 8;
            var secondHash = _unitUnderTest.GetHashCode();

            UnitTesting.Assert.AreEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void GeoCoordinate_GetHashCode_SwitchingLongitudeAndLatitudeReturnsSameHashCodes()
        {
            _unitUnderTest.Latitude = 2;
            _unitUnderTest.Longitude = 3;
            var firstHash = _unitUnderTest.GetHashCode();

            _unitUnderTest.Latitude = 3;
            _unitUnderTest.Longitude = 2;
            var secondHash = _unitUnderTest.GetHashCode();

            UnitTesting.Assert.AreEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void GeoCoordinate_IsUnknownIfLongitudeAndLatitudeIsNaN_ReturnsTrue()
        {
            _unitUnderTest.Longitude = 1;
            _unitUnderTest.Latitude = double.NaN;
            UnitTesting.Assert.IsFalse(_unitUnderTest.IsUnknown);

            _unitUnderTest.Longitude = double.NaN;
            _unitUnderTest.Latitude = 1;
            UnitTesting.Assert.IsFalse(_unitUnderTest.IsUnknown);

            _unitUnderTest.Longitude = double.NaN;
            _unitUnderTest.Latitude = double.NaN;
            UnitTesting.Assert.IsTrue(_unitUnderTest.IsUnknown);
        }

        [TestMethod]
        public void GeoCoordinate_NotEqualsOperatorWithNullParameters_DoesNotThrow()
        {
            GeoCoordinate first = null;
            GeoCoordinate second = null;
            UnitTesting.Assert.IsFalse(first != second);

            first = new GeoCoordinate();
            UnitTesting.Assert.IsTrue(first != second);

            first = null;
            second = new GeoCoordinate();
            UnitTesting.Assert.IsTrue(first != second);
        }

        [TestMethod]
        public void GeoCoordinate_SetAltitude_ReturnsCorrectValue()
        {
            UnitTesting.Assert.AreEqual(_unitUnderTest.Altitude, double.NaN);

            _unitUnderTest.Altitude = 0;
            UnitTesting.Assert.AreEqual(0, _unitUnderTest.Altitude);

            _unitUnderTest.Altitude = double.MinValue;
            UnitTesting.Assert.AreEqual(double.MinValue, _unitUnderTest.Altitude);

            _unitUnderTest.Altitude = double.MaxValue;
            UnitTesting.Assert.AreEqual(double.MaxValue, _unitUnderTest.Altitude);
        }

        [TestMethod]
        public void GeoCoordinate_SetCourse_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.Course = -0.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.Course = 360.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, -0.1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, 360.1));
        }

        [TestMethod]
        public void GeoCoordinate_SetHorizontalAccuracy_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.HorizontalAccuracy = -0.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(double.NaN, double.NaN, double.NaN, -0.1, double.NaN, double.NaN, double.NaN));
        }

        [TestMethod]
        public void GeoCoordinate_SetHorizontalAccuracyToZero_ReturnsNaNInProperty()
        {
            _unitUnderTest = new GeoCoordinate(double.NaN, double.NaN, double.NaN, 0, double.NaN, double.NaN, double.NaN);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.HorizontalAccuracy);
        }

        [TestMethod]
        public void GeoCoordinate_SetLatitude_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.Latitude = 90.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.Latitude = -90.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(90.1, double.NaN));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(-90.1, double.NaN));
        }

        [TestMethod]
        public void GeoCoordinate_SetLongitude_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.Longitude = 180.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.Longitude = -180.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(double.NaN, 180.1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(double.NaN, -180.1));
        }

        [TestMethod]
        public void GeoCoordinate_SetSpeed_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.Speed = -0.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, -1, double.NaN));
        }

        [TestMethod]
        public void GeoCoordinate_SetVerticalAccuracy_ThrowsOnInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _unitUnderTest.VerticalAccuracy = -0.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, -0.1, double.NaN, double.NaN));
        }

        [TestMethod]
        public void GeoCoordinate_SetVerticalAccuracyToZero_ReturnsNaNInProperty()
        {
            _unitUnderTest = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, 0, double.NaN, double.NaN);
            UnitTesting.Assert.AreEqual(double.NaN, _unitUnderTest.VerticalAccuracy);
        }

        [TestMethod]
        public void GeoCoordinate_ToString_ReturnsLongitudeAndLatitude()
        {
            UnitTesting.Assert.AreEqual("Unknown", _unitUnderTest.ToString());

            _unitUnderTest.Latitude = 1;
            _unitUnderTest.Longitude = double.NaN;
            UnitTesting.Assert.AreEqual("1, NaN", _unitUnderTest.ToString());

            _unitUnderTest.Latitude = double.NaN;
            _unitUnderTest.Longitude = 1;
            UnitTesting.Assert.AreEqual("NaN, 1", _unitUnderTest.ToString());
        }

        [TestInitialize]
        public void Initialize()
        {
            _unitUnderTest = new GeoCoordinate();
        }
    }
}