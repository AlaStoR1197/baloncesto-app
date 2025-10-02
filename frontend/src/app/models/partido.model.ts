export interface Partido {
  id?: number;
  equipoLocal: Equipo;
  equipoVisitante: Equipo;
  puntosLocal: number;
  puntosVisitante: number;
  cuartoActual: number;
  tiempoRestante: string;
  faltasLocal: number;
  faltasVisitante: number;
  tiempoCorriendo: boolean;
}

export interface Equipo {
  id: number;
  nombre: string;
  ciudad: string;
  logo?: string;
}