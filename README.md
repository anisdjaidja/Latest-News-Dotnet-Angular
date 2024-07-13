# Latest News Test DotNet + Angular DOCS 📄

<div align="center">
	<code><img width="100" src="https://user-images.githubusercontent.com/25181517/183890595-779a7e64-3f43-4634-bad2-eceef4e80268.png" alt="Angular" title="Angular"/></code>
	<code><img width="100" src="https://user-images.githubusercontent.com/25181517/121405754-b4f48f80-c95d-11eb-8893-fc325bde617f.png" alt=".NET Core" title=".NET Core"/></code>
</div>


The Latest News Platform provides an .Net Server that serves latest news RESTfully, and an AngularJS Client to display and manipulate Latest News Articles with sources and images.
# Setup and Build
## Server
- CD to LatestNewsTestBackend
- To build the server on Dotnet 8 core runtime you should make sure these required dependencies are listed in the *.cproj* file inside :
``` xml
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.7" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.7" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.7">
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="CacheCow.Server.Core.Mvc" Version="2.13.1" />
```
If you are using VS Code, install these one by one in the terminal using this command ```nuget install <packageID | configFilePath> [options]```, consult officail docs : https://learn.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-install

If you are Using VS Studio, make sure you have the latest .net8 release and usually your IDE would auto install the listed packages on first build.

### Database
- Make sure you have MS SQLserver installed on your server machine or an SQLlocalDB on your Visual Studio for quick development setup.
- Make sure to add an *appsettings.json* file into the server root with the following sections:
``` json
    {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultSQLServer": "<YOUR CONNECTION STRING HERE>"
  },
  "ApiKey": "<YOUR NEWS API KEY HERE>",
  "PlaceholderImage": "https://images.unsplash.com/photo-1586339949916-3e9457bef6d3?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
}

```
Replace the values between tags '<>' with your own, you can set a custom Placeholder Image for the server to put into articles that come without image url.

### Database partitioning
Our choice of EF Core leaves us without any practical ways to partition the DB (this was realized later into the project), so this feature was dropped until a solution is found.

### Background worker - news fetcher
Upon starting the server, a background worker thread will start on it own, calling the News API in a loop and updating the server database with the latest found news articles with their sources.

This worker may query the API with the arguments : Date (if there are records on the DB to rely on), apikey (required).
and has a fixed delay before it invokes a new query.

To modify the delay interval this worker invokes in. go to *NewsFetchService.cs* and alter the number shown bellow :
``` c#
        _logger.LogInformation($"News Fetcher Service Updated {newsresponse.totalResults} new articles");
    }
    /// This is a delay to not overload the NEWS API Servers, 
    /// this background worker will request new records every X hours, observe "FromHours(1)"
    await Task.Delay(((int)TimeSpan.FromHours(1).TotalMilliseconds), cancellationToken);
}

```
![Screenshot 2024-07-12 192300](https://github.com/user-attachments/assets/848e8c0d-c6dc-4da9-9e2b-38b9fb796ccb)
### Authentication 🔐
This is an internal server, there is no public access authentication.

### Endpoints ❇️
  ![image](https://github.com/user-attachments/assets/7cde9132-6a75-4a8c-b42c-28e0f340db5b)

- An error HTTP 415 Unsupported media response means that the resquest body is not in json format or doesnt comply with data schema.
  
- An error HTTP 404 Not found response means that the specified route is not valid.

- An error HTTP 401 Unauthorized response means that you are not authorised to make such calls and need CORS allowance.
  
- An HTTP 5xx response will almost never occure due to the internal database being auto-reconnected automatically.

#### Get All News ```/api/News/getall```
Response
``` json
{
	"lastID": 872,
	[
	  
	  {
	    "id": 827,
	    "title": "The Boys Season 4 Introduces A New Villain With A Familiar Power",
	    "author": "staff@slashfilm.com (Devin Meenan)",
	    "description": "Let's talk about the latest villain to pop up on The Boys season 4. Beware of spoilers.",
	    "url": "https://www.slashfilm.com/1617656/the-boys-season-4-introduces-new-villain/",
	    "urlToImage": "https://www.slashfilm.com/img/gallery/the-boys-season-4-introduces-a-new-villain-with-a-familiar-power/l-intro-1720453574.jpg",
	    "publishedAt": "2024-07-11T17:00:00",
	    "content": "The villains of manga/anime \"Fullmetal Alchemist\" are homunculi named after the seven deadly sins. Envy is a shapeshifter, naturally. Since their sin is all about desiring what others have, they can … [+1634 chars]",
	    "source": {
	      "id": "/film",
	      "name": "/FILM",
	      "description": null,
	      "url": null,
	      "category": null,
	      "language": null,
	      "country": null
	    },
	    "sourceId": "/film"
	  },
	  {
	    "id": 872,
	    "title": "The Galaxy Ring won’t require a monthly subscription",
	    "author": "Andrew Romero",
	    "description": "One of the main concerns that cropped up prior to the Galaxy Ring launch was that Samsung may require users to subscribe in order to use the ring’s features. As it stands, Samsung does not plan on offering a subscription-locked set of features for the Galaxy …",
	    "url": "http://9to5google.com/2024/07/11/galaxy-ring-wont-require-subscription/",
	    "urlToImage": "https://i0.wp.com/9to5google.com/wp-content/uploads/sites/4/2024/07/Samsung-Galaxy-Ring-v1.jpg?resize=1200%2C628&quality=82&strip=all&ssl=1",
	    "publishedAt": "2024-07-11T17:11:19",
	    "content": "One of the main concerns that cropped up prior to the Galaxy Ring launch was that Samsung may require users to subscribe in order to use the ring’s features. As it stands, Samsung does not plan on of… [+2351 chars]",
	    "source": {
	      "id": "9to5google.com",
	      "name": "9to5google.com",
	      "description": null,
	      "url": null,
	      "category": null,
	      "language": null,
	      "country": null
	    },
	    "sourceId": "9to5google.com"
	  }
	]
}
```
#### Get All News Starting from provided ID ```/api/News/getall/{LastID}```
Response
``` json
{
	"lastID": 872,
	[
	  
	  {
	    "id": 827,
	    "title": "The Boys Season 4 Introduces A New Villain With A Familiar Power",
	    "author": "staff@slashfilm.com (Devin Meenan)",
	    "description": "Let's talk about the latest villain to pop up on The Boys season 4. Beware of spoilers.",
	    "url": "https://www.slashfilm.com/1617656/the-boys-season-4-introduces-new-villain/",
	    "urlToImage": "https://www.slashfilm.com/img/gallery/the-boys-season-4-introduces-a-new-villain-with-a-familiar-power/l-intro-1720453574.jpg",
	    "publishedAt": "2024-07-11T17:00:00",
	    "content": "The villains of manga/anime \"Fullmetal Alchemist\" are homunculi named after the seven deadly sins. Envy is a shapeshifter, naturally. Since their sin is all about desiring what others have, they can … [+1634 chars]",
	    "source": {
	      "id": "/film",
	      "name": "/FILM",
	      "description": null,
	      "url": null,
	      "category": null,
	      "language": null,
	      "country": null
	    },
	    "sourceId": "/film"
	  }
	]
}
```
#### Get All sources ```/api/News/getall/sources```
Response
``` json
[
  {
    "id": "abc-news",
    "name": "ABC News",
    "description": null,
    "url": null,
    "category": null,
    "language": null,
    "country": null
  },
  {
    "id": "abc-news-au",
    "name": "ABC News (AU)",
    "description": null,
    "url": null,
    "category": null,
    "language": null,
    "country": null
  },
  {
    "id": "adafruit.com",
    "name": "Adafruit.com",
    "description": null,
    "url": null,
    "category": null,
    "language": null,
    "country": null
  },
  {
    "id": "adexchanger",
    "name": "AdExchanger",
    "description": null,
    "url": null,
    "category": null,
    "language": null,
    "country": null
  }
]
```
### Server-side caching
This server implements HTTP response caching over 5 minutes, meaning it saves endpoint responses into memory and re-uses them when appropriate, achieved with the open source CachCow library. 

## Client
- To build the server on AngularJS 17 you should first install NodeJS https://nodejs.org/en/download/prebuilt-installer
- Next Install Angualr by running this command ```npm install -g @angular/cli@17``` on your terminal.
- Once Angular is installed, CD to LatestNewsTestFrontend and install Bootstrap and Material Libraries by excuting these commands on the Angular CLI, ```ng add @ng-bootstrap/ng-bootstrap``` and ```ng add @angular/material```  (recommended to use VScode terminal)
- On the same terminal. Build and Run the client with ```ng serve``` or ```ng serve -o``` to open in the browser.
### Access to server
In the *app.component.ts* file, update the port number with the server port.
``` js
  ngOnInit(): void {
    this.httpclient.get<Article[]>('https://localhost:<YOUR PORT NUMBER HERE>/News/getall')
      .subscribe(result => {
        this.articleService.allArticles = result.sort((a, b) => new Date(b.publishedAt).getTime() - new Date(a.publishedAt).getTime());;
        console.log(this.articleService.allArticles);
      })
  }
```
On the server side, update the CORS policy in *program.cs* to include the client adress.
``` c#
  builder.Services.AddCors(options =>
  {
      options.AddPolicy("CORSPolicy",
          builder =>
          {
              builder
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithOrigins("http://localhost:<YOUR CLIENT PORT NUMBER HERE>");
          });
  });
```
### Client-side caching
This client implements session storage caching, this is a combined server/client mechanisme, meaning the client first makes a new request for all articles, the server respondes as expected and attach a proprety saying what is the last Article ID in the response array (article IDs are created and incremented in the server so they are pre-sorted by ID), this allows the client to store the response object in Session-Storage and make future requests using that stored LastID to only request the newest articles, reducing load on DB and improving Client responsiveness.

### Run everything
Make sure to run both the server and client with - ```ng serve -o``` and ```DotNet Run``` respectively.
![Screenshot 2024-07-12 192007](https://github.com/user-attachments/assets/9102a68e-020e-4b2e-bc2a-ce636d31d1f1)
![Screenshot 2024-07-12 192115](https://github.com/user-attachments/assets/1fe10105-9627-4e28-b0af-00aad33d8a80)
![Screenshot 2024-07-12 192152](https://github.com/user-attachments/assets/e7a5039c-b3c2-4a9c-a11c-964eebc8f55e)

### Disclaimer: this is a test project and not developed with production in mind. Functionalities (even thought very simple) has not been tested by TDD, For any further questions regarding under the hood and api architechture please contact the author.

## Author
Anis Djaidja

Software engineer

anisdjaidja1@gmail.com
