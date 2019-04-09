# Simple JWT for .NET Core 2.2

This is the simplest possible project that implemented JWT and a little else as possible.

## Test Drive

### Launch the project

Load up the project and press **F5** and Visual Studio will launch the site in a browser.

![Browser][Browser]

I get port #55477, so I will use that port in the URLs. 

### Get a JWT Token

In a real app, you would have to log in to get a JWT. Since I wanted to make things as simple as I can, I will just give you the JWT. 

To get a token:
1. Launch Postman
2. Choose "Post" from the method dropdown and "http://localhost:55477/api/jwt/maketoken" in the URL box
3. Select "Body", "raw" and "JSON (application/json"
4. Enter the following JSON in the textbox:
```JSON
{
    "UserName": "dvader",
    "Role": "admin",
    "Id": 42
}
```
You can enter any role you want, there are endpoints for the roles "admin" and "super", Name and Id are added as Claims and can be anything.
5. Click "Send"

![Make the Token with Postman][MakeToken]

The JWT will be sent in the body, you will need it later.

### Use the JWT make a REST request

1. Hi there

[Browser]: https://github.com/jrcs3/SimpleJwt4Core22/blob/master/pictures/Browser.PNG?raw=true "Browser opens after project is started"

[MakeToken]: https://github.com/jrcs3/SimpleJwt4Core22/blob/master/pictures/MakeToken.PNG?raw=true "Request a JWT"

[MakeRequest]: https://github.com/jrcs3/SimpleJwt4Core22/blob/master/pictures/MakeRequest.PNG?raw=true "Call the endpoint"

[BadRequest]: https://github.com/jrcs3/SimpleJwt4Core22/blob/master/pictures/BadRequest.PNG?raw=true "Call the endpoint, wrong role"