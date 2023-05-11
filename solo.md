# General idea on how solo LB should function

## Users login through discord ID
Data gets send by both users to verify who wins. Needs some thinking on how to implement this exactly.

Potential system to prevent cheating/code injection:

Client breadcrumb log.
Each client sends data every x seconds. For big discrepencies at the end this breadcrumb log can be checked to verify whether the solo game is valid or not?

Perhaps add automatic flagging for "suspicious" games

## points and leaderboard
Discord users need to be verified if they are accounts < x weeks old/on the server.
This prevents multi accounting.

Unverified/new accounts are limited to a max of 50/100 elo in their "new user" phase for x days/weeks.
Exception cases are verified accounts, which can be done by a solo admin or some kind of role related to managing this.

Sharp rises/losses e.g. 100 elo gain in 1 day are put on a monitoring list.

All regular not logged in clients with DiscordId's are exempt from these leaderboards.
