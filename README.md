# TournamentOverlayChanger
I was the sole organizer, streamer and narrator for a local League of Legends tournament. So I needed a way to help me organize what was being displayed on the screen.

I used OBS and the logos for each team were images that I had to swap on it. But OBS also allows you to swap the image files themselves and they would reload. When a specific team is being streamed you'll want the overlay in the stream to be changed. So the application changes it for you automatically.

I made a tournament stream helper to manage which logos and images should show up on screen based on the current playing teams.

The app loads every folder inside the folder named "Teams" and puts them in a list. To add any team, you only need to create a new folder with the team's name inside the "Teams" folder You also need a score folder in the same place that the .exe is, with all the team options, and the images for each logo and score count.

Example:
```
 -overlayChanger.exe
|
|__ Teams
|
|__ Scores
   |
   |__ Team 1
   |__ Team 2
   |__ Team 3
```
