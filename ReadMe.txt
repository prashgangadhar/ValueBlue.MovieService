READ ME:

General:
a. The solution is built on .Net8 following clean architecture and solid principles
b. Open solution is VS2022, build solution and run app to launch swagger page. (Use http endpoint if https does not load)

1) Search:
	a. Does not store the requests in db if the movie is not found in omdb
	b. Returns movie back to the caller if the movie is found in omdb but failed to store in the database.
	b. Search endpoint is not secured. No need to supply a api key to perform search.
	
2) Admin:
	a. Admin endpoints are secured via a api key. The value is stored in AppSettings.json.
	"AdminApiKey": "d4f1a3b9e5c6d7a8e2b9c0f8d7e6f1a2b"
	b. If an api key is not provided, response = 401 Unauthorized
	c. If a wrong api key is provided, response = 403 Forbidden
	
3) Additional endpoints added to Admin:
	a. GetLatestNRequests - Gets top n requests
	b. DeleteRequestsBeforeDate - Deletes requests in bulk that are before the supplied date
	
4) Omdb service:
	"ApiKey": "7674a524",
	"BaseUrl": "https://www.omdbapi.com/"
	
5) MongoDb: 
	a. A local mongodb was used for development. Please make sure to replace config values accordingly before running the app.
	b. Configuration is stored in AppSettings.json as
		"MovieSearchHistoryDatabase": {
			"ConnectionString": "mongodb://localhost:27017",
			"DatabaseName": "ValueBlueMovies",
			"CollectionName": "SearchRequests"
		}
	
6) Logging:
	a. Logging is setup using Serilog and configured to log to console and file (rolling file per day)
	b. Log files are stored in Logs folder under ValueBlue.Movies.WebApi folder
	
7) Tests:
	a. Unit tests are in ValueBlue.Movies.Tests project
	b. Tests cover SearchController & OmdbServiceAgent
	c. Packages used: AutoFixture, XUnit, FakeItEasy
	
8) Swagger:
	a. Swagger is configured to facilitate testing & OpenApi documentation
	b. There is an option to Authorize with the api key in order to use Admin endpoints