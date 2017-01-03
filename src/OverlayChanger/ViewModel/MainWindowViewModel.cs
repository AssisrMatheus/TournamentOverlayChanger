using OverlayChanger.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OverlayChanger.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyBase
    {
        public MainWindow View { get; set; }

        private ObservableCollection<Team> teams;
        public ObservableCollection<Team> Teams
        {
            get
            {
                return this.teams;
            }
            set
            {
                if (teams != value)
                    this.teams = value;

                OnPropertyChanged("Teams");
            }
        }

        public Team SelectedTeam { get; set; }

        public Team SelectedPlayingTeam { get; set; }

        private ObservableCollection<Team> teamsPlaying;
        public ObservableCollection<Team> TeamsPlaying
        {
            get
            {
                return this.teamsPlaying;
            }
            set
            {
                if (teams != value)
                    this.teamsPlaying = value;

                OnPropertyChanged("TeamsPlaying");
            }
        }

        private List<string> ScoreImagesPaths { get; set; }

        public ICommand InsertTeamCommand { get; set; }

        public ICommand RemoveTeamCommand { get; set; }

        public ICommand AddScoreCommand { get; set; }

        public ICommand SubtractScoreCommand { get; set; }

        public ICommand InvertCommand { get; set; }

        public MainWindowViewModel()
        {
            this.Teams = new ObservableCollection<Team>();
            this.TeamsPlaying = new ObservableCollection<Team>();

            this.InsertTeamCommand = new Command(parameters =>
            {
                if (this.SelectedTeam == null)
                    MessageBox.Show("You must select a team");
                else
                {
                    if (this.TeamsPlaying.Count >= this.ScoreImagesPaths.Count)
                        MessageBox.Show("You can't have more teams than the score folder has images. Adds more score images to the new team first");
                    else
                    {
                        if (this.TeamsPlaying.Any(team => team == this.SelectedTeam))
                            MessageBox.Show("The team is already on the playing teams' list");
                        else
                            this.AddTeam(this.SelectedTeam);
                    }
                }
            });

            this.RemoveTeamCommand = new Command(parameters =>
            {
                if (this.SelectedPlayingTeam == null)
                    MessageBox.Show("You must select a playing team");
                else
                {
                    this.RemoveTeam(this.SelectedTeam);
                }                
            });

            this.AddScoreCommand = new Command(parameters =>
            {
                if (this.SelectedPlayingTeam == null)
                    MessageBox.Show("You must select a playing team");
                else
                {
                    this.SelectedPlayingTeam.Score += 1;
                    this.CopyScoreToTeam(this.SelectedPlayingTeam.Score, this.TeamsPlaying.IndexOf(this.SelectedPlayingTeam) + 1);
                }
            });

            this.SubtractScoreCommand = new Command(parameters =>
            {
                if (this.SelectedPlayingTeam == null)
                    MessageBox.Show("You must select a playing team");
                else
                {
                    this.SelectedPlayingTeam.Score -= 1;
                    this.CopyScoreToTeam(this.SelectedPlayingTeam.Score, this.TeamsPlaying.IndexOf(this.SelectedPlayingTeam) + 1);
                }
            });

            this.InvertCommand = new Command(parameters =>
            {
                //Inverts the array
                var invertedArray = this.TeamsPlaying.Reverse().ToList();
                var latestArray = this.TeamsPlaying.ToList();

                //Removes the existing teams folders in the current order
                foreach (var team in latestArray)
                {
                    this.RemoveTeam(team);
                }

                //Recreates the folder in the new order
                foreach (var team in invertedArray)
                {
                    this.AddTeam(team);
                }
            });

            this.rootPath = "B:\\Liga 2016\\Overlay";
        }

        private string rootPath;
        private string scoresPath = "Scores";

        public void LoadTeams()
        {
            //Gets every folder in the given rootPath
            var folders = Directory.GetDirectories(Path.Combine(this.rootPath, "Teams"));

            //Extract the team name and logo path for each folder inside the rootPath
            var teams = folders.Select(folderPath =>
            {
                return new Team
                {
                    Name = Path.GetFileName(folderPath),
                    FilesInFolder = Directory.GetFiles(folderPath)
                };
            });

            //Set the found teams to the list, this will show each team on the left list
            this.Teams = new ObservableCollection<Team>(teams.ToList());

            this.ScoreImagesPaths = Directory.GetDirectories(Path.Combine(rootPath, scoresPath)).ToList();
        }

        public void AddTeam(Team teamToAdd)
        {
            var teamFolderName = string.Format("Team {0}", this.TeamsPlaying.Count+ 1);
            var teamPath = Path.Combine(rootPath, teamFolderName);

            //Creates the current team directory
            if (!Directory.Exists(teamPath))
                Directory.CreateDirectory(teamPath);

            //For each file in the team folder, copy it to the current team folder
            foreach (var file in teamToAdd.FilesInFolder)
            {
                var filename = Path.GetFileName(file);

                //If there's team in the file name, we can only take the file for the current team
                if (filename.Contains("team") && !filename.Contains((this.TeamsPlaying.Count + 1).ToString()))
                    continue;

                var destination = Path.Combine(teamPath, filename);

                if (File.Exists(destination))
                    File.Delete(destination);

                File.Copy(file, destination);
            }

            this.CopyScoreToTeam(teamToAdd.Score, this.TeamsPlaying.Count + 1);

            this.TeamsPlaying.Add(teamToAdd);
        }

        public void RemoveTeam(Team teamToRemove)
        {
            var teamFolderName = string.Format("Team {0}", this.TeamsPlaying.Count);
            var teamPath = Path.Combine(rootPath, teamFolderName);

            if (Directory.Exists(teamPath))
            {
                var files = Directory.GetFiles(teamPath);
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                Directory.Delete(teamPath);
            }

            this.TeamsPlaying.Remove(teamToRemove);
        }

        public void CopyScoreToTeam(int score, int team)
        {
            //Gets the given team's folder name
            var teamFolderName = string.Format("Team {0}", team);
            var teamPath = Path.Combine(rootPath, teamFolderName);

            //Gets the score path inside the root folder
            var scorePath = Path.Combine(rootPath, scoresPath);

            //Gets the team path inside the score path
            var teamScorePath = Path.Combine(scorePath, teamFolderName);

            //Gets what file must be copied. The score file must be the one named after the current team's score
            var scoreFile = Directory.GetFiles(teamScorePath).Where(filePath => Path.GetFileNameWithoutExtension(filePath) == score.ToString()).FirstOrDefault();            

            if(scoreFile != null)
            {
                //Gets the score's image extention
                var extension = Path.GetExtension(scoreFile);

                //Combine the default score file to its extension renaming it to "score"
                var fileName = string.Format("score{0}", extension);

                var destination = Path.Combine(teamPath, fileName);

                if (File.Exists(destination))
                    File.Delete(destination);

                File.Copy(scoreFile, destination);
            }                
        }
    }
}
