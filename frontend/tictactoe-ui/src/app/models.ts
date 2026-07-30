export type Player = 'X' | 'O';
export type GameMode = 'TwoPlayer' | 'VsComputer';
export type GameStatus = 'InProgress' | 'Won' | 'Draw';
export type ComputerDifficulty = 'Easy' | 'Medium' | 'Hard';

export interface CellPosition {
  row: number;
  column: number;
}

export interface MoveResponse {
  moveNumber: number;
  player: Player;
  row: number;
  column: number;
}

export interface ScoreboardResponse {
  xWins: number;
  oWins: number;
  draws: number;
}

export interface GameStateResponse {
  gameId: string;
  mode: GameMode;
  difficulty: ComputerDifficulty;
  board: (Player | null)[][];
  currentPlayer: Player;
  status: GameStatus;
  winner: Player | null;
  winningCells: CellPosition[];
  moveHistory: MoveResponse[];
  scoreboard: ScoreboardResponse;
}

export interface CreateGameRequest {
  mode: GameMode;
  difficulty: ComputerDifficulty;
}

export interface MoveRequest {
  player: Player;
  row: number;
  column: number;
}
