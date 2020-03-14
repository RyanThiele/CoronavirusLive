﻿using CoronaVirusLive.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CoronaVirusLive.ViewModels
{
    public class MapViewModel : BaseViewModel
    {

        private readonly List<Pin> pins = new List<Pin>();


        #region Constructors

        public MapViewModel()
        {
            Title = "Map";

            MessagingCenter.Subscribe<MainPageViewModel, IEnumerable<Case>>(this, "CasesUpdated", (sender, arg) =>
            {
                Cases.Clear();
                pins.Clear();

                if (arg != null && arg.Count() > 0)
                {
                    foreach (Case model in arg)
                    {
                        Cases.Add(model);
                        pins.Add(new Pin()
                        {
                            Position = new Position(model.Latitude, model.Longitude),
                            Label = $"Confirmed: {model.Confirmed} Dead: {model.Deaths} Recovered: {model.Recovered}",
                            Address = $"{model.ProvinceState} {model.CountryRegion}",
                            Type = PinType.Place
                        });
                    }

                    var modelsWithDates = arg.Where(x => x.LastUpdate > new DateTime()).ToList();
                    if (modelsWithDates != null || modelsWithDates.Count > 0)
                    {
                        var latestModel = modelsWithDates.OrderByDescending(x => x.LastUpdate).First();
                        Status = $"Latest Update: {latestModel.LastUpdate.ToString()}";
                    }
                    else
                    {
                        Status = $"No updates found.";
                    }

                }
                else
                {
                    Status = $"No updates found.";
                }



                MessagingCenter.Send<MapViewModel, IEnumerable<Pin>>(this, "PinsUpdated", pins);

            });
        }

        #endregion Constructors

        #region Messages




        #endregion Messages

        #region Properties

        public ObservableCollection<Case> Cases { get; set; } = new ObservableCollection<Case>();

        string status = null;
        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        #endregion Properties


        #region Commands

        #endregion Commands

        #region Methods



        #endregion Methods





    }
}
