using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DiffingAPI.DiffLogic {
    public static class DiffManager {

        private static List<DiffEntry> diffs = new List<DiffEntry>();

        /// <summary>
        /// This function will either assign the left value of the diff entry if it
        /// exists or create a new one with the left value and id.
        /// </summary>
        /// <param name="id">ID of entry</param>
        /// <param name="value">New left value</param>
        public static void AddLeftValue(int id, string value) {

            DiffEntry diff = diffs.Find(d => d.ID == id);

            if (diff != null) {

                int diffIndex = diffs.FindIndex(d => d == diff);

                diffs[diffIndex].Left = value;

                return;
            }

            diffs.Add(new DiffEntry(id, left: value));
        }

        /// <summary>
        /// This function will either assign the right value of the diff entry if it
        /// exists or create a new one with the right value and id.
        /// </summary>
        /// <param name="id">ID of entry</param>
        /// <param name="value">New right value</param>
        public static void AddRightValue(int id, string value) {

            DiffEntry diff = diffs.Find(d => d.ID == id);

            if (diff != null) {

                int diffIndex = diffs.FindIndex(d => d == diff);

                diffs[diffIndex].Right = value;

                return;
            }

            diffs.Add(new DiffEntry(id, right: value));
        }

        /// <summary>
        /// Tries to find and compare the values of a desired DiffEntry
        /// </summary>
        /// <param name="id">ID of DiffEntry to compare</param>
        public static DiffResult Compare(int id) {

            // Try to find the desired DiffEntry
            DiffEntry entry = diffs.Find(d => d.ID == id);

            if (entry == null || entry.Left == string.Empty || entry.Right == string.Empty) {

                // Invalid ID or incomplete entry so return appropriate diff result
                return new DiffResult { Valid = false };
            }

            if (entry.Left.Length != entry.Right.Length) {

                // Lenghts don't match so return that
                return new DiffResult {
                    Valid = true,
                    ResultType = DiffResult.DiffResultType.SizeDoNotMatch
                };
            }

            if (entry.Left != entry.Right) {

                // The data is of the same length so let's compare and
                // return the results.
                return compareByteArrays(entry.Left, entry.Right);
            }

            // If all the checks pass, the data is the same.
            // Return appropriate result.
            return new DiffResult {
                Valid = true,
                ResultType = DiffResult.DiffResultType.Equals
            };
        }

        private static DiffResult compareByteArrays(string left, string right) {

            byte[] leftVal = Convert.FromBase64String(left);
            byte[] rightVal = Convert.FromBase64String(right);

            List<DiffResult.Diff> diffs = new List<DiffResult.Diff>();

            DiffResult.Diff currentDiff = null;

            // Loop through the array and compare each index
            for (int i = 0; i < leftVal.Length; i++) {

                if (leftVal[i] == rightVal[i]) {

                    // Values are the same, check if we have a diff record.
                    // If so, add it to list and get rid of it.
                    if (currentDiff != null) {

                        diffs.Add(currentDiff);

                        currentDiff = null;
                    }

                    continue;
                }

                if (leftVal[i] != rightVal[i]) {

                    // Values are different, first check if we already have a diff record.
                    // If so, increase lenght of diff, if not create a new one.
                    if (currentDiff == null) {

                        currentDiff = new DiffResult.Diff { Offset = i, Lenght = 1 };
                    }
                    else {

                        currentDiff.Lenght++;
                    }
                }
            }

            // After comparing values, check if there is a diff remaining.
            // If so, add it to the list and get rid of it.
            if (currentDiff != null) {

                diffs.Add(currentDiff);

                currentDiff = null;
            }

            // Return result type and diffs.
            return new DiffResult {
                Valid = true,
                ResultType = DiffResult.DiffResultType.ContentDoNotMatch,
                Diffs = diffs
            };
        }
    }

    public class DiffEntry {

        public int ID;

        public string Left;
        public string Right;

        public DiffEntry(int id, string left = "", string right = "") {

            ID = id;

            Left = left;
            Right = right;
        }
    }

    [JsonObject]
    public class DiffResult {

        public enum DiffResultType {
            Equals, ContentDoNotMatch, SizeDoNotMatch
        }

        [JsonIgnore]
        public bool Valid;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "diffResultType")]
        public DiffResultType ResultType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "diffs")]
        public List<Diff> Diffs;

        public class Diff {

            public int Offset;
            public int Lenght;
        }
    }
}