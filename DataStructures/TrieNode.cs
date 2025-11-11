/*
========  TrieNode.cs  ========
Purpose: TrieNode represents a single character position in a prefix tree for predictive text suggestions.
Why its used: In the Sidequest municipal app, trie nodes enable fast autocomplete for issue descriptions.

In detail:
The TrieNode class forms the skeleton of our predictive text engine. Each node holds a character
position, a dictionary of child branches for the next possible letters, and metadata to signal
whether it marks the end of a valid suggestion. By tracking frequency counts and full suggestion
strings at terminal nodes, we can rank completions by how often residents actually submit them.

When users type into the ReportIssues form, the system traverses this node chain letter by letter,
exploring child paths to surface the most relevant municipal phrases like "Burst Pipe" or "Broken
Street Light." This shallow structure keeps lookups instant even when the suggestion library
grows beyond hundreds of known issue patterns.
=============================
*/
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class TrieNode {
        public Dictionary<char, TrieNode> Children { get; private set; }
        public bool IsEndOfWord { get; set; }
        public string FullSuggestion { get; set; }
        public int Frequency { get; set; }

        public TrieNode() {
            Children = new Dictionary<char, TrieNode>();
            IsEndOfWord = false;
            FullSuggestion = string.Empty;
            Frequency = 0;
        }

        public bool HasChild(char c) {
            return Children.ContainsKey(char.ToLower(c));
        }

        public TrieNode GetChild(char c) {
            return Children.ContainsKey(char.ToLower(c)) ? Children[char.ToLower(c)] : null;
        }

        public void AddChild(char c, TrieNode node) {
            Children[char.ToLower(c)] = node;
        }
    }
}
