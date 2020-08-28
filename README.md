# GeoCoordinate.NetStandard2


GeoCoordinate is a Portable Class Library compatible implementation of System.Device.Location.GeoCoordinate. It is an exact 1:1 API compliant implementation and will be supported until MSFT [sees it fit to embed the type](https://visualstudio.uservoice.com/forums/121579-visual-studio-2015/suggestions/5221530-geocoordinate-class-included-in-portable-class-lib). Which at that point this implementation will cease development/support and you will be able to simply remove this package and everything will still work.

# Supported Platforms

* .NET Standard 2.1

# Installation
Installation is done via NuGet:

    PM> Install-Package GeoCoordinate.NetStandard2
    
# Usage

    GeoCoordinate pin1 = new GeoCoordinate(lat, lng);
    GeoCoordinate pin2 = new GeoCoordinate(lat, lng);
    
    double distanceBetween = pin1.GetDistanceTo(pin2);

For more examples, refer to the MSDN reference documentation over at: https://msdn.microsoft.com/en-us/library/system.device.location.geocoordinate(v=vs.110).aspx
