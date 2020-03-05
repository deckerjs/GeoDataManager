import { AreaBounds } from '../models/area-bounds';
import { LatLngPoint } from '../models/lat-lng-point';

// import { LatLngBoundsLiteral } from "@agm/core";

export class geojsonHelperFunctions {

  //LatLngBoundsLiteral
  public static getBounds(gj: any): AreaBounds {
    {
      var coords, bbox;
      if (!gj.hasOwnProperty("type")) return;
      coords = getCoordinatesDump(gj);
      bbox = [
        Number.POSITIVE_INFINITY,
        Number.POSITIVE_INFINITY,
        Number.NEGATIVE_INFINITY,
        Number.NEGATIVE_INFINITY
      ];
      let boundsArray = coords.reduce(function (prev, coord) {
        return [
          Math.min(coord[0], prev[0]),
          Math.min(coord[1], prev[1]),
          Math.max(coord[0], prev[2]),
          Math.max(coord[1], prev[3])
        ];
      }, bbox);
      let areaBounds = new AreaBounds();
      //return {"east":boundsArray[0], "north":boundsArray[1], "west":boundsArray[2], "south":boundsArray[3]}

      areaBounds.east = boundsArray[0];
      areaBounds.north = boundsArray[1];
      areaBounds.west = boundsArray[2];
      areaBounds.south = boundsArray[3];

      return areaBounds;
    }

    function getCoordinatesDump(gj) {
      var coords;
      if (gj.type == "Point") {
        coords = [gj.coordinates];
      } else if (gj.type == "LineString" || gj.type == "MultiPoint") {
        coords = gj.coordinates;
      } else if (gj.type == "Polygon" || gj.type == "MultiLineString") {
        coords = gj.coordinates.reduce(function (dump, part) {
          return dump.concat(part);
        }, []);
      } else if (gj.type == "MultiPolygon") {
        coords = gj.coordinates.reduce(function (dump, poly) {
          return dump.concat(
            poly.reduce(function (points, part) {
              return points.concat(part);
            }, [])
          );
        }, []);
      } else if (gj.type == "Feature") {
        coords = getCoordinatesDump(gj.geometry);
      } else if (gj.type == "GeometryCollection") {
        coords = gj.geometries.reduce(function (dump, g) {
          return dump.concat(getCoordinatesDump(g));
        }, []);
      } else if (gj.type == "FeatureCollection") {
        coords = gj.features.reduce(function (dump, f) {
          return dump.concat(getCoordinatesDump(f));
        }, []);
      }

      return coords;
    }

  }

  public static getCenter(areaBounds: AreaBounds): LatLngPoint {
    let center = new LatLngPoint();
    center.lat = (areaBounds.north + areaBounds.south) / 2;
    center.lng = (areaBounds.east + areaBounds.west) / 2;
    return center;
  }

}
