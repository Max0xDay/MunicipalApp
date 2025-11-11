/*
========  IssuePredictionTrie.cs  ========
Purpose: IssuePredictionTrie provides prefix-based autocomplete suggestions for municipal issue descriptions.
Why its used: In the Sidequest municipal app, tries accelerate user input with context-aware predictions.

In detail:
The IssuePredictionTrie class is our smart suggestion engine for the ReportIssues form. It builds
a prefix tree seeded with common municipal complaints like "Burst Pipe" and "Broken Street Light,"
then learns dynamically from resident submissions to bubble popular phrases to the top. Each
insertion walks the trie letter by letter, incrementing frequency counters so the ranking logic
can surface the most relevant completions first.

When a user types even two letters, the system traverses down the matching branch, gathers all
terminal suggestions from that subtree, and sorts them by frequency before limiting the results
to the top five. This keeps the UI responsive while guiding reporters toward standardized phrasing
that streamlines ticket categorization and dispatch routing for municipal staff.
=============================
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidequest_municiple_app {
    public class IssuePredictionTrie {
        private TrieNode root;
        private Dictionary<string, int> suggestionFrequency;

        public IssuePredictionTrie() {
            root = new TrieNode();
            suggestionFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            InitializeCommonIssues();
        }

        private void InitializeCommonIssues() {
            string[] commonIssues = new string[] {
                "Burst Pipe",
                "Burst Water Main",
                "Broken Street Light",
                "Broken Traffic Light",
                "Pothole Damage",
                "Multiple Potholes",
                "Damaged Road Surface",
                "Cracked Pavement",
                "Blocked Drain",
                "Blocked Sewer",
                "Leaking Water Main",
                "Leaking Pipe",
                "No Water Supply",
                "No Electricity",
                "Power Outage",
                "Power Failure",
                "Overflowing Bin",
                "Overflowing Rubbish",
                "Illegal Dumping",
                "Uncollected Waste",
                "Uncollected Rubbish",
                "Missing Manhole Cover",
                "Damaged Manhole",
                "Street Flooding",
                "Water Leak",
                "Sewage Leak",
                "Sewage Spill",
                "Fallen Tree",
                "Overgrown Vegetation",
                "Overgrown Grass",
                "Damaged Sidewalk",
                "Broken Pavement",
                "Cracked Road",
                "Sinkhole",
                "Faulty Street Light",
                "Traffic Signal Malfunction",
                "Damaged Fence",
                "Graffiti Vandalism",
                "Property Vandalism",
                "Noise Complaint",
                "Stray Animals",
                "Dead Animal",
                "Broken Water Meter",
                "Faulty Electricity Meter",
                "Storm Damage",
                "Road Safety Issue"
            };

            foreach (string issue in commonIssues) {
                Insert(issue, 1);
            }
        }

        public void Insert(string suggestion, int frequency = 1) {
            if (string.IsNullOrWhiteSpace(suggestion)) {
                return;
            }

            suggestion = suggestion.Trim();
            TrieNode current = root;

            foreach (char c in suggestion.ToLower()) {
                if (!current.HasChild(c)) {
                    current.AddChild(c, new TrieNode());
                }
                current = current.GetChild(c);
            }

            current.IsEndOfWord = true;
            current.FullSuggestion = suggestion;
            current.Frequency += frequency;

            if (suggestionFrequency.ContainsKey(suggestion)) {
                suggestionFrequency[suggestion] += frequency;
            }
            else {
                suggestionFrequency[suggestion] = frequency;
            }
        }

        public List<string> GetSuggestions(string prefix, int maxResults = 5) {
            if (string.IsNullOrWhiteSpace(prefix)) {
                return new List<string>();
            }

            prefix = prefix.Trim();
            TrieNode current = root;

            foreach (char c in prefix.ToLower()) {
                if (!current.HasChild(c)) {
                    return new List<string>();
                }
                current = current.GetChild(c);
            }

            List<string> suggestions = new List<string>();
            CollectSuggestions(current, suggestions);

            return suggestions
                .OrderByDescending(s => {
                    if (suggestionFrequency.ContainsKey(s)) {
                        return suggestionFrequency[s];
                    }
                    return 0;
                })
                .ThenBy(s => s.Length)
                .Take(maxResults)
                .ToList();
        }

        private void CollectSuggestions(TrieNode node, List<string> suggestions) {
            if (node == null) {
                return;
            }

            if (node.IsEndOfWord && !string.IsNullOrEmpty(node.FullSuggestion)) {
                suggestions.Add(node.FullSuggestion);
            }

            foreach (TrieNode child in node.Children.Values) {
                CollectSuggestions(child, suggestions);
            }
        }

        public string GetTopSuggestion(string prefix) {
            List<string> suggestions = GetSuggestions(prefix, 1);
            return suggestions.Count > 0 ? suggestions[0] : string.Empty;
        }

        public void LearnFromInput(string input) {
            if (string.IsNullOrWhiteSpace(input) || input.Length < 5) {
                return;
            }

            input = input.Trim();
            Insert(input, 1);
        }

        public int GetSuggestionCount() {
            return suggestionFrequency.Count;
        }

        public bool HasSuggestions(string prefix) {
            if (string.IsNullOrWhiteSpace(prefix)) {
                return false;
            }

            TrieNode current = root;
            foreach (char c in prefix.ToLower()) {
                if (!current.HasChild(c)) {
                    return false;
                }
                current = current.GetChild(c);
            }
            return true;
        }
    }
}
