using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coin_counting
{
    class Program
    {
        private static List<Listing> stuff = new List<Listing>();
        private static List<string> Servers = new List<String>()
        {
            "Adamantoise", "Cactuar", "Faerie", "Gilgamesh", "Jenova", "Midgardsormr", "Sargatanas", "Siren"
        };

        private static List<List<Listing>> GoodStuff = new List<List<Listing>>();

        public static void Main(string[] args)
        {
            int newAlgoWins = 0;
            int oldAlgoWins = 0;

            int newAlgoNumListings = 0;
            int oldAlgoNumListings = 0;

            long newAlgoTime = 0;
            long oldAlgoTime = 0;

            int i = 0;

            while (i < 100)
            {
                // generate dataset
                MakeABunchOfNumbers();

                // generate combinations of dataset whose quantity adds up to target
                int target = 100;

                // new algo
                var newAlgoTimer = new Stopwatch();
                newAlgoTimer.Start();

                sum_up(stuff, target);
                var newAlgo = GoodStuff.OrderBy(x => x.Sum(y => y.Price * y.Quantity))
                    .ToList().FirstOrDefault();

                newAlgoTimer.Stop();
                newAlgoTime += newAlgoTimer.ElapsedMilliseconds;

                // old algo 
                var oldAlgoTimer = new Stopwatch();
                oldAlgoTimer.Start();

                var oldAlgo = OldStuff.GetMostEfficientPurchases(stuff, target).FirstOrDefault();

                oldAlgoTimer.Stop();
                oldAlgoTime += oldAlgoTimer.ElapsedMilliseconds;

                // sum up algo result totals
                var newSum = 0;
                foreach (var wot in newAlgo)
                {
                    newSum += wot.Price;
                }
                newAlgoNumListings += newAlgo.Count();

                var oldSum = 0;
                foreach (var wot in oldAlgo)
                {
                    oldSum += wot.Price;
                }
                oldAlgoNumListings += oldAlgo.Count();

                // compare totals
                if (newSum < oldSum)
                    newAlgoWins += 1;
                else
                    oldAlgoWins += 1;

                Console.WriteLine($"Iteration {i} complete");
                i++;
            }

            Console.WriteLine($"New algo wins: {newAlgoWins} | Old algo wins: {oldAlgoWins}");
            Console.WriteLine($"New algo listings: {newAlgoNumListings} | Old algo listings: {oldAlgoNumListings}");
            Console.WriteLine($"New algo time: {newAlgoTime}ms | Old algo time: {oldAlgoTime}ms");

            Console.ReadLine();
        }

        private static void sum_up(List<Listing> listings, int target)
        {
            sum_up_recursive(listings, target, new List<Listing>());
        }

        private static void sum_up_recursive(List<Listing> listings, int target, List<Listing> partialListings)
        {
            int sum = 0;
            foreach (var x in partialListings)
            {
                sum += x.Quantity;
            }                

            if (sum >= target)
            {
                GoodStuff.Add(partialListings);
                return;
            }

            //Debug.WriteLine(sum);
            for (int i = 0; i < listings.Count; i++)
            {
                List<Listing> remaining = new List<Listing>();
                Listing n = listings[i];
                for (int j = i + 1; j < listings.Count; j++) remaining.Add(listings[j]);

                List<Listing> partial_rec = new List<Listing>(partialListings);
                partial_rec.Add(n);
                sum_up_recursive(remaining, target, partial_rec);
            }
        }


        private static void MakeABunchOfNumbers()
        {
            // clear dataset
            stuff = new List<Listing>();

            Random rng = new Random();
            var i = 0;
            while (i < 17)
            {
                var thingy = new Listing
                {
                    Quantity = rng.Next(1, 99),
                    Server = Servers[rng.Next(0, 7)],
                    Price = rng.Next(3000, 6000)
                };
                stuff.Add(thingy);
                i++;
            }
        }


    }
}
