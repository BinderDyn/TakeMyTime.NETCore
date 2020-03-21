using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Tests
{
    [TestClass]
    public class WeekdayProductivityManagerTests
    {
        private WeekdayProductivityManager manager;

        [TestInitialize]
        public void Initialize()
        {
            this.manager = new WeekdayProductivityManager();
        }

        private Entry[] GenerateEntries()
        {
            return new Entry[]
            {
                new Entry
                {
                    DurationAsTicks = TimeSpan.FromHours(1).Ticks,
                    Started = new DateTime(2020, 3, 16)
                },
                new Entry
                {
                    DurationAsTicks = TimeSpan.FromHours(1).Ticks,
                    Started = new DateTime(2020, 3, 17)
                },
                new Entry
                {
                    DurationAsTicks = TimeSpan.FromHours(1).Ticks,
                    Started = new DateTime(2020, 3, 10)
                },
                new Entry
                {
                    DurationAsTicks = TimeSpan.FromHours(1).Ticks,
                    Started = new DateTime(2020, 3, 18)
                },
                new Entry
                {
                    DurationAsTicks = TimeSpan.FromHours(1).Ticks,
                    Started = new DateTime(2020, 3, 19)
                },
                new Entry
                {
                    DurationAsTicks = TimeSpan.FromHours(1).Ticks,
                    Started = new DateTime(2020, 3, 20)
                }
            };
        }

        [TestMethod]
        public void TestDayWithoutEntries()
        {
            // ARRANGE
            var entries = this.GenerateEntries();

            // ACT
            foreach (var entry in entries)
            {
                this.manager.ProcessEntry(entry);
            }
            var result = this.manager.GetResults();

            // ASSERT
            double expectedTotalResultMonday = 0;
            double expectedAverageHoursMonday = 0;
            double expectedShareMonday = 0;

            Assert.AreNotEqual(expectedTotalResultMonday, result.Single(r => r.Day == DayOfWeek.Monday).TotalHours);
            Assert.AreNotEqual(expectedAverageHoursMonday, result.Single(r => r.Day == DayOfWeek.Monday).AverageHours);
            Assert.AreNotEqual(expectedShareMonday, result.Single(r => r.Day == DayOfWeek.Monday).Value);
        }

        [TestMethod]
        public void TestDayWithEntries()
        {
            // ARRANGE
            var entries = this.GenerateEntries();

            // ACT
            foreach (var entry in entries)
            {
                this.manager.ProcessEntry(entry);
            }
            var result = this.manager.GetResults();

            // ASSERT
            double expectedTotalResultTuesday = 2;
            double expectedAverageHoursTuesday = 1;
            double expectedShareTuesday = 33; // 100 / 6 = 16,7 => 16,7 x 2 ~ 33
            double expectedShareWednesday = 17;

            Assert.AreEqual(expectedTotalResultTuesday, result.Single(r => r.Day == DayOfWeek.Tuesday).TotalHours);
            Assert.AreEqual(expectedAverageHoursTuesday, result.Single(r => r.Day == DayOfWeek.Tuesday).AverageHours);
            double roundedResultOfTuesdayShares = result.Where(r => r.Day == DayOfWeek.Tuesday).Sum(d => Math.Round(d.Value * 100));
            Assert.AreEqual(expectedShareTuesday, roundedResultOfTuesdayShares);
            double roundedResultOfWednesdayShares = Math.Round(result.Single(r => r.Day == DayOfWeek.Wednesday).Value * 100, 1);
            Assert.AreEqual(expectedShareWednesday, roundedResultOfWednesdayShares);
        }
    }
}
