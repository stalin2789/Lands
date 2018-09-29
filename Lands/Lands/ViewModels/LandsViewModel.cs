﻿namespace Lands.ViewModels
{
    using Lands.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Services;
    using Xamarin.Forms;

    public class LandsViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Atributtes
        private ObservableCollection<Land> lands;
        #endregion

        #region Properties
        public ObservableCollection<Land> Lands
        {
            get
            {
                return this.lands;
            }
            set
            {
                SetValue(ref this.lands, value);
            }
        }
        //Observable collection para poder pintar en el listview
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLans();
        }
        #endregion

        #region Methods
        private async void LoadLans()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                    connection.Message, 
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await this.apiService.GetList<Land>("http://restcountries.eu", "/rest", "/v2/all");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var list = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(list);
        }
        #endregion
    }
}
