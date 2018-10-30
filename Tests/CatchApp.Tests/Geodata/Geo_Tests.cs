using CatchApp.Geodata;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CatchApp.Tests.Geodata
{

    public class Geo_Tests
    {
        [Fact]
        public static void Check_Distance()
        {
            GeoPoint point1 = new GeoPoint(46.599550, 14.270328);
            GeoPoint point2 = new GeoPoint(47.051497, 15.452759);

            Assert.Equal(103.04, Geo.Distance(point1, point2), 2);

            //point1 = new GeoPoint(46.619906, 14.304134);
            //point2 = new GeoPoint(46.599582, 14.270487);

            //DbGeography geoPoint1 = DbGeography.FromText(point1.ToString());
            //DbGeography geoPoint2 = DbGeography.FromText(point2.ToString());
            //double? distance = geoPoint1.Distance(geoPoint2);

            //Assert.Equal(103.04, Geo.Distance(point1, point2), 2);

        }
    }
}
