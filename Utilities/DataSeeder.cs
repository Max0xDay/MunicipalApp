using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class DataSeeder {
        private readonly DatabaseHelper dbHelper;
        private readonly Random random;
        private readonly List<string> locations;
        private readonly List<string> categories;
        private readonly List<string> waterIssues;
        private readonly List<string> electricityIssues;
        private readonly List<string> sanitationIssues;
        private readonly List<string> roadsIssues;
        private readonly List<string> utilitiesIssues;

        public DataSeeder() {
            dbHelper = new DatabaseHelper();
            random = new Random();
            
            locations = new List<string> {
                "Main Street", "Church Street", "Market Street", "Park Avenue", "Oak Street",
                "Elm Street", "Maple Avenue", "Pine Street", "Cedar Road", "Birch Lane",
                "Victoria Street", "Queen Street", "King Street", "Nelson Mandela Drive",
                "Kingsway Avenue", "Railway Road", "Station Street", "Hospital Road",
                "School Street", "Library Road", "Commercial Street", "Industrial Avenue",
                "Residential Drive", "Beach Road", "Mountain View", "Valley Road",
                "Hillside Avenue", "Riverside Drive", "Lakeview Street", "Forest Lane"
            };

            categories = new List<string> {
                "Sanitation", "Roads", "Utilities", "Water", "Electricity"
            };

            waterIssues = new List<string> {
                "Burst water pipe causing flooding on street",
                "No water supply for 3 days in area",
                "Low water pressure affecting multiple households",
                "Water main break causing road damage",
                "Contaminated water reports from residents",
                "Leaking water meter at property",
                "Blocked storm water drain causing flooding",
                "Water tank overflow damaging property",
                "Fire hydrant continuously leaking",
                "Broken water connection at street level"
            };

            electricityIssues = new List<string> {
                "Power outage affecting entire street",
                "Street light not working for 2 weeks",
                "Exposed electrical wiring dangerous to public",
                "Transformer making loud buzzing noise",
                "Frequent power surges damaging appliances",
                "Illegal electricity connection reported",
                "Overhead power lines hanging too low",
                "Electricity meter box damaged and open",
                "Traffic light not functioning at intersection",
                "Sparking electrical box on street pole"
            };

            sanitationIssues = new List<string> {
                "Overflowing sewage drain on street",
                "Blocked sewer causing backup in homes",
                "Illegal dumping of waste on vacant lot",
                "Missed refuse collection for 2 weeks",
                "Broken manhole cover hazardous to traffic",
                "Foul smell from sewer system",
                "Refuse bins not collected on schedule",
                "Sewage spill near residential area",
                "Public toilet facility not maintained",
                "Waste water pooling on street"
            };

            roadsIssues = new List<string> {
                "Large pothole damaging vehicles",
                "Road surface cracked and breaking up",
                "Missing road sign at intersection",
                "Faded road markings need repainting",
                "Damaged speed bump needs repair",
                "Collapsed stormwater drain on road",
                "Tree branches blocking road signs",
                "Road flooding during rain",
                "Broken kerb and sidewalk hazard",
                "Gravel road needs grading and maintenance"
            };

            utilitiesIssues = new List<string> {
                "Damaged telephone line hanging low",
                "Gas leak smell reported in area",
                "Broken fire hydrant needs replacement",
                "Damaged traffic signal controller box",
                "Public phone booth vandalized",
                "Street name sign missing or damaged",
                "Park bench broken and unsafe",
                "Public bin overflowing and damaged",
                "Broken street pole needs attention",
                "Damaged pedestrian crossing sign"
            };
        }

        public void SeedData(int numberOfRecords) {
            try {
                for (int i = 0; i < numberOfRecords; i++) {
                    Issue issue = GenerateRandomIssue();
                    dbHelper.SaveIssue(issue);
                }
            }
            catch (Exception ex) {
                throw new Exception("Error seeding data: " + ex.Message, ex);
            }
        }

        public void SeedRelatedIssues(int numberOfClusters, int issuesPerCluster) {
            for (int cluster = 0; cluster < numberOfClusters; cluster++) {
                string location = locations[random.Next(locations.Count)];
                string category = categories[random.Next(categories.Count)];

                for (int i = 0; i < issuesPerCluster; i++) {
                    Issue issue = GenerateIssueForLocation(location, category);
                    dbHelper.SaveIssue(issue);
                }
            }
        }

        private Issue GenerateRandomIssue() {
            string location = locations[random.Next(locations.Count)];
            string category = categories[random.Next(categories.Count)];
            return GenerateIssueForLocation(location, category);
        }

        private Issue GenerateIssueForLocation(string location, string category) {
            string description = GetDescriptionForCategory(category);
            string attachmentPath = string.Empty;

            Issue issue = new Issue {
                Location = location,
                Category = category,
                Description = description,
                AttachmentPath = attachmentPath,
                DateTime = GenerateRandomDateTime()
            };

            return issue;
        }

        private string GetDescriptionForCategory(string category) {
            switch (category.ToLowerInvariant()) {
                case "water":
                    return waterIssues[random.Next(waterIssues.Count)];
                case "electricity":
                    return electricityIssues[random.Next(electricityIssues.Count)];
                case "sanitation":
                    return sanitationIssues[random.Next(sanitationIssues.Count)];
                case "roads":
                    return roadsIssues[random.Next(roadsIssues.Count)];
                case "utilities":
                    return utilitiesIssues[random.Next(utilitiesIssues.Count)];
                default:
                    return "General municipal service issue requiring attention";
            }
        }

        private DateTime GenerateRandomDateTime() {
            DateTime start = DateTime.Now.AddMonths(-6);
            DateTime end = DateTime.Now;
            int range = (end - start).Days;
            return start.AddDays(random.Next(range)).AddHours(random.Next(24)).AddMinutes(random.Next(60));
        }

        public void ClearAllData() {
            try {
                List<Issue> allIssues = dbHelper.GetAllIssues();
                foreach (Issue issue in allIssues) {
                    if (!string.IsNullOrWhiteSpace(issue.UniqueID)) {
                        dbHelper.DeleteIssue(issue.UniqueID);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Error clearing data: " + ex.Message, ex);
            }
        }
    }
}
