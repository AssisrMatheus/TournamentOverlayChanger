# TournamentOverlayChanger
Change files based on wich team is playing

Sometimes I create game tournaments and stream its matches.

In the stream we have an overlay for each team, images placed above the game's image that shows what are the current teams that are being streamed right now, its scores and any additional information.

When a specific team is being streamed you'll want the overlay in the stream to be changed. So instead of changing it on the stream program (something like OBS), the application changes it for you automatically using pre-prepared images to be placed at the team's folder.

You just need to take the desired team in the "existing" list at the left side and add it to the "playing" list at the right side. 

This application loads every folder inside a folder named "Teams"(that needs to be at the same place the .exe is) and put them in a list. To add any team, you must create a new folder with the team's name inside the "Teams" folder

You also need a score folder in the same place that the .exe is, telling how many teams there will be, and its images for each point in the score.

In the end, you'll need to have this in your file system:

--- overlayChanger.exe

--- Teams

--- Score

---       -¬

---         --- Team 1

---         --- Team 2

                 ...(Any amount you want, but the folder must exist))                 
