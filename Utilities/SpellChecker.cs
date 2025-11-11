using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sidequest_municiple_app {
    public class SpellChecker {
        private HashSet<string> dictionary;
        private Dictionary<string, int> wordFrequency;
        private const int MAX_EDIT_DISTANCE = 2;

        public SpellChecker() {
            dictionary = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            wordFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            LoadSouthAfricanDictionary();
        }

        private void LoadSouthAfricanDictionary() {
            string[] commonWords = new string[] {
                "pothole", "potholes", "road", "roads", "street", "streets", "pavement", "pavements",
                "water", "leak", "leaking", "burst", "pipe", "pipes", "main", "supply",
                "electricity", "power", "outage", "outages", "blackout", "blackouts", "load", "shedding",
                "sanitation", "sewage", "drain", "drains", "blocked", "overflow", "overflowing",
                "refuse", "rubbish", "garbage", "waste", "collection", "bins", "bin",
                "traffic", "light", "lights", "robot", "robots", "intersection", "intersections",
                "municipal", "municipality", "council", "ward", "service", "services",
                "maintenance", "repair", "repairs", "damaged", "damage", "broken", "fixed",
                "urgent", "emergency", "dangerous", "hazard", "hazardous", "unsafe",
                "suburb", "suburbs", "township", "location", "area", "region", "district",
                "resident", "residents", "community", "neighbourhood", "neighborhood",
                "report", "reporting", "reported", "issue", "issues", "problem", "problems",
                "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with",
                "is", "are", "was", "were", "be", "been", "being", "have", "has", "had",
                "do", "does", "did", "will", "would", "could", "should", "may", "might",
                "this", "that", "these", "those", "there", "here", "where", "when", "why", "how",
                "my", "your", "his", "her", "its", "our", "their", "me", "you", "him", "them",
                "very", "too", "also", "just", "now", "then", "than", "so", "as", "if",
                "not", "no", "yes", "can", "cannot", "cant", "dont", "doesnt", "wont", "wouldnt",
                "street", "road", "avenue", "drive", "lane", "way", "close", "crescent",
                "north", "south", "east", "west", "central", "upper", "lower",
                "johannesburg", "pretoria", "tshwane", "cape", "town", "durban", "ethekwini",
                "soweto", "sandton", "centurion", "midrand", "randburg", "roodepoort",
                "please", "help", "urgent", "asap", "immediately", "soon", "today", "yesterday",
                "week", "weeks", "month", "months", "year", "years", "day", "days",
                "morning", "afternoon", "evening", "night", "monday", "tuesday", "wednesday",
                "thursday", "friday", "saturday", "sunday", "january", "february", "march",
                "april", "may", "june", "july", "august", "september", "october", "november", "december",
                "large", "small", "big", "little", "huge", "tiny", "major", "minor",
                "new", "old", "good", "bad", "better", "worse", "best", "worst",
                "about", "above", "across", "after", "against", "along", "among", "around",
                "before", "behind", "below", "beneath", "beside", "between", "beyond",
                "during", "except", "from", "inside", "into", "like", "near", "off",
                "onto", "outside", "over", "past", "since", "through", "throughout",
                "till", "toward", "under", "underneath", "until", "up", "upon", "within", "without",
                "people", "person", "man", "woman", "child", "children", "family", "families",
                "business", "businesses", "school", "schools", "hospital", "hospitals",
                "park", "parks", "mall", "malls", "shop", "shops", "store", "stores",
                "house", "houses", "home", "homes", "building", "buildings", "flat", "flats",
                "number", "numbers", "code", "codes", "address", "addresses", "contact",
                "phone", "telephone", "cell", "mobile", "email", "fax", "website",
                "braai", "bakkie", "robot", "veld", "koppie", "spruit", "vlei",
                "taxi", "taxis", "rank", "ranks", "kombi", "bus", "buses", "train", "trains",
                "eskom", "telkom", "sabc", "sars", "saps", "metro", "jmpd",
                "cbd", "industrial", "residential", "commercial", "informal", "settlement",
                "shack", "shacks", "squatter", "camp", "informal", "housing", "rented",
                "section", "phase", "extension", "block", "unit", "complex", "estate",
                "gated", "security", "guard", "access", "controlled", "boom", "gate"
            };

            foreach (string word in commonWords) {
                dictionary.Add(word.ToLower());
                wordFrequency[word.ToLower()] = 100;
            }
        }

        public void LearnWord(string word) {
            if (string.IsNullOrWhiteSpace(word)) return;
            
            string cleanWord = word.ToLower().Trim();
            if (cleanWord.Length > 1) {
                dictionary.Add(cleanWord);
                if (wordFrequency.ContainsKey(cleanWord))
                    wordFrequency[cleanWord]++;
                else
                    wordFrequency[cleanWord] = 1;
            }
        }

        public bool IsWordCorrect(string word) {
            if (string.IsNullOrWhiteSpace(word)) return true;
            
            string cleanWord = word.ToLower().Trim();
            if (cleanWord.Length <= 1) return true;
            if (char.IsDigit(cleanWord[0])) return true;
            
            return dictionary.Contains(cleanWord);
        }

        public List<string> GetSuggestions(string word, int maxSuggestions = 5) {
            if (string.IsNullOrWhiteSpace(word)) return new List<string>();
            
            string cleanWord = word.ToLower().Trim();
            if (cleanWord.Length <= 1) return new List<string>();
            
            Dictionary<string, int> candidates = new Dictionary<string, int>();
            
            foreach (string dictWord in dictionary) {
                if (Math.Abs(dictWord.Length - cleanWord.Length) > MAX_EDIT_DISTANCE)
                    continue;
                
                int distance = CalculateLevenshteinDistance(cleanWord, dictWord);
                
                if (distance <= MAX_EDIT_DISTANCE) {
                    int score = (MAX_EDIT_DISTANCE - distance) * 1000;
                    
                    if (wordFrequency.ContainsKey(dictWord))
                        score += wordFrequency[dictWord];
                    
                    if (dictWord.StartsWith(cleanWord.Substring(0, Math.Min(2, cleanWord.Length))))
                        score += 500;
                    
                    candidates[dictWord] = score;
                }
            }
            
            return candidates
                .OrderByDescending(kvp => kvp.Value)
                .Take(maxSuggestions)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        private int CalculateLevenshteinDistance(string source, string target) {
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(target) ? 0 : target.Length;
            
            if (string.IsNullOrEmpty(target))
                return source.Length;
            
            int sourceLength = source.Length;
            int targetLength = target.Length;
            
            int[,] distance = new int[sourceLength + 1, targetLength + 1];
            
            for (int i = 0; i <= sourceLength; i++)
                distance[i, 0] = i;
            
            for (int j = 0; j <= targetLength; j++)
                distance[0, j] = j;
            
            for (int i = 1; i <= sourceLength; i++) {
                for (int j = 1; j <= targetLength; j++) {
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;
                    
                    distance[i, j] = Math.Min(
                        Math.Min(
                            distance[i - 1, j] + 1,
                            distance[i, j - 1] + 1
                        ),
                        distance[i - 1, j - 1] + cost
                    );
                }
            }
            
            return distance[sourceLength, targetLength];
        }

        public List<SpellingError> CheckText(string text) {
            List<SpellingError> errors = new List<SpellingError>();
            
            if (string.IsNullOrWhiteSpace(text)) return errors;
            
            string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '(', ')', '[', ']', '{', '}', '"', '\'', '-', '/' }, 
                StringSplitOptions.RemoveEmptyEntries);
            
            int position = 0;
            foreach (string word in words) {
                position = text.IndexOf(word, position, StringComparison.OrdinalIgnoreCase);
                
                if (!IsWordCorrect(word)) {
                    errors.Add(new SpellingError {
                        Word = word,
                        Position = position,
                        Length = word.Length,
                        Suggestions = GetSuggestions(word)
                    });
                }
                
                position += word.Length;
            }
            
            return errors;
        }

        public void AddToDictionary(string word) {
            if (string.IsNullOrWhiteSpace(word)) return;
            
            string cleanWord = word.ToLower().Trim();
            if (cleanWord.Length > 1) {
                dictionary.Add(cleanWord);
                wordFrequency[cleanWord] = 50;
            }
        }
    }

    public class SpellingError {
        public string Word { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public List<string> Suggestions { get; set; }
    }
}
