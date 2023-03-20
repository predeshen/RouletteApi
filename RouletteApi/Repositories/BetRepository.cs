using Microsoft.Data.Sqlite;
using RouletteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
namespace RouletteApi.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly string connectionString;

        public BetRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> Add(Bet bet)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var sql = @"INSERT INTO Bets (PlayerName, BetAmount, BetType, BetValue, PlacedAt)
                    VALUES (@PlayerName, @BetAmount, @BetType, @BetValue, @PlacedAt);
                    SELECT last_insert_rowid();";

            return await connection.QuerySingleAsync<int>(sql, bet);
        }

        public async Task<Bet> Get(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var sql = "SELECT * FROM Bets WHERE Id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Bet>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Bet>> GetAll()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var sql = "SELECT * FROM Bets";

            return await connection.QueryAsync<Bet>(sql);
        }
    }
}