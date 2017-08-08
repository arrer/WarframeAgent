﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Core.Model;

namespace Core.ViewModel
{
    public class InvasionViewModel : VM
    {
        private Invasion invasion;

        public InvasionViewModel(Invasion invasion)
        {
            this.invasion = invasion;
            Id = invasion.Id;
            isDefenderFactionInfestation = invasion.DefenderMissionInfo.Faction == "FC_INFESTATION";
            AttackerFaction = FactionViewModel.ById(invasion.AttackerMissionInfo.Faction);
            DefenderFaction = FactionViewModel.ById(invasion.DefenderMissionInfo.Faction);
            Faction = FactionViewModel.ById(invasion.Faction);
            NodeArray = invasion.Node.GetFilter(Model.Filters.FilterType.Planet).FirstOrDefault().Key.ToUpper().Split('|');
            LocTag = invasion.LocTag.GetFilter(Model.Filters.FilterType.Mission).FirstOrDefault().Key;
            DefenderReward = GetRewardString(invasion.DefenderReward);
            AttackerReward = GetRewardString(invasion.AttackerReward);
            Update();
        }

        public void Update()
        {
            Count = invasion.Count;
            Goal = invasion.Goal;
            UpdatePercent();
        }

        public Id Id { get; }
        public string LocTag { get; }
        public FactionViewModel Faction { get; }
        public string[] NodeArray { get; }
        public FactionViewModel DefenderFaction { get; }
        public FactionViewModel AttackerFaction { get; }
        public string DefenderReward { get; }
        public string AttackerReward { get; }

        private readonly bool isDefenderFactionInfestation;

        private double _count;
        public double Count
        {
            get => _count;
            private set => Set(ref _count, value);
        }

        private double _goal;
        public double Goal
        {
            get => _goal;
            private set => Set(ref _goal, value);
        }

        static string GetRewardString(InvasionReward reward)
        {
            var rewardString = reward?.CountedItems[0]?.ItemType.GetFilter(Model.Filters.FilterType.Item).FirstOrDefault().Key;
            var count = reward?.CountedItems[0]?.ItemCount;
            if (count > 1)
                rewardString += $" [{count}]";

            if (string.IsNullOrEmpty(rewardString))
                return "Награды нет";

            return rewardString;
        }

        void UpdatePercent()
        {
            var val = isDefenderFactionInfestation
                    ? (Goal + Count) / Goal * 100
                    : (Goal + Count) / (Goal * 2) * 100;

            Percent = val;
            PercentOut = 100 - val;
        }

        private double _percentOut;
        public double PercentOut
        {
            get => _percentOut;
            private set => Set(ref _percentOut, value);
        }

        private double _percent;
        public double Percent
        {
            get => _percent;
            private set => Set(ref _percent, value);
        }
    }
}
