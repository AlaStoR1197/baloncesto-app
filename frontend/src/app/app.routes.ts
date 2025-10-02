import { Routes } from '@angular/router';
import { TableroComponent } from './components/tablero/tablero.component';
import { LoginComponent } from './components/login/login.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: TableroComponent },
  { path: 'login', component: LoginComponent },
  { 
    path: 'admin', 
    component: AdminPanelComponent,
    canActivate: [authGuard]  // Proteger esta ruta
  },
  { path: '**', redirectTo: '' }
];