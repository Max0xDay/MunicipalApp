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
