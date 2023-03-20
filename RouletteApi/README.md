# RouletteApi

This is a RESTful API based around the game of roulette, implemented using C#, .NET 5, 
the Model-View-Controller (MVC) design pattern, and the SOLID principles. 
The API uses a SQLite database via Dapper and includes NUnit tests.

Requirements
.NET 5
SQLitePCLRaw (installed via NuGet)
Dapper
Moq

Endpoints
POST /api/PlaceBet - Place a bet.

POST /api/Spin -Spin the roulette wheel.

POST /api/Payout - Calculate the payout for all bets on the last spin.

POST /api/ShowPreviousSpins - Get last X number of spins.

