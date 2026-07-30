import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  CreateGameRequest,
  GameStateResponse,
  MoveRequest,
  ScoreboardResponse
} from './models';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class GameApiService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private readonly http: HttpClient) {}

  createGame(payload: CreateGameRequest): Observable<GameStateResponse> {
    return this.http.post<GameStateResponse>(`${this.baseUrl}/api/games`, payload);
  }

  getGame(gameId: string): Observable<GameStateResponse> {
    return this.http.get<GameStateResponse>(`${this.baseUrl}/api/games/${gameId}`);
  }

  makeMove(gameId: string, payload: MoveRequest): Observable<GameStateResponse> {
    return this.http.post<GameStateResponse>(`${this.baseUrl}/api/games/${gameId}/moves`, payload);
  }

  undo(gameId: string): Observable<GameStateResponse> {
    return this.http.post<GameStateResponse>(`${this.baseUrl}/api/games/${gameId}/undo`, {});
  }

  resetGame(gameId: string): Observable<GameStateResponse> {
    return this.http.post<GameStateResponse>(`${this.baseUrl}/api/games/${gameId}/reset`, {});
  }

  getScoreboard(): Observable<ScoreboardResponse> {
    return this.http.get<ScoreboardResponse>(`${this.baseUrl}/api/scoreboard`);
  }

  resetScoreboard(): Observable<ScoreboardResponse> {
    return this.http.post<ScoreboardResponse>(`${this.baseUrl}/api/scoreboard/reset`, {});
  }
}
