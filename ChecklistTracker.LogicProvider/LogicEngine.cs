using ChecklistTracker.Config;
using ChecklistTracker.LogicProvider.DataFiles;
using ChecklistTracker.LogicProvider.DataFiles.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.LogicProvider
{
    public class LogicEngine
    {
        private ISet<string> CheckedLocations = new HashSet<string>();

        private LocationsData Locations;
        private LogicHelpers Helpers;
        private TrackerConfig Config;

        private ConcurrentDictionary<string, HintRegion> HintRegions;

        public LogicEngine(string version, Settings settings)
        {
            Config = TrackerConfig.Init().Result;

            var logicFiles = LogicFiles.LoadLogicFiles(LogicFileCache.GetCachedLogicFilesForTagAsync(version).Result).Result;

            Locations = LocationsData.Initialize(Config, logicFiles, settings).Result;

            Helpers = LogicHelpers.InitHelpers(Config, logicFiles, settings, Locations).Result;

            Inventory = new Dictionary<string, int>(Config.DefaultInventory);
            
            foreach (var equip in settings.StartingEquipment)
            {
                Inventory[equip.Replace(" ", "_")] = 1;
            }
            foreach (var inv in settings.StartingInventory)
            {
                Inventory[inv.Replace(" ", "_")] = 1;
            }
            foreach (var items in settings.StartingItems)
            {
                Inventory[items.Key.Replace(" ", "_")] += items.Value;
            }
            foreach (var song in settings.StartingSongs)
            {
                Inventory[song.Replace(" ", "_")] = 1;
            }

            InitRegions();

            UpdateItems(Inventory);
        }

        public IDictionary<string, int> Inventory { get; private set; }

        public void UpdateItems(IDictionary<string, int> items)
        {
            Inventory = items;
            Helpers.UpdateItems(Inventory);
            UpdateLocationData();
        }

        private void InitRegions()
        {
            HintRegions = new ConcurrentDictionary<string, HintRegion>();

            foreach (var location in Locations.Locations.SelectMany(kv => kv.Value.Values).SelectMany(kv => kv.Values))
            {
                var hintRegionName = Locations.RegionMap.TryGetValue(location.ParentRegion, out var mappedRegion) ? mappedRegion : location.ParentRegion;
                Config.HintRegionShortNames.TryGetValue(hintRegionName, out var regionShortName);
                var hintRegion = HintRegions.GetOrAdd(hintRegionName, (region) => new HintRegion(region, regionShortName));

                var locationData = new LocationInfo(hintRegion, location.LocationName);
                hintRegion.Locations.Add(locationData);
            }
        }

        private void UpdateLocationData()
        {
            foreach (var hintRegion in HintRegions.Values)
            {
                var active = false;
                foreach (var location in hintRegion.Locations)
                {
                    location.IsActive = Locations.ActiveLocations.ContainsKey(location.Name);
                    if (location.IsActive)
                    {
                        location.IsAccessible = Helpers.IsLocationAvailable(location.Name);
                        location.IsProgress = Locations.IsProgessLocation(Locations.ActiveLocations[location.Name]);
                        location.IsSkull = Locations.ActiveSkullsLocations.ContainsKey(location.Name);
                    }
                    else
                    {
                        location.IsAccessible = false;
                        location.IsProgress = false;
                        location.IsSkull = false;
                    }
                    active |= location.IsAccessible;
                }
                hintRegion.IsActive = active;
            }
        }

        public IList<HintRegion> GetRegions()
        {
            return HintRegions.Values.ToList();
        }

        public IList<string> GetLocations()
        {
            return Locations.ActiveLocations
                .Where(kv => CanAccess(kv.Key) && Locations.IsProgessLocation(kv.Value))
                .Select(kv => kv.Key)
                .ToList();
        }

        public void CheckLocation(string location)
        {
            CheckedLocations.Add(location);
        }

        public bool IsChecked(string location)
        {
            return CheckedLocations.Contains(location);
        }

        public bool CanAccess(string location)
        {
            if (!Locations.ActiveLocations.ContainsKey(location))
            {
                return false;
            }

            return Helpers.IsLocationAvailable(location);
        }

        public bool CanAccessEvent(string eventName)
        {
            if (!Locations.ActiveEvents.ContainsKey(eventName))
            {
                return false;
            }

            return Helpers.EvalEvent(eventName);
        }

        public bool CanAccessRegion(string region, string age)
        {
            return Helpers.IsRegionAccessible(region, age);
        }

        public bool IsProgressLocation(string location)
        {
            return Locations.ActiveLocations.TryGetValue(location, out var loc) && Locations.IsProgessLocation(loc);
        }

        public ISet<string> GetAvailableSkulls()
        {
            return Helpers.GetAccessilbeSkulls();
        }
    }
}
