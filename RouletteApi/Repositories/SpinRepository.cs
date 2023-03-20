using Dapper;
using Microsoft.Data.Sqlite;
using RouletteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Repositories
{
    public class SpinRepository : ISpinRepository
    {
        private readonly string connectionString;

        public SpinRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> Add(Spin spin)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var sql = @"INSERT INTO Spins (WinningNumber, SpunAt)
                VALUES (@WinningNumber, @SpunAt);
                SELECT last_insert_rowid();";

            return await connection.QuerySingleAsync<int>(sql, spin);
        }

        public async Task<Spin> Get(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var sql = "SELECT * FROM Spins WHERE Id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Spin>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Spin>> GetAll()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var sql = "SELECT * FROM Spins";

            return await connection.QueryAsync<Spin>(sql);
        }

    }
}
