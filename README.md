# GeoDataManager

**Environment settings:**

- AuthURL - use main host url (ex http://localhost:5000)
- GeoMgrClientSecret - secret to use for the geomgrui client

- AppOptions__GenerateDefaultUsers - if true will create 3 users and default roles if they dont already exist
   - user1:password1
   - user2:password2
   - admin1:password1

- IDSOptions__IssuerUri - useful if running in a docker container, set it to the url it would connect to inside the container (ex http://geodataapicontainer)

Example docker compose file:
```
version: "3.4"

services:
  geodataapi:    
    image: hallertau/geomarklar:latest
    container_name: geodataapi
    environment:
    - AppOptions__GenerateDefaultUsers=true
    - GeoMgrClientSecret=1234
    - AuthURL=http://geodataapi
    - IDSOptions__IssuerUri=http://geodataapi
    ports:
    - 8080:80
    expose:
    - 80
    networks:
    - geodatanet
networks:
  geodatanet:
```

**Using postman authorization to get a token:**
1. In your request open the Authorization tab
2. select **Oauth 2.0** for the **Type**
3. click **get new access token** to open the token dialog
   - Grant Type: Password Credentials
   - Access Token Url: your main host address followed by connect/token (ex: http://localhost:5000/connect/token)
   - Username: whatever user you are using (ex user1)
   - Password: (ex password1 if using user1)
   - clientid: geomgrui
   - client secret: whatever you set for the client secret environment setting


