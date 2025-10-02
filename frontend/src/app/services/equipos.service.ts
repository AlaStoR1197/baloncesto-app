import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Equipo {
  id: number;
  nombre: string;
  ciudad: string;
  logo?: string;
  creadoEn: string;
}

@Injectable({
  providedIn: 'root'
})
export class EquiposService {
  private apiUrl = 'http://localhost:5216/api/equipos';

  constructor(private http: HttpClient) { }

  getEquipos(): Observable<Equipo[]> {
    return this.http.get<Equipo[]>(this.apiUrl);
  }

  getEquipo(id: number): Observable<Equipo> {
    return this.http.get<Equipo>(`${this.apiUrl}/${id}`);
  }

  crearEquipo(equipo: Omit<Equipo, 'id' | 'creadoEn'>): Observable<Equipo> {
    return this.http.post<Equipo>(this.apiUrl, equipo);
  }
}