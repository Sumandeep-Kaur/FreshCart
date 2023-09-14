import { CanActivateFn } from '@angular/router';

export const authGuard: CanActivateFn = () => {
  if(localStorage.getItem("LoggedIn"))
    return true;
  return false;
};
