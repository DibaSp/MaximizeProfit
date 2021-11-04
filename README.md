# Simple solution for getting best BID/ASK orders
## Solution description
This solution consist of four projects:
- ConsoleApp - for testing Lib
- Lib - logic for loading exchange books, best order selection, ...
- Tests - tests for testing main flow of best order selection
- WebApi - WebApi that exposes the functionality of Lib

## How to use
First (in solution root folder) run:
```
dotnet build
```
### ConsoleApp
To use console app, run:
```
dotnet run --project MaximizeProfitConsoleApp/
```
To test the console app, follow the instructions in console.
### Tests
To execute tests, run:
```
dotnet test
```
Expected output should be:
```
Passed!  - Failed:     0, Passed:    18, Skipped:     0, Total:    18, Duration: 25 ms - MaximizeProfit.Lib.Tests.dll (netcoreapp3.1)
```

### WebApi
To use web api, use docker compose file and run:
```
docker compose up
```
This should build a new container and run WebApi with exposed endpoint for creation of best order selection.

Example of calling the endpoint:
```
curl --location --request POST 'http://localhost:80/orders' \
--header 'Content-Type: application/json' \
--data-raw '{
    "type": "SELL",
    "amount": 10
}'
```
This should return random orders that user can execute for selling 10 BTC.