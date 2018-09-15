using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmCross_Application1.Core.Model
{
    [Table(nameof(Video))]
    public class Video
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }
        public string Url { get; set; }
        public Video(string id, string url)
        {
            Url = url;
            Id = id;
            
        }
    }
}
