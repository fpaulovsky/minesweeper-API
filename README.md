# minesweeper-API

This is a MineSweeper ASP.NET Core Web API project.

# Setup
It persists games data in an SQL Server database. You just need to change the connection string in the appsettings.json file. Open it, and replace &lt;here you put your user&gt; by your DB user ID and &lt;here you put your password&gt; by your password.
After setting the connection string up, please run the Update-Database command in the Package Manager Console to let EF Core Tools create the Minesweeper database.

# Running the API
Open the project with VS2019, build it and run it by clicking the play button. A browser will be open showing Swagger (Open API). There you'll be able to play with the API.

# API Endpoints
You'll have more information about JSON schemas and endpoints once you run the project and it opens Swagger, but just in case you want to read this now, here you go:

At the moment the API has 2 controllers: Players and Games.

* To manage players (route /Players):
  * POST to /Players to register a new player. The JSON body of this request must have userName and password properties.
  * POST to /Players/Login to login with a specific player: The JSON body of this request must have userName and password properties. A JWT will be returned in the body of the response when userName and password are valid and match with a valid user.

* To play the game (route /Games):
  * POST to /Games to start a new game. The JSON body of this request must have width, height and minesCount properties. A JSON containing the game information will be returned in the response if the settings are valid. Only logged-in users can create new games.
  * GET to /Games/{id} to retrieve game with ID {id}. Only logged-in users can retrieve their games. Only games belonging to the logged-in user will be returned.
  * PATCH to /Games/{id}/UncoverCell?x={x}&y={u} to uncover a cell with coordinates x and y on a particular game with ID {id}.
  * PATCH to /Games/{id}/FlagCell?x={x}&y={u} to flag a cell with coordinates x and y on a particular game with ID {id}.
  * PATCH to /Games/{id}/UnFlagCell?x={x}&y={u} to unflag a cell with coordinates x and y on a particular game with ID {id}.
  * GET to /Games?pageSize={s}&pageNumber={n} to retrieve a list of the logged-in user games. The list is paginated. Set "s" to specify page size and set "n" to specify page number. First page is 1.

All routes in the /Games endpoint require that the logged-in user sends a valid authentication token using the Bearer format in the "Authorization" header.
