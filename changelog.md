# Atlas changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 2025.07.31

### Fixed

- Fixed the settings which was not properly used in edge browser ([#102])
<!-- 2025.07.31 -->
[#102]: https://github.com/kappaduck/atlas/issues/102

## 2025.07.01

### Added

- First major release of Atlas
- Added a 'Give up' button to the game
- Added a new difficulty: 'Grayscale'
- Added countries section in the settings modal to display the list of countries ([#16])

### Changed

- Replace bug link to a feedback menu ([#33])
- Changed difficulty section to use a radio button instead of checkbox
- Optimized web app to use AOT compilation and IL stripping (native wasm)
- Optimized the performance of the web app (lookup, caching, etc.)
- Optimized the performance of Prometheus
- Prometheus is seperated from Atlas workflow with a dedicated workflow to update countries data
- Used a new versioning scheme for the project (YYYY.MM.DD)

### Fixed

- Fixed the zoom feature to correctly display the flag on mobile devices

<!-- 2025.07.01 -->
[#16]: https://github.com/kappaduck/atlas/issues/16
[#33]: https://github.com/kappaduck/atlas/issues/33

## 0.4.1 &#8212; 2024-11-29

### Fixed

- Fixed dependabot paths for nuget packages

## 0.4.0 &#8212; 2024-11-29

### Added

- Added a settings modal
- Can see changelog by clicking on the version or settings icon to open the modal (changelog section)
- Can manage theme and language (french and english) in the settings modal (general section)
- Can manage difficulty in the settings modal (difficulty section)
- Added dev mode for testing country. Only in stage site
- Added a zoom feature when clicking on the image
- Added a button to return to the index page via games

### Changed

- Improve pipelines to deploy the application 
- Improve the performance to lookup countries by initials
- Update Atlas and Prometheus to .NET 9.0
- Update tests project using XUnit 3

## 0.3.0 &#8212; 2024-11-09

### Added

- Added new game mode: Daily flag
- Added a game list on the index page
- Added Bunit tests for testing components
- Lookup input can accept initials of a country name

### Changed

- Improve the links to be more intuitive
- Improve the algorithm to remove diacritics from the country names when guessing
- Improve the performance to get all countries for the lookup input
- Improve the performance to get a specific country when guessing
- Improve the unit tests
- Improve the source code structure to implement new games easier

### Fixed

- Fixed Random flag game where the flag wasn't displayed when navigating back to the game
- Fixed Lookup input to keep the focus when pressing the `Escape` key

## 0.2.1 &#8212; 2024-09-16

### Fixed

- Hide logs in production
- Pressing `Enter` key will select the exact country name in any order from the list
- External links will open in a new tab

## 0.2.0 &#8212; 2024-09-16

### Added

- Mentions used projects in the `README.md` file
- Scroll to the autocomplete input when having the focus for mobile devices
- Extract flag image uri and map uri from API to use in the game
- Distinct environment between preview and production
- Added 404 page

### Changed

- Filter guessed countries from the autocomplete list
- Hide the autocomplete and display answer or congralute you when the game is over
- Change javascript to typescript
- Simplify language code for translations
- Improve icons usage

### Fixed

- Improve randomized flag image quality
- Pressing `Enter` key on an exact country name will select the first country in the list corresponding to the name
- Hide logs in production

## 0.1.0 &#8212; 2024-09-11

### Added

- Initial release
- Add randomized flag game
