﻿using VeganPhoneApp.Resources;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Newtonsoft.Json;
using VeganPhoneApp.Models;
using System.Windows;


namespace VeganPhoneApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
       
        
        //const string apiUrl = @"http://169.254.21.12/api/Users";  had here originally but moved it down to LoadData() method. 
        public MainViewModel()
        {
            /* var hostnames = Windows.Networking.Connectivity.NetworkInformation.GetHostNames();  //code to check the internal address of WP8E
             foreach (var hn in hostnames)
             {
                 if (hn.IPInformation != null)
                 {
                     string ipAddress = hn.DisplayName;
                 }
             } */
            this.Items = new ObservableCollection<ItemViewModel>();

        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (this.IsDataLoaded == false)
            {
                //this.Items.Clear();
                //this.Items.Add(new ItemViewModel() { ID = "0", LineOne = "Please Wait...", LineTwo = "Please wait while the catalog is downloaded from the server.", LineThree = null });
                WebClient webClient = new WebClient();
                webClient.Headers["Accept"] = "application/json";
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadCatalogCompleted); //initiates event handler
                
                
                //webClient.DownloadStringAsync(new Uri(@"http://169.254.21.12/api/Users"));
                webClient.DownloadStringAsync(new Uri(@"http://169.254.21.12/api/Restaurant"));  //this gets data from VeganWebApi and passes it to DownloadStringCompletedEventArgs e
               
            }
        }

        private void webClient_DownloadCatalogCompleted(object sender, DownloadStringCompletedEventArgs e)  //sender is the object in XAML that the handler is attached to.  e is the data that is sent. 
        // DownloadStringCompletedEventArgs gets the data that is downloaded by the DownloadStringAsync method.
        {
            
            try
            {
                //this.Items.Clear();
                if (e.Result != null)
                {
                    //var users = JsonConvert.DeserializeObject<User[]>(e.Result);
                    var restaurants = JsonConvert.DeserializeObject<Restaurant[]>(e.Result);
                    int id = 0;
                   
                   
                    //foreach (User user in users)
                    foreach (Restaurant restaurant in restaurants)
                    {
                        this.Items.Add(new ItemViewModel()
                        {
                            ID = (id++).ToString(),

                            //LineOne = user.UserName,
                            LineOne = restaurant.RestaurantName,
                            //LineTwo = user.Password,
                            LineTwo = restaurant.Rating.ToString()
                            //LineThree = restaurant.Rating.ToString(),

                            

                        });
                    }
                     
                    this.IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                this.Items.Add(new ItemViewModel()
                {
                    ID = "0",
                    LineOne = "An Error Occurred",
                    LineTwo = String.Format("The following exception occured: {0}", ex.Message),
                    LineThree = String.Format("Additional inner exception information: {0}", ex.InnerException.Message)

                });
            }
        }

      /*   private Rating _newRating;
      /// <summary>
      /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
      /// </summary>
      /// <returns></returns>
      public string NewRating
      {
          get
          {
              return _newRating;
          }
          set
          {
              if (value != _newRating)
              {
                  _newRating = value;
                  NotifyPropertyChanged("NewRating");
              }
          }
      }
 
        */

        public event PropertyChangedEventHandler PropertyChanged; //an event declaration of delegate type PropertyChangedEventHandler
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}