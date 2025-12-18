import { HttpInterceptorFn } from '@angular/common/http';
import Swal from 'sweetalert2';
import { finalize } from 'rxjs';

let activeRequests = 0;

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  // allow skipping the global loading indicator by setting header 'x-skip-loading'
  if (req.headers.has('x-skip-loading')) {
    const cloned = req.clone({ headers: req.headers.delete('x-skip-loading') });
    return next(cloned);
  }

  const methodsToShow = ['GET', 'POST', 'PUT', 'DELETE', 'PATCH'];
  const shouldShow = methodsToShow.includes(req.method.toUpperCase());

  if (shouldShow) {
    if (activeRequests === 0) {
      Swal.fire({
        title: 'Loading...',
        allowOutsideClick: false,
        didOpen: () => {
          Swal.showLoading();
        }
      });
    }
    activeRequests++;
  }

  return next(req).pipe(
    finalize(() => {
      if (shouldShow) {
        activeRequests--;
        if (activeRequests <= 0) {
          activeRequests = 0;
          Swal.close();
        }
      }
    })
  );
};
