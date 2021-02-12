## Getting osm map data
Of the many choices, I went with a geofabrik and used an extract of Colorado.  The [planet](https://wiki.openstreetmap.org/wiki/Planet.osm) is 54G compressed, the pbf for [Colorado](https://download.geofabrik.de/north-america/us/colorado.html#) is around 200M.


## Starting up PostGis and PGAdmin in Docker
Use a linux machine, [WSL in windows 10](https://docs.microsoft.com/en-us/windows/wsl/install-win10) or just normal Docker Desktop for Windows should work.  You need both Docker and Docker Compose, they are seperate installs in Linux.
https://get.docker.com/

https://docs.docker.com/compose/install/

**postgis-compose.yml**
```yaml
version: "3.7"
services:
  postgisdb:
    image: postgis/postgis:latest
    environment:
      #choose a db name
      name: something-postgis
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: somepassword
      PGDATA: /var/lib/postgresql/data
    volumes:
      #this is mapping the data dir in the container to a local directory pgisdata
      # you may need to add permissions on the dir
      - ./pgisdata:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    
  pgadmin:
    image: dpage/pgadmin4:latest
    environment:
      #This appears to need to be a valid email format
      #The email doesnt need to be valid
      #you need this to log in to the admin site
      PGADMIN_DEFAULT_EMAIL: something@something.com
      PGADMIN_DEFAULT_PASSWORD: somepassword
      PGADMIN_LISTEN_PORT: 80
    ports:
      - "8080:80"
    volumes:
    #this is mapping the data dir in the container to a local directory pgadmindata
    #you may need to add special permissions 
    #run this in the same dir you started the compose file
    #sudo chown -R 5050:5050
      - ./pgadmindata:/var/lib/pgadmin
    links:
      - "postgisdb:postgis-server"
```

you can start the docker compose group with:

```docker-compose -f postgis-compose.yml up -d```

(When first running it you may want watch for errors in the logs in the console window, to do that leave off the -d)

and to stop the containers:

```docker-compose -f postgis-compose.yml down```

The PGAdmin site should be available at port 8080 (if you are already running something on that port, you will need to set a different one in the compose file), you will need to log in with whatever email and credentials you have in the compose file. If you are running it locally on your windows machine with wsl or docker, it should be available at http://localhost:8080

The Postgis db will be have the network name postgis-server, due to this:
```yaml
 links:
      - "postgisdb:postgis-server" 
```
in PGAdmin you need to add a connection to the db before you can do anything with it. Use postgis-server as the host, 5432 as the port and you will need to use the db login credentials you set in the docker compose file.

Log into the PGAdmin site , from the tools menu open a query tool tab, then run:

``` CREATE EXTENSION postgis ```

(This may not always be necessary, but I had to do it. It adds the required extensions to the db for spatial data.)

If you are running it locally on your windows machine with wsl or docker, the db should be available at http://localhost:5432 this is the address you would use when adding a postgis connection in Qgis for example.

## Importing the osm pbf file into postgis
(this is done in linux)

If you dont have osm2pgsql installed, install it with this:

``` sudo apt install osm2pgsql ```

To start the import, run this using the db user you specified in the docker compose file, it should also prompt you for a password :

``` osm2pgsql -d postgres -U admin -W -H localhost colorado-latest.osm.pbf ```

(The 200M Colorado file took around 3 minutes to import on the old machine I was using for the db)

## Creating Views for the different sets of data

The raw osm data probably has a lot more in it than you will want to see, so you may want to filter it down to only render the stuff you care about.  There are other ways to go about this, but I went with creating a views on the db, then pulled in the views in qgis when creating layers.  

The one thing that got me when doing this, is Qgis requires a unique id as the first column, and it will bomb when adding the layer if this is not the case. (of course it didn't have any clue in the error message that that's why it was failing). 

The osm_id that is there after importing wasn't unique for me for whatever technical reasons that I couldn't be bothered with investigating. You can add your own unique id on your views by just adding the row number as your first column:

``` row_number() over () as id, ```

I created views for each of the different layers I wanted, I didnt spend that much time thinking about the best way to do it, and I'm sure later I'll be doing it differently to give the ability to show/hide different sets of data. 
I included these in the repo:
osm_line_actualroads
osm_line_tracks
osm_line_waterway
osm_point_barriers
osm_polygon_water

For the most part the osm data follows some loose standards, but some of it can be kind of messy as you would expect when anyone can add data. For example there is a field for surface which has some normal values like "paved", "unpaved", "gravel", but then some people can't decide what to put in so they get creative with "ground;dirt", "bad", or "yes". 

just take a look:
select surface, count(*) from planet_osm_line group by surface

Styling this using the most common values should work 99% of the time, as it looks like records with the odd entries are rare in comparison.

Fyi, when you create the views, your sql of:

```sql
select row_number() OVER () AS id, * from public.planet_osm_line where highway in ('cycleway','path','track','unclassified')
```
changes to something like 

```sql
SELECT row_number() OVER () AS id,
    planet_osm_line.osm_id,
    planet_osm_line.access,
    planet_osm_line."addr:housename",
    --...(all the columns)
    planet_osm_line.way
   FROM planet_osm_line
  WHERE (planet_osm_line.highway = ANY (ARRAY['cycleway'::text, 'path'::text, 'track'::text, 'unclassified'::text]));
```
so you may want to keep the smaller original sql around for editing later.


## Using Qgis to connect to postgis and create layers from the views
[Get and install Qgis](https://qgis.org/en/site/index.html)
In the Browser toolbox window right click on PostGis and add new connection, the address will be something like localhost:5432, Database will be **postgres** and login credentials will be whatever you have in the compose file.
The views should show along with the original tables they are based on.  Drag one of the views over into the map content window to create a new layer from the view.
The layer content should be drawn with a single color in the map content view. 


## Adding styles to the layers
 The new layer should also be listed in the layers toolbox window.  You can Change the style for layer by right clicking on it, select properties.
In the content for the Symbology tab, at the top there is a dropdown that defaults to "single symbol" which can be changed to rule-based. then you can add rules for what styles apply to the different values for the data in the layer.  
Example: for the National forest boundry rule to set the color I used
```
"operator" =  'United States Forest Service' OR "operator" =  'US Forest Service'
```
An important thing you will also probably want to set in a rule is the **scale range** so you don't try to draw everything when zoomed out to far for it to be useful.
Example: for tracks I have the minimum set to 1:463593 and max set to 1:500. This only draws the roads when you are zoomed in close enough to be able to make use of them.

## Processing the map into a raster mbtiles file
In the **Processing Toolbox** expand **Raster Tools**, Select **Generate XYZ tiles (MBTiles)**
- Extent: is the area it will render, you can use Map Canvas extent to just to the whole map
- Min, Max zoom: important to not over do it, as each zoom level you go in will increase the size and processing time quite a bit.  I used max:16, min:10
Output file: give it a path and file name or it will just put it some default location


## Some notes on mapsui and how to open map files in the Xamarin forms app
[Mapsui](https://github.com/pauldendulk/Mapsui) currently only supports raster tiles, thus all the messing around converting the osm pbf file.

To include the map file in your project and provide sqllite the path to it at runtime you need to do a few things differently than you might expect coming from a normal c# .net app standpoint. I was at first expecting to just include the file in the output directory and then point to it in the code that gives sqllite its db file path, but nothing I did would put the file in the apps directlry. Appearently in Xamarin forms this is normal, what you need to do instead is read the file and write it to a location your app has access to, then point sqllite to that file.  In the project, the file should be set as Embedded resource.  You can then read it and write it somewhere else. note you need to prefix the filename with the ProjectName. when reading it as an embedded resource. Apart from this everything else I've used with Mapsui worked as expected.

```c#
var basepath = @"." + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var file = "co-test.mbtiles";
var fullpath = Path.Combine(basepath, file);

var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"TrackDataDroid.co-full-test-2.mbtiles");


using (BinaryReader br = new BinaryReader(stream))
{
    using (BinaryWriter bw = new BinaryWriter(new FileStream(fullpath, FileMode.Create)))
    {
        byte[] buffer = new byte[2048];
        int len = 0;
        while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
        {
            bw.Write(buffer, 0, len);
        }
    }
}
```







