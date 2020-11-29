 SELECT row_number() OVER () AS id,
    planet_osm_polygon.osm_id,
    planet_osm_polygon.access,
    planet_osm_polygon."addr:housename",
    planet_osm_polygon."addr:housenumber",
    planet_osm_polygon."addr:interpolation",
    planet_osm_polygon.admin_level,
    planet_osm_polygon.aerialway,
    planet_osm_polygon.aeroway,
    planet_osm_polygon.amenity,
    planet_osm_polygon.area,
    planet_osm_polygon.barrier,
    planet_osm_polygon.bicycle,
    planet_osm_polygon.brand,
    planet_osm_polygon.bridge,
    planet_osm_polygon.boundary,
    planet_osm_polygon.building,
    planet_osm_polygon.construction,
    planet_osm_polygon.covered,
    planet_osm_polygon.culvert,
    planet_osm_polygon.cutting,
    planet_osm_polygon.denomination,
    planet_osm_polygon.disused,
    planet_osm_polygon.embankment,
    planet_osm_polygon.foot,
    planet_osm_polygon."generator:source",
    planet_osm_polygon.harbour,
    planet_osm_polygon.highway,
    planet_osm_polygon.historic,
    planet_osm_polygon.horse,
    planet_osm_polygon.intermittent,
    planet_osm_polygon.junction,
    planet_osm_polygon.landuse,
    planet_osm_polygon.layer,
    planet_osm_polygon.leisure,
    planet_osm_polygon.lock,
    planet_osm_polygon.man_made,
    planet_osm_polygon.military,
    planet_osm_polygon.motorcar,
    planet_osm_polygon.name,
    planet_osm_polygon."natural",
    planet_osm_polygon.office,
    planet_osm_polygon.oneway,
    planet_osm_polygon.operator,
    planet_osm_polygon.place,
    planet_osm_polygon.population,
    planet_osm_polygon.power,
    planet_osm_polygon.power_source,
    planet_osm_polygon.public_transport,
    planet_osm_polygon.railway,
    planet_osm_polygon.ref,
    planet_osm_polygon.religion,
    planet_osm_polygon.route,
    planet_osm_polygon.service,
    planet_osm_polygon.shop,
    planet_osm_polygon.sport,
    planet_osm_polygon.surface,
    planet_osm_polygon.toll,
    planet_osm_polygon.tourism,
    planet_osm_polygon."tower:type",
    planet_osm_polygon.tracktype,
    planet_osm_polygon.tunnel,
    planet_osm_polygon.water,
    planet_osm_polygon.waterway,
    planet_osm_polygon.wetland,
    planet_osm_polygon.width,
    planet_osm_polygon.wood,
    planet_osm_polygon.z_order,
    planet_osm_polygon.way_area,
    planet_osm_polygon.way
   FROM planet_osm_polygon
  WHERE (planet_osm_polygon.boundary = ANY (ARRAY['national_park'::text, 'protected_area'::text, 'administrative'::text, 'fence'::text]));