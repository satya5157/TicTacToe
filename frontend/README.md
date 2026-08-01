# Frontend - tictactoe-ui

Angular source code is in `frontend/tictactoe-ui`.

## Software Required
- Git 2.40+
- Node.js 20+ (Node 22 or 24 also works)
- npm 10+
- Modern browser (Chrome/Edge/Firefox)

Optional for unit tests:

- Chrome (for Karma ChromeHeadless)

Check versions:

- `git --version`
- `node --version`
- `npm --version`

## Run
1. Install Node.js 20+ and npm
2. From `frontend/tictactoe-ui`:
   - `npm install`
   - `npm start`
3. Open `http://localhost:4200`

## Verify UI
- Open `http://localhost:4200`
- Choose Two Player or Play Against Computer
- For computer mode, choose Easy/Medium/Hard difficulty
- On win or draw, a popup message appears and closes automatically after 3 seconds
- Make moves and verify scoreboard, undo, and reset actions

## Test
From `frontend/tictactoe-ui`:
- `npm test`

## API Base URL
Configured in `src/environments/environment.ts` as:
- `http://localhost:5249`
