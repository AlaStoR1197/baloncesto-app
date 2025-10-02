import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Partido, Equipo } from '../../models/partido.model';

@Component({
  selector: 'app-tablero',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tablero.component.html',
  styleUrls: ['./tablero.component.scss']
})
export class TableroComponent implements OnInit, OnDestroy {
  partido: Partido = {
    equipoLocal: { id: 1, nombre: 'Lakers', ciudad: 'Los Angeles' },
    equipoVisitante: { id: 2, nombre: 'Warriors', ciudad: 'San Francisco' },
    puntosLocal: 0,
    puntosVisitante: 0,
    cuartoActual: 1,
    tiempoRestante: '10:00',
    faltasLocal: 0,
    faltasVisitante: 0,
    tiempoCorriendo: false
  };

  private intervalo: any;
  private tiempoSegundos: number = 600; // 10 minutos en segundos

  ngOnInit() {
    this.iniciarTemporizador();
  }

  ngOnDestroy() {
    this.detenerTemporizador();
  }

  // Control del temporizador
  iniciarTemporizador() {
    if (!this.partido.tiempoCorriendo) {
      this.partido.tiempoCorriendo = true;
      this.intervalo = setInterval(() => {
        if (this.tiempoSegundos > 0) {
          this.tiempoSegundos--;
          this.actualizarTiempoDisplay();
        } else {
          this.finalizarCuarto();
        }
      }, 1000);
    }
  }

  pausarTemporizador() {
    this.partido.tiempoCorriendo = false;
    clearInterval(this.intervalo);
  }

  reiniciarTemporizador() {
    this.tiempoSegundos = 600;
    this.actualizarTiempoDisplay();
    this.pausarTemporizador();
  }

  private detenerTemporizador() {
    clearInterval(this.intervalo);
  }

  private actualizarTiempoDisplay() {
    const minutos = Math.floor(this.tiempoSegundos / 60);
    const segundos = this.tiempoSegundos % 60;
    this.partido.tiempoRestante = `${minutos.toString().padStart(2, '0')}:${segundos.toString().padStart(2, '0')}`;
  }

  // Control de puntos
  sumarPuntos(equipo: 'local' | 'visitante', puntos: number) {
    if (equipo === 'local') {
      this.partido.puntosLocal += puntos;
    } else {
      this.partido.puntosVisitante += puntos;
    }
  }

  restarPuntos(equipo: 'local' | 'visitante', puntos: number) {
    if (equipo === 'local') {
      this.partido.puntosLocal = Math.max(0, this.partido.puntosLocal - puntos);
    } else {
      this.partido.puntosVisitante = Math.max(0, this.partido.puntosVisitante - puntos);
    }
  }

  // Control de faltas
  sumarFalta(equipo: 'local' | 'visitante') {
    if (equipo === 'local') {
      this.partido.faltasLocal++;
    } else {
      this.partido.faltasVisitante++;
    }
  }

  restarFalta(equipo: 'local' | 'visitante') {
    if (equipo === 'local') {
      this.partido.faltasLocal = Math.max(0, this.partido.faltasLocal - 1);
    } else {
      this.partido.faltasVisitante = Math.max(0, this.partido.faltasVisitante - 1);
    }
  }

  // Control de cuartos
  siguienteCuarto() {
    if (this.partido.cuartoActual < 4) {
      this.partido.cuartoActual++;
      this.reiniciarTemporizador();
    }
  }

  cuartoAnterior() {
    if (this.partido.cuartoActual > 1) {
      this.partido.cuartoActual--;
      this.reiniciarTemporizador();
    }
  }

  private finalizarCuarto() {
    this.pausarTemporizador();
    // Aquí podrías agregar una notificación o sonido
    console.log(`¡Cuarto ${this.partido.cuartoActual} finalizado!`);
  }

  reiniciarPartido() {
    this.partido.puntosLocal = 0;
    this.partido.puntosVisitante = 0;
    this.partido.faltasLocal = 0;
    this.partido.faltasVisitante = 0;
    this.partido.cuartoActual = 1;
    this.reiniciarTemporizador();
  }
}