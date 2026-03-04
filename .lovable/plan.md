

# Cosmic Fruits — Browser-Based HTML5 Game

A colorful kids' catcher game built with React and HTML5 Canvas, featuring CSS/SVG art.

## Screens & Flow

### 1. Main Menu
- Animated space background with twinkling stars (CSS)
- SVG Astro-Cat character waving
- Three buttons: **PLAY**, **LEVELS**, **SOUND** toggle
- Smooth fade transitions between screens

### 2. Level Select (Planet Map)
- 10 planet icons in a grid layout
- Unlocked planets: bright colorful SVG circles with unique themes
- Locked planets: greyed out with padlock icon
- Progress saved to localStorage

### 3. Game Screen (Canvas-based gameplay)
- **Countdown sequence**: 3 → 2 → 1 → GO! with animated numbers
- **Astro-Cat**: SVG cat at bottom, moves left/right
- **Falling items**: Colorful CSS/SVG fruits (apples, oranges, stars) fall from top
- **Green slime**: Enemy blobs that freeze player for 1 second
- **Robo-Bot**: Flies across top every 30-40s dropping power-ups
- **Controls**: Arrow keys / A+D on desktop, tap left/right halves on mobile
- **HUD**: Progress bar (top-left), score (top-right), pause button

### 4. Win Screen
- "LEVEL COMPLETE!" text with bouncing star animations
- 3 star rating display
- "Next Level" button

## 10 Levels with Unique Themes
Each level has a distinct CSS gradient background and color palette:
1. **Green Terra** — green meadows, slow speed (tutorial)
2. **Orange Dune** — sandy desert tones
3. **Mechano** — grey/metal, Robo-Bot introduced
4. **Neon City** — dark with neon colors, faster items
5. **Ice Igloo** — blue/white, slime enemies introduced
6. **Crystallia** — purple crystals, more items
7. **Aquamarine** — underwater blue, items sway side-to-side
8. **Magma Prime** — red/orange lava, lots of slime
9. **Foggy Void** — dark space, items fade in/out
10. **Heart of Galaxy** — max speed, fireworks on completion

## Game Mechanics
- Items spawn at random X positions with increasing speed per level
- Collision detection between basket and falling items
- Progress bar fills as fruits are caught; full bar = level complete
- Magnet power-up attracts all items for 5 seconds
- Star power-up fills progress bar faster
- No game over — slime only delays/freezes, never ends the game
- Game state managed with React state + requestAnimationFrame game loop

## Technical Approach
- Game loop using `requestAnimationFrame` with a Canvas element
- SVG characters and items drawn programmatically
- localStorage for saving level progress and high scores
- Responsive sizing for mobile and desktop
- Touch controls for mobile (tap left/right screen halves)

