using Dapper;
using Microsoft.Data.Sqlite;
using RouletteApi.Models;
using RouletteApi.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly IDbConnection dbConnection;

        public RouletteService(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<int> PlaceBet(Bet bet)
        {
            using var transaction = dbConnection.BeginTransaction();

            try
            {
                string query = @"INSERT INTO Bet (Amount, BetType, BetValue, PlacedAt) 
                             VALUES (@Amount, @BetType, @BetValue, @PlacedAt);
                             SELECT last_insert_rowid();";
                int betId = await dbConnection.QuerySingleOrDefaultAsync<int>(query, bet);
                transaction.Commit();
                return betId;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Spin> Spin()
        {
            using var transaction = dbConnection.BeginTransaction();

            try
            {
                int winningNumber = new Random().Next(0, 37);
                DateTime spunAt = DateTime.UtcNow;
                string query = @"INSERT INTO Spin (WinningNumber, SpunAt) 
                             VALUES (@WinningNumber, @SpunAt);
                             SELECT last_insert_rowid();";
                int spinId = await dbConnection.QuerySingleOrDefaultAsync<int>(query, new { WinningNumber = winningNumber, SpunAt = spunAt });

                string betsQuery = @"SELECT * FROM Bet WHERE BetType = 'Number' AND BetValue = @WinningNumber;";
                var winningBets = await dbConnection.QueryAsync<Bet>(betsQuery, new { WinningNumber = winningNumber });

                foreach (var bet in winningBets)
                {
                    string updateQuery = @"UPDATE Bet SET Amount = @Amount WHERE Id = @Id";
                    int payoutAmount = (int)(bet.Amount * 36);
                    await dbConnection.ExecuteAsync(updateQuery, new { PayoutAmount = payoutAmount, Id = bet.Id });
                }

                transaction.Commit();
                return new Spin { WinningNumber = winningNumber.ToString(), SpunAt = spunAt };
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> Payout(string winningNumber)
        {
            using var transaction = dbConnection.BeginTransaction();

            try
            {
                string query = @"UPDATE Bet SET Amount = CASE 
                                    WHEN BetType = 'Number' AND BetValue = @WinningNumber THEN Amount * 36 
                                    WHEN BetType = 'Even' AND @WinningNumber IN (2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36) THEN Amount * 2 
                                    WHEN BetType = 'Odd' AND @WinningNumber IN (1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35) THEN Amount * 2 
                                    WHEN BetType = 'Red' AND @WinningNumber IN (1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36) THEN Amount * 2 
                                    WHEN BetType = 'Black' AND @WinningNumber IN (2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35) THEN Amount * 2 
                                ELSE 0 
                            END
                     WHERE BetType != 'Number' OR BetValue = @WinningNumber;";
                int rowsAffected = await dbConnection.ExecuteAsync(query, new { WinningNumber = winningNumber });
                transaction.Commit();
                return rowsAffected;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Spin>> ShowPreviousSpins(int count)
        {
            string query = "SELECT * FROM Spin ORDER BY SpunAt DESC LIMIT @Count";
            return await dbConnection.QueryAsync<Spin>(query, new { Count = count });
        }
    }

}


