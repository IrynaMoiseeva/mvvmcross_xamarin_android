﻿using MvvmCross.Core.ViewModels;
using MvvmCross_Application1.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmCross_Application1.Core.ViewModels
{
    public class PlayVideoViewModel : MvxViewModel
    {
        private List<Video> videos;
        public const string ApiKey = "AIzaSyAn95XgsxmK2c3fwrtyV0-pxOm6RhIr-cI";
        public const string Key = "AIzaSyAqB61v3YI6H7Q-jhx3HVSPNBDvX-dr_yY";
        private string apiUrlForContent = "https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&maxResults=2&playlistId="
            + Key + "&key=" + ApiKey;

        //+""+"&key="+Deleliper.key
        private string apiUrlForPlaylist = "https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&maxResults=2&playlistId="
       + "PL8XvIF6dDmUs4bjs3qMbX7coPsqCCcGHu"
       //+ "Your_PlaylistId"
       + "&key="
       + ApiKey;

        // doc : https://developers.google.com/apis-explorer/#p/youtube/v3/youtube.search.list
        private string apiUrlForVideosDetails = "https://www.googleapis.com/youtube/v3/videos?part=snippet,statistics&id="
            + "{0}"
            //+ "0ce22qhacyo,j8GU5hG-34I,_0aQJHoI1e8"
            //+ "Your_Videos_Id"
            + "&key="
            + ApiKey;

        private List<YoutubeItem> youtubeItems;

        public List<YoutubeItem> YoutubeItems
        {
            get { return youtubeItems; }
            set
            {
                youtubeItems = value;
                RaisePropertyChanged(() => YoutubeItems);
                //  OnPropertyChanged();
            }
        }
        public List<Video> Videos //{ get; set; }
        {
            get { return videos; }
            set
            {
                videos = value;
                RaisePropertyChanged(() => Videos);
            }
        }
        public string VideoUrl { get; private set; }
        public string PlayListUrl { get { var b = youtubeItems.First(); return b.VideoId; }
            private set { RaisePropertyChanged(() => PlayListUrl); } }

        public void Prepare(string videourl)
        {
            VideoUrl = videourl;
        }
        public async Task InitDataAsync()
        {
            var videoIds = await GetVideosIdsFromPlayListAsync();
        }

        public async Task<List<string>> GetVideosIdsFromPlayListAsync()
        {
            var api_key = "03e8168ffbb8cafb6b8b6679c528ec97";
            var citytext = "Kiev";
            //string URL = ("http://api.openweathermap.org/data/2.5/weather?q=" + citytext + "&appid=" + api_key);
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(apiUrlForPlaylist);
            var f = new List<string>();
            var videoIds = new List<string>();

            try
            {
                JObject response = JsonConvert.DeserializeObject<dynamic>(json);

                var items = response.Value<JArray>("items");

                foreach (var item in items)
                {
                    videoIds.Add(item.Value<JObject>("contentDetails")?.Value<string>("videoId"));
                };

               YoutubeItems = await GetVideosDetailsAsync(videoIds);
                
            }
            catch (Exception exception)
            {
            }

            return videoIds;
        }

        private async Task<List<YoutubeItem>> GetVideosDetailsAsync(List<string> videoIds)
        {

           var videoIdsString = "";
            foreach (var s in videoIds)
            {
                videoIdsString += s + ",";
            }

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(string.Format(apiUrlForVideosDetails, videoIdsString));

            var youtubeItems = new List<YoutubeItem>();

            try
            {
                JObject response = JsonConvert.DeserializeObject<dynamic>(json);

                var items = response.Value<JArray>("items");

                foreach (var item in items)
                {
                    var snippet = item.Value<JObject>("snippet");
                    var statistics = item.Value<JObject>("statistics");

                    var youtubeItem = new YoutubeItem
                    {
                        Title = snippet.Value<string>("title"),
                        Description = snippet.Value<string>("description"),
                        ChannelTitle = snippet.Value<string>("channelTitle"),
                        PublishedAt = snippet.Value<DateTime>("publishedAt"),
                        VideoId = item?.Value<string>("id"),
                        DefaultThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("default")?.Value<string>("url"),
                        MediumThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("medium")?.Value<string>("url"),
                        HighThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("high")?.Value<string>("url"),
                        StandardThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("standard")?.Value<string>("url"),
                        MaxResThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("maxres")?.Value<string>("url"),

                        ViewCount = statistics?.Value<int>("viewCount"),
                        LikeCount = statistics?.Value<int>("likeCount"),
                        DislikeCount = statistics?.Value<int>("dislikeCount"),
                        FavoriteCount = statistics?.Value<int>("favoriteCount"),
                        CommentCount = statistics?.Value<int>("commentCount"),

                        Tags = (from tag in snippet?.Value<JArray>("tags") select tag.ToString())?.ToList(),
                    };

                    youtubeItems.Add(youtubeItem);
                }
                
                return youtubeItems;
            }
            catch (Exception exception)
            {
                return youtubeItems;
            }
        }




        public PlayVideoViewModel()
        {
            InitDataAsync();
            Videos = new List<Video> {
                new Video("YouTube Collection11", "Y_UmWdcTrrc" ),
                new Video("GMail Tap","1KhZKNZO8mQ")};
            var f = 1;
        }
        private MvxCommand<Video> selectvideo;
     /*   public ICommand SelectDogCommand
        {
            get
            {
                selectvideo = selectvideo ?? new MvxCommand<Video>(dog => _parent.SelectDog(dog));
                return selectvideo;
            }
        }*/
        public MvxCommand<Video> ShowVideoCommand
        {
            get
            {
                return new MvxCommand<Video>(SelectedVideo => {
                    
          {
                        VideoUrl = SelectedVideo.Url;
                        // do something
                    }
                } ); 
            }
        }
        public void method(int item)
        {

         //   var PlaylistRequest = YouTubeService.PlaylistItems.List("snippet");
        }
       
    }
        
      


    }

