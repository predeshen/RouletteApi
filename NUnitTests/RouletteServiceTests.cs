using Dapper;
using Microsoft.Data.Sqlite;
using Moq;
using NUnit.Framework;
using RouletteApi.Models;
using RouletteApi.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTests
{
    [TestFixture]
    public class RouletteServiceTests
    {
        private IDbConnection dbConnection;
        private RouletteService rouletteService;

        [SetUp]
        public void SetUp()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            dbConnection = new SqliteConnection("Data Source=:memory:");
            dbConnection.Open();

            // create tables
            dbConnection.Execute(@"
            CREATE TABLE Bet (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BetType TEXT NOT NULL,
                BetValue TEXT NOT NULL,
                Amount INTEGER NOT NULL,
                PlacedAt TEXT NOT NULL
            );

            CREATE TABLE Spin (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                WinningNumber TEXT NOT NULL,
                SpunAt TEXT NOT NULL
            );
        ");

            rouletteService = new RouletteService(dbConnection);
        }

        [TearDown]
        public void TearDown()
        {
           dbConnection.Close();
        }

        [Test]
        public async Task TestPlaceBet()
        {
            Bet bet = new Bet { BetType = "Number", BetValue = 17, Amount = 500 ,PlacedAt=DateTime.Now};

            int betId = await rouletteService.PlaceBet(bet);

            Assert.That(betId, Is.GreaterThan(0));
        }

        [Test]
        public async Task TestSpin()
        {
            Spin spin = await rouletteService.Spin();

            Assert.That(spin.WinningNumber, Is.Not.Empty);
        }

        [Test]
        public async Task TestPayout()
        {
            Spin spin = await rouletteService.Spin();

            Bet bet1 = new Bet {BetType = "Number", BetValue = Convert.ToInt32(spin.WinningNumber), Amount = 500 ,PlacedAt = DateTime.Now};
            Bet bet2 = new Bet {BetType = "Red", BetValue = Convert.ToInt32(spin.WinningNumber), Amount = 1000 ,PlacedAt=DateTime.Now};
            await rouletteService.PlaceBet(bet1);
            await rouletteService.PlaceBet(bet2);

            int rowsAffected = await rouletteService.Payout(spin.WinningNumber);

            Assert.That(rowsAffected, Is.EqualTo(2));
        }

        [Test]
        public async Task TestShowPreviousSpins()
        {
            Spin spin1 = new Spin { WinningNumber = "17", SpunAt = DateTime.Now };
            Spin spin2 = new Spin { WinningNumber = "Red", SpunAt = DateTime.Now };
            await dbConnection.ExecuteAsync("INSERT INTO Spin (WinningNumber, SpunAt) VALUES (@WinningNumber, @SpunAt)", new { WinningNumber = spin1.WinningNumber, SpunAt = spin1.SpunAt });
            await dbConnection.ExecuteAsync("INSERT INTO Spin (WinningNumber, SpunAt) VALUES (@WinningNumber, @SpunAt)", new { WinningNumber = spin2.WinningNumber, SpunAt = spin2.SpunAt });

            IEnumerable<Spin> spins = await rouletteService.ShowPreviousSpins(2);

            Assert.That(spins.Count(), Is.EqualTo(2));
        }
    }


}