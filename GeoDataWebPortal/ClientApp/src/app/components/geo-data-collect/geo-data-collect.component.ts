import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-geo-data-collect',
  templateUrl: './geo-data-collect.component.html',
  styleUrls: ['./geo-data-collect.component.scss']
})
export class GeoDataCollectComponent implements OnInit {


  public locations: Array<Position> = new Array<Position>();
  // public locations: any = {};
  private watchId: number;

  constructor() { }

  ngOnInit(): void {
  }

  public startWatch(): void {
    if (navigator.geolocation) {
      this.watchId = navigator.geolocation.watchPosition((position) => {
        console.log("******* navigator.geolocation.watchPosition :", position);
        this.locations.push(position);

        // this won't work. I hate javascript.
        // const locationsJson = JSON.stringify(this.locations);
        // console.log("******* this.locations :", this.locations);
        // console.log("******* locationsJson :", locationsJson);
        // localStorage.setItem('watchId'+this.watchId, locationsJson);
      },
        err => {
          console.log("watchPosition error:", err)
        },
        {
          timeout: 6000,
          enableHighAccuracy: true,
          maximumAge: Infinity
        });
    } else {
      console.log("no navigator.geolocation: ", navigator)
    }
  }

  public stopWatch(): void {
    navigator.geolocation.clearWatch(this.watchId);
  }

  public addCurrentLocation(): void {

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        console.log("******* navigator.geolocation.getCurrentPosition :", position);
        this.locations.push(position);
      },
        err => {
          console.log("getCurrentPosition error:", err)
        },
        {
          timeout: 6000,
          enableHighAccuracy: true,
          maximumAge: Infinity
        });
    } else {
      console.log("no navigator.geolocation: ", navigator)
    }

  }


}


// /** The position and altitude of the device on Earth, as well as the accuracy with which these properties are calculated. */
// interface Coordinates {
//   readonly accuracy: number;
//   readonly altitude: number | null;
//   readonly altitudeAccuracy: number | null;
//   readonly heading: number | null;
//   readonly latitude: number;
//   readonly longitude: number;
//   readonly speed: number | null;
// }

// interface Position {
//   readonly coords: Coordinates;
//   readonly timestamp: number;
// }

// /** The reason of an error occurring when using the geolocating device. */
// interface PositionError {
//   readonly code: number;
//   readonly message: string;
//   readonly PERMISSION_DENIED: number;
//   readonly POSITION_UNAVAILABLE: number;
//   readonly TIMEOUT: number;
// }

// interface PositionErrorCallback {
//   (positionError: PositionError): void;
// }

// interface PositionOptions {
//   enableHighAccuracy?: boolean;
//   maximumAge?: number;
//   timeout?: number;
// }