import { Routes } from '@angular/router';

// Importa o componente Curriculo
import { Curriculo } from './curriculo/curriculo';

export const routes: Routes = [
  {
    path: 'curriculo',
    component: Curriculo,
  },
  {
    path: '',
    redirectTo: 'curriculo',
    pathMatch: 'full',
  },
];
