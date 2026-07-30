import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GameApiService } from './game-api.service';
import { ComputerDifficulty, GameMode, GameStateResponse, Player, ScoreboardResponse } from './models';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  selectedMode: GameMode = 'TwoPlayer';
  selectedDifficulty: ComputerDifficulty = 'Medium';
  gameState: GameStateResponse | null = null;
  scoreboard: ScoreboardResponse = { xWins: 0, oWins: 0, draws: 0 };
  errorMessage = '';
  isBusy = false;

  constructor(private readonly api: GameApiService) {}

  ngOnInit(): void {
    this.refreshScoreboard();
    this.createGame();
  }

  get statusMessage(): string {
    if (!this.gameState) {
      return 'Loading game...';
    }

    if (this.gameState.status === 'Won') {
      return `Winner: ${this.gameState.winner}`;
    }

    if (this.gameState.status === 'Draw') {
      return 'Game ended in a draw';
    }

    return `Current turn: ${this.gameState.currentPlayer}`;
  }

  get canUndo(): boolean {
    return !!this.gameState && this.gameState.status === 'InProgress' && this.gameState.moveHistory.length > 0;
  }

  get board(): (Player | null)[][] {
    return this.gameState?.board ?? [
      [null, null, null],
      [null, null, null],
      [null, null, null]
    ];
  }

  onModeChange(): void {
    this.createGame();
  }

  onCellClick(row: number, column: number): void {
    if (!this.gameState || this.gameState.status !== 'InProgress' || this.gameState.board[row][column]) {
      return;
    }

    this.errorMessage = '';
    this.isBusy = true;

    this.api
      .makeMove(this.gameState.gameId, {
        player: this.gameState.currentPlayer,
        row,
        column
      })
      .subscribe({
        next: (state) => {
          this.gameState = state;
          this.scoreboard = state.scoreboard;
          this.isBusy = false;
        },
        error: (err) => {
          this.errorMessage = this.getErrorMessage(err);
          this.isBusy = false;
        }
      });
  }

  undoLastMove(): void {
    if (!this.gameState || !this.canUndo) {
      return;
    }

    this.errorMessage = '';
    this.isBusy = true;

    this.api.undo(this.gameState.gameId).subscribe({
      next: (state) => {
        this.gameState = state;
        this.scoreboard = state.scoreboard;
        this.isBusy = false;
      },
      error: (err) => {
        this.errorMessage = this.getErrorMessage(err);
        this.isBusy = false;
      }
    });
  }

  resetGame(): void {
    if (!this.gameState) {
      return;
    }

    this.errorMessage = '';
    this.isBusy = true;

    this.api.resetGame(this.gameState.gameId).subscribe({
      next: (state) => {
        this.gameState = state;
        this.scoreboard = state.scoreboard;
        this.isBusy = false;
      },
      error: (err) => {
        this.errorMessage = this.getErrorMessage(err);
        this.isBusy = false;
      }
    });
  }

  resetScoreboard(): void {
    this.errorMessage = '';
    this.isBusy = true;

    this.api.resetScoreboard().subscribe({
      next: (scoreboard) => {
        this.scoreboard = scoreboard;
        if (this.gameState) {
          this.gameState = {
            ...this.gameState,
            scoreboard
          };
        }
        this.isBusy = false;
      },
      error: (err) => {
        this.errorMessage = this.getErrorMessage(err);
        this.isBusy = false;
      }
    });
  }

  isWinningCell(row: number, column: number): boolean {
    return !!this.gameState?.winningCells.some((cell) => cell.row === row && cell.column === column);
  }

  cellPositionLabel(row: number, column: number): string {
    return `Row ${row + 1}, Column ${column + 1}`;
  }

  trackByMoveNumber(_: number, move: { moveNumber: number }): number {
    return move.moveNumber;
  }

  private createGame(): void {
    this.errorMessage = '';
    this.isBusy = true;

    this.api.createGame({ mode: this.selectedMode, difficulty: this.selectedDifficulty }).subscribe({
      next: (state) => {
        this.gameState = state;
        this.scoreboard = state.scoreboard;
        this.isBusy = false;
      },
      error: (err) => {
        this.errorMessage = this.getErrorMessage(err);
        this.isBusy = false;
      }
    });
  }

  private refreshScoreboard(): void {
    this.api.getScoreboard().subscribe({
      next: (scoreboard) => {
        this.scoreboard = scoreboard;
      }
    });
  }

  private getErrorMessage(error: unknown): string {
    const fallback = 'Something went wrong while talking to the server.';
    if (!error || typeof error !== 'object') {
      return fallback;
    }

    const typed = error as { error?: unknown };
    if (typeof typed.error === 'string') {
      return typed.error;
    }

    return fallback;
  }
}
