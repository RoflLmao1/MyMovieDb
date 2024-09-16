# MyMovieDb (Demo)

This is a simple demo API application for retrieving movies and movie details from The Movie Database (TMDB).
It supports authentication to retrieve an authorization token for accessing the other endpoints and allows retrieving the authenticated user's favourite movies as well as adding/removing a movie as their favourite movie.

## Setup

### Pre-requisites

- .NET 8 SDK or .NET 8 Runtime is required in order to run the application. See [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- MySQL database is required for data storage, and the tables required need to be created.
  - SQL scripts for the tables can be found in the sub-directory <code>Sql\Tables</code>.
  - All the SQL scripts will need to be executed in the MySQL database being hosted for the application.
- TMDB access token. Provided via email when requested (not for public use)
  - You can also create your own account at [The Movie Database (TMDB) (themoviedb.org)](https://www.themoviedb.org/) to generate an access token for use.
- Client access credentials. Provided via email when requested (not for public use).
- (Optional) Redis database. If not provided, will use in-memory cache instead.

### Running the Application

<ol>
	<li>Open the Commandline (For Windows, press <code>Windows Key + R</code>, input <strong>cmd</strong> and press <code>Enter</code>.</li>
	<li>Navigate to the sub-directory <code>src\MyMovieDb.HttpApi</code>.</li>
	<li>Enter the command <code>dotnet run MyMovieDb.HttpApi --environment "Development" ConnectionStrings:MyMovieDb={ConnectionString} Adapter:Tmdb:AccessToken={TmdbAccessToken}</code>.
		<ul>
			<li>Replace <strong>{ConnectionString}</strong> with the actual connection string to your MySQL database.</li>
			<li>Replace <strong>{TmdbAccessToken}</strong> with the actual access token for accessing the TMDB API.</li>
		</ul>
	</li>
	<li>(Optional) To use Redis database, add the following command <code>Redis:IsEnabled=true Redis:Configuration={RedisConfiguration}</code> to the end of the command specified above.</li>
		<ul>
			<li>Replace <strong>{RedisConfiguration}</strong> with the actual connection string to your Redis database.</li>
		</ul>
	<li>The application should be up and running with the Swagger documentation accessible via <code>https://localhost:7264/swagger/index.html</code>.</li>
</ol>