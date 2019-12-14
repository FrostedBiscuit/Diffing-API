using System;
using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;
using Dapper;

using System.Diagnostics;


namespace DiffingAPI.DiffLogic {

    public static class DiffDB {

        /// <summary>
        /// Loads a list of entries that are stored in the SQLite database
        /// </summary>
        public static List<DiffEntry> LoadDiffEntries() {

            using (SQLiteConnection cnn = new SQLiteConnection(getConnectionString())) {

                Debug.WriteLine("Connected to db");

                var diffs = cnn.Query<DiffEntry>("SELECT * FROM Comparison");

                return diffs.ToList();
            }
        }

        /// <summary>
        /// This saves a new entry to the database
        /// </summary>
        /// <param name="entry">Entry to save</param>
        public static void SaveEntry(DiffEntry entry) {

            using (SQLiteConnection cnn = new SQLiteConnection(getConnectionString())) {

                Debug.WriteLine($"New entry: ID: {entry.ID}, Left: {entry.Left}, Right: {entry.Right}");

                cnn.Execute("REPLACE INTO Comparison(ID, Left, Right) Values (@ID, @Left, @Right);", entry);// new { ID = entry.ID, Left = entry.Left, Right = entry.Right });
            }
        }

        /// <summary>
        /// Returns a connection string to the database with a relative path.
        /// </summary>
        private static string getConnectionString() {

            return $@"Data Source={AppDomain.CurrentDomain.BaseDirectory}App_Data\ComparisonDB.db;Version=3;";
        }
    }
}