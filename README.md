# YouTube to Podcast

Convert YouTube channels into podcast RSS feeds.

## How To

![How To](img/howTo.gif)

- Copy url to YouTube channel.
- Paste it into the app. Optionally provide filtering criteria.
- Copy generated url to RSS feed.
- Paste generated url into your podcast player.

## Features

- RSS (XML) feed generation
- Caching
- Filtering by duration and title
- Installable (PWA)

## Build

- Run `build.cmd` script from tools directory.
- Provide YouTube API key under `"YouTubeApi:Key"`.

## Requirements

- .NET Core
- LibMan CLI
- EF Core CLI
- Python (optional, required by alternative scrapper implementation)