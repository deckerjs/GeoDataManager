import { Component, OnInit } from '@angular/core';
import {
  CoordinateDataMessageBusService,
  MessageType
} from '../../services/coordinate-data-message-bus.service';
import { AuthService } from 'src/app/auth/auth.service';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faSync, faChevronDown, faChevronRight } from '@fortawesome/free-solid-svg-icons';
import { AppUser } from 'src/app/models/app-user';
import { UserSettingsAPIService } from 'src/app/services/user-settings-api.service';
import { stringify } from 'querystring';
import { Observable, pipe } from 'rxjs';
import { tap, take } from 'rxjs/operators';
import { CoordinateDataAPIService } from 'src/app/services/coordinate-data-api.service';
import { CoordinateData, CoordinateDataSummary } from 'src/app/models/coordinate-data';

@Component({
  selector: 'app-geo-data-selector',
  templateUrl: './geo-data-selector.component.html',
  styleUrls: ['./geo-data-selector.component.scss']
})
export class GeoDataSelectorComponent implements OnInit {
  public datasetCollection: Array<CoordinateDataSummary>;
  public selectedDataset: CoordinateDataSummary;
  public userList: AppUser[] = [];
  public showFilter: boolean = false;
  public filterShowShared: boolean = false;
  public filterForSelectedUser: AppUser;
  public currentUserId: string;

  constructor(
    private apiDataService: CoordinateDataAPIService,
    private msgService: CoordinateDataMessageBusService,
    private authService: AuthService,
    private userDataService: UserSettingsAPIService,
    private falibrary: FaIconLibrary
  ) {
    this.datasetCollection = [];
    falibrary.addIcons(faSync, faChevronDown, faChevronRight);
  }

  ngOnInit() {
    this.loadDatasetsFromRepo();

    this.msgService
      .subscribeGeneral(MessageType.NewGeoDataAvailable)
      .subscribe(x => {
        this.loadDatasetsFromRepo();
      });

    this.authService.userIsAuthenticated
      .subscribe({
        next: x => {
          if (x) {
            this.loadDatasetsFromRepo();
          } else {
            this.clearDataset();
          }
        }
      });

    this.authService.userId
      .subscribe({
        next: x => {
          this.currentUserId = x;
        }
      })

  }

  private clearDataset() {
    this.datasetCollection = [];
  }

  private loadDatasetsFromRepo() {
    this.datasetCollection = [];

    this.refreshUserData().subscribe({
      next: u => {
        this.apiDataService.GetAllOwnedSummary().subscribe({
          next: x => {
            this.appendViewData(x, false);            
            const sorted = x.sort((val1:any, val2:any)=> {return val1.DateCreated - val2.DateCreated});
            this.datasetCollection.push(...sorted);
          }
        });
      }
    });

    if (this.filterShowShared) {
      const apiFilter: Map<string, string> = new Map<string, string>();
      if (this.filterForSelectedUser != null) {
        apiFilter.set('UserID', this.filterForSelectedUser.ID);
      }
      this.refreshUserData().subscribe({
        next: u => {
          this.apiDataService.GetAllSharedSummary(apiFilter).subscribe({
            next: x => {
              this.appendViewData(x, true);
              const sorted = x.sort((val1:any, val2:any)=> {return val1.DateCreated - val2.DateCreated});
              this.datasetCollection.push(...sorted);
            }
          });
        }
      });
    }
  }

  public refreshUserData(): Observable<AppUser[]> {
    return this.userDataService.getAllUsers().pipe(tap(
      x => {
        this.userList = x;
      }
    ));
  }

  private appendViewData(x: CoordinateDataSummary[], isShared:boolean) {
    x.forEach(ds => {
      ds['isShared'] = isShared;
      ds['userName'] = this.getUserNameFromId(ds.UserID);
    });
  }

  private getUserNameFromId(id: string): string {
    return this.userDataService.getUserNameFromId(id, this.userList);
  }

  public selectDataset(item: CoordinateDataSummary) {        
    this.selectedDataset = item;
    this.apiDataService.Get(item.ID).pipe(take(1)).subscribe({next: dataset=>{
      this.msgService.publishCoordinateDatasetSelected(dataset);
    }})
  }

  public reload(): void {
    this.loadDatasetsFromRepo();
  }

  public filterToggle(): void {
    this.showFilter = !this.showFilter;
  }

}
