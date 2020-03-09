# GeoDataManager

**GeoStoreAPI Environment settings:**

- AuthURL - use main host url (ex http://localhost:5000)
- GeoMgrClientSecret - secret to use for the ui client

- AppOptions__GenerateDefaultUsers - if true will create 3 users and default roles if they dont already exist
   - user1:password1
   - user2:password2
   - admin1:password1

- IDSOptions__IssuerUri - useful if running in a docker container, set it to the url it would connect to inside the container (ex http://geodataapicontainer)

**GeoDataWebPortal  Environment settings:**
  - ClientConfigurationSettings__AuthUrl - use api host url
  - ClientConfigurationSettings__AuthClientSecret - use same ui client secret specified for api
  - ClientConfigurationSettings__GeoDataApiUrl - use api host url
  - ClientConfigurationSettings__MapboxToken - create an account and get an api key from https://www.mapbox.com/
  }

**Basic Docker Examples**
(After installing docker support)

From within the project root build the images for the api and portal services using docker-compose:
```
docker-compose build
```

Verify the images exist (optional)
```
docker image ls
```

Adjust the environment variables as needed in the .env file
```
API_IMAGE_VERSION=latest

GENERATE_DEFAULT_USERS=True
API_AUTH_URL=http://geodataapi
ISSUER_URI=http://geodataapi
API_PORT=8080

PORTAL_AUTH_URL=http://localhost:8080
PORTAL_AUTH_CLIENT_SECRET=notreallyasecret
PORTAL_PORT=8081
API_URL=http://localhost:8080
MAPBOX_TOKEN=getamapboxtokenandputithere
```

Start up the containers from any directory that has both the docker-compose.yml and .env file
```
docker-compose up
```

Using the above environment settings you should be able to point your browser to http://localhost:8081 and not see an error.  You should also be able to post data to the api at http://localhost:8080

Stop the containers
```
docker-compose down
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



