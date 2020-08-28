using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoCoordinate.NetStandard2.Tests
{
    public static class Assert
    {
        public static T Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex is T exOfT)
                    return exOfT;

                throw;
            }

            throw new AssertFailedException("Expected to throw " + typeof(T).Name + ".");
        }
    }
}
