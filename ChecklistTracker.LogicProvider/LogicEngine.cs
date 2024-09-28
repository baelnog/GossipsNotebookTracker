using ChecklistTracker.Config;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static ChecklistTracker.LogicProvider.LocationsData;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.LogicProvider
{
    public partial class LogicEngine : INotifyPropertyChanged
    {
        private ISet<string> CheckedLocations = new HashSet<string>();

        private LocationsData Locations;
        private LogicHelpers Helpers;
        private TrackerConfig Config;

        private ConcurrentDictionary<string, HintRegion> HintRegions;
        private IDictionary<string, (HintRegion, LocationInfo, RuleData)> ActiveLocations;

        public event PropertyChangedEventHandler? PropertyChanged;

        public LogicEngine(TrackerConfig config, string version)
        {
            Config = config;

            var logicFiles = LogicFiles.LoadLogicFiles(LogicFileCache.GetCachedLogicFilesForTagAsync(version).Result).Result;

            Locations = LocationsData.Initialize(Config, logicFiles).Result;

            Helpers = LogicHelpers.InitHelpers(Config, logicFiles, Locations).Result;

            Inventory = new Dictionary<string, int>(Config.DefaultInventory);

            foreach (var equip in Config.RandomizerSettings.StartingEquipment)
            {
                Inventory[equip.Replace(" ", "_")] = 1;
            }
            foreach (var inv in Config.RandomizerSettings.StartingInventory)
            {
                Inventory[inv.Replace(" ", "_")] = 1;
            }
            foreach (var items in Config.RandomizerSettings.StartingItems)
            {
                Inventory[items.Key.Replace(" ", "_")] += items.Value;
            }
            foreach (var song in Config.RandomizerSettings.StartingSongs)
            {
                Inventory[song.Replace(" ", "_")] = 1;
            }

            InitRegions();

            UpdateItems(Inventory);

            Locations.PropertyChanged += LocationDataChanged;
        }

        private void LocationDataChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateItems(Inventory);
        }

        public IDictionary<string, int> Inventory { get; private set; }

        public void UpdateItems(IDictionary<string, int> items)
        {
            lock (this)
            {
                Inventory = items;
                Helpers.UpdateItems(Inventory);
                UpdateLocationData();
            }
        }

        private void InitRegions()
        {
            HintRegions = new ConcurrentDictionary<string, HintRegion>();

            foreach (var location in Locations.ActiveLocations.Values)
            {
                var hintRegionName = location.HintRegion;
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
                    var locationData = Locations.ActiveLocations[location.Name];
                    location.IsActive = true; // TODO MQ: locationData.IsDungeon ? !locationData.IsMq : true;
                    if (location.IsActive)
                    {
                        location.Accessiblity = Helpers.IsLocationAvailable(location.Name);
                        location.IsProgress = Locations.IsProgessLocation(Locations.ActiveLocations[location.Name]);
                        location.IsSkull = Locations.ActiveSkullsLocations.ContainsKey(location.Name);
                    }
                    else
                    {
                        location.Accessiblity = Accessibility.None;
                        location.IsProgress = false;
                        location.IsSkull = false;
                    }
                    active |= location.Accessiblity > 0;
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
            return Helpers.IsLocationAvailable(location).HasFlag(Accessibility.Synthetic);
        }

        public bool CanAccessEvent(string eventName)
        {
            if (!Locations.ActiveEvents.ContainsKey(eventName))
            {
                return false;
            }

            return Helpers.EvalEvent(eventName).HasFlag(Accessibility.Synthetic);
        }

        public bool CanAccessRegion(string region, string age)
        {
            return Helpers.IsRegionAccessible(region, age).HasFlag(Accessibility.Synthetic);
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
