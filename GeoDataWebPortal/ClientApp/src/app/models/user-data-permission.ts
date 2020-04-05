export interface UserDataPermission {
    ID:string;
    OwnerUserID:string;
    AllowedUserID:string;
    ResourceName:string;
    Read:boolean;
    Write:boolean;
}