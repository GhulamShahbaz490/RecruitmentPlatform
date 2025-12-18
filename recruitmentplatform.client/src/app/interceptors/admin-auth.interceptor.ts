import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const adminAuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const adminToken = authService.getAdminToken();

  if (adminToken && req.url.includes('/api/admin')) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${adminToken}`
      }
    });
    return next(cloned);
  }

  return next(req);
};