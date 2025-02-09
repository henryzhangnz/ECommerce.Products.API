Steps to run the app:

1. Navigate to ./Ecommerce directory.
2. If you have docker installed, then run command `docker-compose up -d --build`
3. Wait for docker to pull down all the dependencies.
4. Ideally, you should then have all components talking to each other.
5. Open web browser, navigate to `localhost:88` to open the webapp.

If you don't have docker, then it could take some bit of time to setup the environment.
Spin up the server:

1. Navigate to Products.API folder, then open up Products.API solution.
2. Connect to a SQL Server by providing the connection string in `DefaultConnection` of `ConnectionStrings` in `appsettings.Development.json`.
3. If you use VS, then build and run the Products.API solution. If you are using vscode, then run `dotnet run`.

Spin up the angular webapp:

1. Navigate to webapp folder.
2. Run `npm install`.
3. Make sure the `./src/app/environments/environment.development.ts` has got config of `productApiBaseUrl: 'https://localhost:7110'`
4. Run command `ng serve`, then open browser with `localhost:4200`.

After webapp shows up, you should be able to add products / edit products / delete products / search products / sort and navigate through products via pagination.

Stuff to consider / things to improve / limitations:

1. Should add a separate AuthService to generate token, i.e JWT and / or EntityFrame Identity, and depending on whether the user is authenticated / authorized, they can then perform add / edit / delete operations.
2. Could use NgRx to handle states better for the angular webapp, but it's a bit overkill if the system is small.
3. Create an event bus or use gRPC or use CQRS to communicate with other microservices once the system is scaling up.
4. Use kubernetes orchestration to improve stability of the system.
5. Using cache, improve loggings to improve system performance and monitoring.
6. More solid unit tests / integration tests.
