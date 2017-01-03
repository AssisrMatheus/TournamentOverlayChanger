using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayChanger.Classes
{
    public class Team : NotifyPropertyBase
    {
        public string Name { get; set; }

        public string[] FilesInFolder { get; set; }

        private int score { get; set; }

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                if (score != value)
                    this.score = value;

                OnPropertyChanged("Score");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Name, this.Score);
        }
    }
}
