using Accord.MachineLearning.Rules;
using KeePark.Data;
using KeePark.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

// Important : In Order to use the Accord package you should download it
// through the nugget package manager and run the following commands:
// pm> Install-Package Accord -Version 3.8.2-alpha
// pm> Install-Package Accord -Version 3.8.2
// pm> Install-Package Accord.MachineLearning
// pm> Install-Package Accord.Math -Version 3.8.0
// pm> Install-Package Accord.Statistics -Version 3.8.0

// all packages can be found at the path C:\Users\<userName>\.nuget\packages


namespace KeePark.Services
{
    public class MLApriori
    {
        private readonly KeeParkContext _KeeParkContext;
        private readonly IdentityContext _context;
        // _threshold is equivelent to the support - basiclly its the amount of times an item is found in different baskets/sub-list in the matrix from the dataset
        private readonly int _threshold;
        // _confidense is an indication of how often the rule has been found true
        private readonly double _minConfidence;
        // An “Association Rule” (defined) – an implication of two itemsets, for which there’s a direct,
        // evident and unambiguous relationship between the specific items in these both sets
        AssociationRuleMatcher<int> classifier = null;

        public MLApriori(IdentityContext db, KeeParkContext kpb)
        {
            _context = db;
            _KeeParkContext = kpb;
            _threshold = 2;
            _minConfidence = .1;
        }
        // thats will be excecuted to update the classifier 
        public async Task UpdateRecommendedSpotsAsync()
        {
            await Task.Run(() =>
            {
                RecommendedSpots();
            });
        }

        public void RecommendedSpots()
        {
            // First I bring the entire usrs from the DB
            List<GeneralUser> allUsers = _context.Users.ToList();
            // Second I am listing whole spots shown in the entire system users history
            List<int[]> allHistorySpots = new List<int[]>();

            allUsers.ForEach(user =>
            {
                if (!string.IsNullOrEmpty(user.History))
                    // we are inserting to the allHistory list chunks of the user history -- [ [spotID ,spotID ,spotID ,spotID ] , [spotID ,spotID ,spotID ,spotID ] , [spotID, spotID] ]
                    allHistorySpots.Add(user.History.Split(",").Select(int.Parse).ToArray());
            });

            int[][] dataSet = allHistorySpots.ToArray();
            // Apriori Algorithm is used to determine the frequent spot in the entire transactions found in the DB.
            // Create a new a-priori learning algorithm with the properties we set at the C-TOR 
            Apriori apriori = new Apriori(_threshold, _minConfidence);

            // And now we will create the learning on the array we prepared before
            classifier = apriori.Learn(dataSet);
        }

        // getting the identity id of the user!
        public List<ParkingSpot> GetRecommendedSpots(int[] uid)
        {
            if (classifier == null)
                RecommendedSpots();

            int[][] matches = classifier.Decide(uid);

            List<int> similarItems = new List<int>();

            // that nested forEach loop is to convert the matrix into list of the recomended spotsID's
            foreach (int[] match in matches)
            {
                foreach (int item in match)
                {
                    similarItems.Add(item);
                }
            }

            // 1 most recommended items - using the hash-set
            var similarIds = similarItems.ToHashSet().Take(1); 
            // getting the spot from the DB
            var getSpot = _KeeParkContext.ParkingSpot.Where(x => similarIds.Contains(x.ParkingSpotID));

            return getSpot.ToList();
        }
    }
}