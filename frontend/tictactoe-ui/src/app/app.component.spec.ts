import { of } from 'rxjs';
import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { GameApiService } from './game-api.service';
import { GameStateResponse, ScoreboardResponse } from './models';

const scoreboard: ScoreboardResponse = {
  xWins: 0,
  oWins: 0,
  draws: 0
};

const initialGame: GameStateResponse = {
  gameId: '1',
  mode: 'TwoPlayer',
  difficulty: 'Medium',
  board: [
    [null, null, null],
    [null, null, null],
    [null, null, null]
  ],
  currentPlayer: 'X',
  status: 'InProgress',
  winner: null,
  winningCells: [],
  moveHistory: [],
  scoreboard
};

describe('AppComponent', () => {
  beforeEach(async () => {
    const apiStub: Partial<GameApiService> = {
      getScoreboard: () => of(scoreboard),
      createGame: () => of(initialGame),
      makeMove: () => of(initialGame),
      undo: () => of(initialGame),
      resetGame: () => of(initialGame),
      resetScoreboard: () => of(scoreboard)
    };

    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [{ provide: GameApiService, useValue: apiStub }]
    }).compileComponents();
  });

  it('should create and show title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const root = fixture.nativeElement as HTMLElement;

    expect(root.querySelector('h1')?.textContent).toContain('Tic Tac Toe Arena');
  });

  it('should disable undo with no moves', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();

    expect(fixture.componentInstance.canUndo).toBeFalse();
  });
});
